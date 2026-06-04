namespace Huquqim.Domain.Enums;

/// <summary>
/// Suhbat xabarining muallifi (Claude API roli bilan mos).
/// </summary>
public enum EMessageRole
{
    /// <summary>Foydalanuvchi xabari.</summary>
    User = 0,

    /// <summary>AI yordamchi (Claude) javobi.</summary>
    Assistant = 1,

    /// <summary>Tizim xabari (ichki, ko'rsatilmaydi).</summary>
    System = 2
}
