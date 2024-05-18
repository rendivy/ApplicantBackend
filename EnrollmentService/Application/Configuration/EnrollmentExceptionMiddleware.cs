using Common.Exception;

namespace EnrollmentService.Application.Configuration;

public class EnrollmentExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (EnrollmentException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (Exception exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
    }


    private static async Task SetExceptionAsync(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new Error
        {
            StatusCode = status,
            Message = message
        });
    }

    private class UnauthorizedError
    {
        public int StatusCode { get; set; }
    }

    private class Error
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }
    }
}