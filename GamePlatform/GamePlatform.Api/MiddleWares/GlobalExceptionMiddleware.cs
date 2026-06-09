using GamePlatform.Api.Application.Common;
using GamePlatform.Api.Application.Common.CustomExceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message) = MapException(ex);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = ApiResult<string>.Fail(message);

        await context.Response.WriteAsync(
            System.Text.Json.JsonSerializer.Serialize(response));
    }

    private static (int statusCode, string message) MapException(Exception ex)
    {
        return ex switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, ex.Message),

            BusinessException => (StatusCodes.Status400BadRequest, ex.Message),

            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }

}