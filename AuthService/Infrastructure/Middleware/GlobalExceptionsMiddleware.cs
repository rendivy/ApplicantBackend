using AuthService.Domain.Exception;
using Common.CustomException;

namespace AuthService.Infrastructure.Middleware;

public class GlobalExceptionsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UserAlreadyHaveRoleException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (UserNotFoundException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (UserDoesntHavePermissionException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status403Forbidden, exception.Message);
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

    private class Error
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }
    }
}