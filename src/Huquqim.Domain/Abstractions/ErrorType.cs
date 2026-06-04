namespace Huquqim.Domain.Abstractions;

/// <summary>
/// Xatolik turlari. HTTP status kodiga moslashtiriladi.
/// </summary>
public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Success = 5,

    /// <summary>Premium (pullik tarif) talab qilinadi — HTTP 402.</summary>
    PaymentRequired = 6
}
