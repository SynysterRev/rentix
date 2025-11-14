using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rentix.Application.Exceptions;

namespace Rentix.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IWebHostEnvironment _appEnv;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment appEnv)
        {
            _logger = logger;
            _appEnv = appEnv;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";
            int statusCode;
            object responseBody;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    responseBody = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "Resource Not Found",
                        Detail = exception.Message,
                        Instance = httpContext.Request.Path
                    };
                    break;
                case ValidationException validationException:
                    statusCode = StatusCodes.Status400BadRequest;

                    var errors = validationException.Errors
                        .GroupBy(err => err.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(err => err.ErrorMessage).ToArray()
                        );

                    responseBody = new ValidationProblemDetails(errors)
                    {
                        Status = statusCode,
                        Title = "One or more validation errors occurred.",
                        Instance = httpContext.Request.Path,
                        Detail = _appEnv.IsDevelopment() ? exception.StackTrace : null
                    };
                    break;
                case ArgumentException:
                    statusCode = StatusCodes.Status400BadRequest;
                    responseBody = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "Bad Request",
                        Detail = exception.Message,
                        Instance = httpContext.Request.Path
                    };
                    break;
                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    responseBody = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "Unauthorized",
                        Detail = exception.Message,
                        Instance = httpContext.Request.Path
                    };
                    break;
                case InvalidOperationException:
                    statusCode = StatusCodes.Status500InternalServerError;
                    responseBody = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "Configuration Error",
                        Detail = _appEnv.IsDevelopment() ? exception.Message : "A configuration error occurred.",
                        Instance = httpContext.Request.Path
                    };
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    responseBody = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "Internal Server Error",
                        Detail = _appEnv.IsDevelopment() ? exception.Message : "An unexpected error occurred.",
                        Instance = httpContext.Request.Path
                    };
                    break;
            }

            _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(responseBody, cancellationToken);

            return true;
        }
    }
}
