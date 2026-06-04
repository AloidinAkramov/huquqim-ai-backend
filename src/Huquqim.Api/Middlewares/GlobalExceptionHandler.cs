using System.Net;
using Huquqim.Domain.Abstractions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Huquqim.Api.Middlewares;

/// <summary>
/// Ushlanmagan istisnolarni RFC 7231 ProblemDetails formatiga o'tkazadi.
/// </summary>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Ushlanmagan istisno: {Message}", exception.Message);

        var (statusCode, error) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, Error.Unauthorized("Auth.Unauthorized", "Ruxsat yo'q.")),
            _ => (HttpStatusCode.InternalServerError, Error.InternalServerError)
        };

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = environment.IsDevelopment() ? exception.Message : "Server xatosi",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Extensions =
            {
                ["errors"] = new
                {
                    code = error.Code,
                    type = error.Type.ToString(),
                    description = error.Message
                }
            }
        };

        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
