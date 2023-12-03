using Microsoft.IdentityModel.Tokens;
using SchoolAutomation.Enums;
using SchoolAutomation.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolAutomation.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public Task<string> GenerateJwtTokenAsync(User user)
    {
        var claims = new List<Claim>{
            new Claim("Id", user.Id.ToString()),
            new Claim("Username", user.Username),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim("Role", user.Role.ToString())
        };

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Jwt:ScretKey"]));
        var expireTime = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:TokenExpiryInMinutes"]));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expireTime,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }

    public Task<User> GetUserFromJwtTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Task.FromResult<User?>(null);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:ScretKey"]);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var user = new User
            {
                Id = Convert.ToInt32(jwtToken.Claims.First(x => x.Type == "Id").Value),
                Username = jwtToken.Claims.First(x => x.Type == "Username").Value,
                FirstName = jwtToken.Claims.First(x => x.Type == "FirstName").Value,
                LastName = jwtToken.Claims.First(x => x.Type == "LastName").Value,
                Role = (RoleType)Enum.Parse(typeof(RoleType), jwtToken.Claims.First(x => x.Type == "Role").Value)
            };
            return Task.FromResult(user);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogError(ex, "Token has expired");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while validating token");
            return Task.FromResult<User?>(null);
        }
    }
}
