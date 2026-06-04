using Huquqim.Domain.Enums;

namespace Huquqim.Application.Identity.Contracts;

public record AuthResponse
{
    public string Token { get; init; } = default!;

    public DateTime ExpiresAt { get; init; }

    public UserResponse User { get; init; } = default!;
}

public record UserResponse
{
    public long Id { get; init; }

    public string FullName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public string? PhoneNumber { get; init; }

    public ESubscriptionTier SubscriptionTier { get; init; }

    public DateTime? SubscriptionExpiresAt { get; init; }

    /// <summary>Foydalanuvchi premium (pullik tarif)mi — frontend uchun qulaylik.</summary>
    public bool IsPremium { get; init; }
}
