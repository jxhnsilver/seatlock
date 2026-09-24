using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.ErrorHandling
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, Exception exception, CancellationToken ct)
        {
            var (status, title, type) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Resource not found", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"),
                InvariantViolationException => (StatusCodes.Status422UnprocessableEntity, "Invariant violation", "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2"),
                BusinessRuleException => (StatusCodes.Status409Conflict, "Business rule violation", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8"),
                _ => (StatusCodes.Status500InternalServerError, "Internal server error", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1")
            };

            if (status == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(
                    exception,
                    "Unhandled exception while processing {HttpMethod} {RequestPath}. " +
                    "TraceId: {TraceId}. ExceptionType: {ExceptionType}",
                    context.Request.Method,
                    context.Request.Path,
                    context.TraceIdentifier,
                    exception.GetType().FullName);
            }

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = status == StatusCodes.Status500InternalServerError
                    ? null
                    : exception.Message,
                Instance = context.Request.Path,
                Type = type,
            }, ct);

            return true;
        }
    }
}
