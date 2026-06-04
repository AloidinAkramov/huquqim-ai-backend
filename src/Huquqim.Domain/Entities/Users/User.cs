using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Enums;

namespace Huquqim.Domain.Entities.Users;

/// <summary>
/// Tizim foydalanuvchisi (fuqaro). Email/parol bilan ro'yxatdan o'tadi.
/// </summary>
public class User : AuditableModelBase<long>
{
    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public string? PhoneNumber { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public ESubscriptionTier SubscriptionTier { get; set; } = ESubscriptionTier.Free;

    /// <summary>Obuna tugash sanasi (oylik tarif uchun).</summary>
    public DateTime? SubscriptionExpiresAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    // Navigatsiya
    public ICollection<Cases.Case> Cases { get; set; } = new List<Cases.Case>();
}
