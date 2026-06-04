namespace Huquqim.Domain.Enums;

/// <summary>
/// Foydalanuvchi obuna tarifi (TZ 6-bo'lim biznes model).
/// </summary>
public enum ESubscriptionTier
{
    /// <summary>Bepul: holatni aniqlash + asosiy tushuntirish.</summary>
    Free = 0,

    /// <summary>Bir martalik: 1 ta to'liq ish (tushuntirish + hujjat + yo'riqnoma).</summary>
    OneTime = 1,

    /// <summary>Oylik obuna: cheksiz ishlar + saqlash + eslatmalar.</summary>
    Monthly = 2
}
