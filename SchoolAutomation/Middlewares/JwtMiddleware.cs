
using SchoolAutomation.Services;

namespace SchoolAutomation.Middlewares;

public class JwtMiddleware 
{
    private readonly RequestDelegate _next;
    public JwtMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context, IJwtTokenService jwtTokenService, ILogger<JwtMiddleware> logger)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        try
        {
            var user = await jwtTokenService.GetUserFromJwtTokenAsync(token);
            context.Items["User"] = user;
        }catch (Exception ex)
        {
            logger.LogError(ex, "JwtMiddleware error!");
        }
        finally
        {
            await _next(context);
        }
    }
}
