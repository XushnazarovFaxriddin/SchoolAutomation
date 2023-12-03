using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAutomation.Data;
using SchoolAutomation.Enums;
using SchoolAutomation.Helpers;
using SchoolAutomation.Models;
using SchoolAutomation.Models.DbModels;
using SchoolAutomation.Services;

namespace SchoolAutomation.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly SchoolDbContext _dbContext;
    private readonly IJwtTokenService _jwtTokenService;
    public AuthController(SchoolDbContext dbContext, IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
    }
    /// <summary>
    /// Iltimos rolingizni tanlang
    /// </summary>
    /// <param name="loginModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        if (loginModel.Role == RoleType.Admin)
        {
            var admin = await _dbContext.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == loginModel.Username);
            if (admin is null || !CryptoHelper.VerifyPassword(loginModel.Password, admin.Password))
                return BadRequest(new { message = "Username or Password is incorrect" });
            admin.Password = null;
            var token = await _jwtTokenService.GenerateJwtTokenAsync(admin);
            return Ok(new
            {
                token,
                user = admin
            });
        }
        if (loginModel.Role == RoleType.Teacher)
        {
            var teacher = await _dbContext.Teachers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == loginModel.Username);
            if (teacher is null || !CryptoHelper.VerifyPassword(loginModel.Password, teacher.Password))
                return BadRequest(new { message = "Username or Password is incorrect" });
            teacher.Password = null;
            var token = await _jwtTokenService.GenerateJwtTokenAsync(teacher);
            return Ok(new
            {
                token,
                user = teacher
            });
        }
        if (loginModel.Role == RoleType.Student)
        {
            var student = await _dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == loginModel.Username);
            if (student is null || !CryptoHelper.VerifyPassword(loginModel.Password, student.Password))
                return BadRequest(new { message = "Username or Password is incorrect" });
            student.Password = null;
            var token = _jwtTokenService.GenerateJwtTokenAsync(student);
            return Ok(new
            {
                token,
                user = student
            });
        }
        return BadRequest(new
        {
            message = "Please select Role"
        });
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var user = new User
        {
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            Username = registerModel.Username,
            Password = CryptoHelper.HashPassword(registerModel.Password),
            Role = registerModel.Role
        };
        dynamic response = user.Role switch
        {
            RoleType.Student => _dbContext.Students.Add(user.ToStudent()).Entity,
            RoleType.Teacher => _dbContext.Teachers.Add(user.ToTeacher()).Entity,
        };
        await _dbContext.SaveChangesAsync();
        response.Password = null;
        return Ok(response);
    }
}
