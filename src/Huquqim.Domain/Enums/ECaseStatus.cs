namespace Huquqim.Domain.Enums;

/// <summary>
/// Ishning joriy holati (foydalanuvchi yo'lining qaysi bosqichida).
/// </summary>
public enum ECaseStatus
{
    /// <summary>Yangi ochilgan, triage jarayonida.</summary>
    Triage = 0,

    /// <summary>Holat tushuntirildi, huquqlar ko'rsatildi.</summary>
    Explained = 1,

    /// <summary>Hujjat tayyorlanmoqda.</summary>
    DocumentDrafting = 2,

    /// <summary>Hujjat tayyor, yuklab olindi.</summary>
    DocumentReady = 3,

    /// <summary>Sudga tayyorgarlik bosqichida.</summary>
    Preparing = 4,

    /// <summary>Ish yopildi (yakunlandi).</summary>
    Closed = 5,

    /// <summary>Yuristga yo'naltirildi (advokat majburiy).</summary>
    ReferredToLawyer = 6
}
