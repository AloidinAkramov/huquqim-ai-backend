using Huquqim.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Huquqim.Api.Extensions;

/// <summary>
/// Result -> HTTP javob (muvaffaqiyat yoki RFC 7231 ProblemDetails).
/// </summary>
public static class ResultExtensions
{
    public static IResult ToOk<T>(this Result<T> result) =>
        result.IsSuccess ? Results.Ok(result.Data) : result.ToProblem();

    public static IResult ToOk(this Result result) =>
        result.IsSuccess ? Results.NoContent() : result.ToProblem();

    public static IResult ToProblem(this Result result)
    {
        var error = result.Error;
        var statusCode = GetStatusCode(error.Type);

        return Results.Problem(
            statusCode: statusCode,
            title: GetTitle(error.Type),
            type: GetTypeLink(error.Type),
            extensions: new Dictionary<string, object?>
            {
                ["errors"] = new
                {
                    code = error.Code,
                    type = error.Type.ToString(),
                    description = error.Message
                }
            });
    }

    private static int GetStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.PaymentRequired => StatusCodes.Status402PaymentRequired,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };

    private static string GetTitle(ErrorType type) => type switch
    {
        ErrorType.Validation => "Tekshiruv xatosi",
        ErrorType.Unauthorized => "Ruxsat yo'q",
        ErrorType.PaymentRequired => "Premium talab qilinadi",
        ErrorType.NotFound => "Topilmadi",
        ErrorType.Conflict => "Ziddiyat",
        _ => "Server xatosi"
    };

    private static string GetTypeLink(ErrorType type) => type switch
    {
        ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
        ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
    };
}
