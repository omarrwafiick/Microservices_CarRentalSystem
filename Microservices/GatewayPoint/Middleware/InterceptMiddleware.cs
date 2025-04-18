namespace GatewayPoint.Middleware
{
    public class InterceptMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["Referrer"] = "API-GATEWAY";
            await next(context);
        }
    }
}
