using Huquqim.Domain.Enums;

namespace Huquqim.Application.Identity.Contracts;

/// <summary>
/// Tarif sotib olish (yangilash) so'rovi.
/// DEMO: hozircha to'lov tekshirilmaydi, darrov premium qilinadi.
/// Keyinroq Click/Payme integratsiyasi qo'shiladi.
/// </summary>
public record UpgradeRequest
{
    /// <summary>Tanlangan tarif: OneTime (bir martalik) yoki Monthly (oylik).</summary>
    public ESubscriptionTier Tier { get; set; }
}
