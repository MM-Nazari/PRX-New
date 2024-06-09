using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SecurityTokenException ex)
        {
            await HandleExceptionAsync(context, ex, StatusCodes.Status401Unauthorized, "Invalid token.");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string message)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new { error = message, details = exception.Message });
            await context.Response.WriteAsync(result);
        }
        else
        {
            // Log the error if the response has already started
            // Use your preferred logging framework here
            Console.WriteLine("Error occurred after response started: " + exception.Message);
        }
    }
}

// Extension method to add the middleware to the pipeline
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
