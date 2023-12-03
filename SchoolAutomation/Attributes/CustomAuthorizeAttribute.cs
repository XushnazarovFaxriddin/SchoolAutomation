using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolAutomation.Enums;
using SchoolAutomation.Models;

namespace SchoolAutomation.Attributes;

public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly IList<RoleType> _roles;
    public CustomAuthorizeAttribute(params RoleType[] roles)
    {
        _roles = roles;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<CustomAllowAnonymousAttribute>().Any())
            return Task.CompletedTask;

        try
        {
            var user = context.HttpContext.Items["User"] as User;

            if (user is null)
            {
                context.Result = new ObjectResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return Task.CompletedTask;
            }

            if (_roles.Any() && !_roles.Contains(user.Role))
            {
                context.Result = new ObjectResult(new { message = "Forbidden" })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return Task.CompletedTask;
            }

            return Task.CompletedTask;

        }
        catch
        {
            context.Result = new ObjectResult(new { message = "Internal Server Error" })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return Task.CompletedTask;
        }
    }
}
