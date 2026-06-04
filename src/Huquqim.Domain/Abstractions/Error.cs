namespace Huquqim.Domain.Abstractions;

/// <summary>
/// Operatsiya natijasidagi xatolikni ifodalovchi yozuv (record).
/// Kod, tur va xabardan iborat.
/// </summary>
public record Error
{
    public static readonly Error None = new(string.Empty, ErrorType.Success);

    public Error(string code, ErrorType type, string message = "")
    {
        Code = code;
        Type = type;
        Message = message;
    }

    public string Code { get; }

    public ErrorType Type { get; }

    public string Message { get; }

    public static Error Failure(string code, string message = "") => new(code, ErrorType.Failure, message);

    public static Error Validation(string code, string message = "") => new(code, ErrorType.Validation, message);

    public static Error NotFound(string code, string message = "") => new(code, ErrorType.NotFound, message);

    public static Error Conflict(string code, string message = "") => new(code, ErrorType.Conflict, message);

    public static Error Unauthorized(string code, string message = "") => new(code, ErrorType.Unauthorized, message);

    public static Error PaymentRequired(string code, string message = "") => new(code, ErrorType.PaymentRequired, message);

    public static readonly Error InternalServerError =
        new("Server.InternalError", ErrorType.Failure, "Ichki server xatosi yuz berdi.");
}
