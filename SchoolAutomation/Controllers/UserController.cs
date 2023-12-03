using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAutomation.Attributes;
using SchoolAutomation.Data;
using SchoolAutomation.Enums;

namespace SchoolAutomation.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly SchoolDbContext _dbContext;

    public UserController(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet, CustomAuthorize(RoleType.Admin)]
    public async Task<IActionResult> Teachers()
    {
        var teachers = await _dbContext.Teachers
            .AsNoTracking()
            .ToListAsync();

        return Ok(teachers);
    }

    [HttpGet, CustomAuthorize(RoleType.Admin, RoleType.Teacher)]
    public async Task<IActionResult> Students()
    {
        var students = await _dbContext.Students
            .AsNoTracking()
            .ToListAsync();
        return Ok(students);
    }

    [HttpGet]
    public async Task<IActionResult> Admins()
    {
        var admins = await _dbContext.Admins
            .AsNoTracking()
            .ToListAsync();
        return Ok(admins);
    }
}
