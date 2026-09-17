using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.ErrorHandling
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, Exception exception, CancellationToken ct)
        {
            var (status, title, type) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not found", "https://tools.ietf.org/html/rfc7231#section-6.5.4"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict", "https://tools.ietf.org/html/rfc7231#section-6.5.8"),
                BusinessRuleException => (StatusCodes.Status400BadRequest, "Bad request", "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                _ => (StatusCodes.Status500InternalServerError, "Internal server error", "https://tools.ietf.org/html/rfc7231#section-6.6.1")
            };

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
