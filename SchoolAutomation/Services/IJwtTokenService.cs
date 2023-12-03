using SchoolAutomation.Models;

namespace SchoolAutomation.Services;

public interface IJwtTokenService
{
    Task<string> GenerateJwtTokenAsync(User user);
    Task<User> GetUserFromJwtTokenAsync(string token);
}
