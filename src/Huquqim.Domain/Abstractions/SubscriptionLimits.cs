using Huquqim.Domain.Enums;

namespace Huquqim.Domain.Abstractions;

/// <summary>
/// Tarif (obuna) bo'yicha biznes cheklovlari.
/// Bepul foydalanuvchi cheklangan, pullik tariflar to'liq imkoniyatga ega.
/// </summary>
public static class SubscriptionLimits
{
    /// <summary>Bepul tarifda bitta ish bo'yicha ruxsat etilgan AI xabarlari soni.</summary>
    public const int FreeMessageLimit = 2;

    /// <summary>Foydalanuvchi premium (pullik)mi — obuna amal qiladimi.</summary>
    public static bool IsPremium(ESubscriptionTier tier, DateTime? expiresAt, DateTime now)
    {
        return tier switch
        {
            ESubscriptionTier.OneTime => true,
            ESubscriptionTier.Monthly => expiresAt is null || expiresAt > now,
            _ => false
        };
    }

    /// <summary>Hujjat generatsiyasi faqat premium uchun.</summary>
    public static bool CanGenerateDocuments(ESubscriptionTier tier, DateTime? expiresAt, DateTime now)
        => IsPremium(tier, expiresAt, now);
}
