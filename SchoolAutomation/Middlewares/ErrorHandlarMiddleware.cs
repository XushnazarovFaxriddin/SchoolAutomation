namespace SchoolAutomation.Middlewares
{
    public class ErrorHandlarMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlarMiddleware(RequestDelegate next)
        => _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = ex switch
                {
                    KeyNotFoundException => 404,
                    UnauthorizedAccessException => 401,
                    ArgumentException => 400,
                    MemberAccessException => 403,
                    _ => 500
                };
                await response.WriteAsJsonAsync(new
                {
                    response.StatusCode,
                    ex.Message
                });
            }
        }
    }
}
