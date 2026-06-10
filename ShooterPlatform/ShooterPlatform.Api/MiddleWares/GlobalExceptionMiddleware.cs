using System.Text.Json;
using FluentValidation;
using ShooterPlatform.Api.Application.Common;
using ShooterPlatform.Api.Application.Common.CustomExceptions;

namespace ShooterPlatform.Api.MiddleWares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
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
            var (statusCode, message, errorCode, errors) = MapException(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiResult<string>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                Errors = errors
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static (int statusCode, string message, string? errorCode, object? errors)
            MapException(Exception ex)
        {
            return ex switch
            {
                NotFoundException => (
                    StatusCodes.Status404NotFound,
                    ex.Message,
                    "NOT_FOUND",
                    null
                ),

                BusinessException => (
                    StatusCodes.Status400BadRequest,
                    ex.Message,
                    "BUSINESS_ERROR",
                    null
                ),

                ValidationException ve => (
                    StatusCodes.Status400BadRequest,
                    "Validation failed",
                    "VALIDATION_ERROR",
                    ve.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        )
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "SERVER_ERROR",
                    null
                )
            };
        }
    }
}