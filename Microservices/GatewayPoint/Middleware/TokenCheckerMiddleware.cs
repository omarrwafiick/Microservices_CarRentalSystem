namespace GatewayPoint.Middleware
{
    public class TokenCheckerMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var requestPath = context.Request.Path.Value;
            if(requestPath.Contains("auth/login", StringComparison.InvariantCultureIgnoreCase) ||
               requestPath.Contains("auth/register", StringComparison.InvariantCultureIgnoreCase) ||
               requestPath.Contains("auth/forgetpassword", StringComparison.InvariantCultureIgnoreCase) ||
               requestPath.Contains("auth/resetpassword", StringComparison.InvariantCultureIgnoreCase) ||
               requestPath.Equals("/")
            )
            {
                await next(context);
            }
            else
            {
                var authHeader = context.Request.Headers.Authorization;
                if(authHeader.FirstOrDefault() == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Access denied");
                }
                else
                {
                    await next(context);
                }
            }

        }
    }
}
