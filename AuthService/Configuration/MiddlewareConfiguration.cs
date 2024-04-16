using AuthService.Infrastructure.Middleware;

namespace AuthService.Configuration;

public static class MiddlewareConfiguration
{
    public static void AddMiddlewares(IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionsMiddleware>();
    }
}