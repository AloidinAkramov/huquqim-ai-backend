namespace Huquqim.Domain.Enums;

/// <summary>
/// Tayyorlanadigan hujjat turi (TZ Modul 3).
/// </summary>
public enum EDocumentType
{
    /// <summary>Da'vo arizasi.</summary>
    Claim = 0,

    /// <summary>Pretenziya (rasmiy talabnoma).</summary>
    Pretension = 1,

    /// <summary>Shikoyat.</summary>
    Complaint = 2,

    /// <summary>E'tiroz.</summary>
    Objection = 3,

    /// <summary>Tushuntirish xati.</summary>
    Explanatory = 4,

    /// <summary>Apellyatsiya shikoyati.</summary>
    Appeal = 5
}
