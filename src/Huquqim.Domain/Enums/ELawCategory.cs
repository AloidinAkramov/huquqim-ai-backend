namespace Huquqim.Domain.Enums;

/// <summary>
/// Qonun manbasi toifasi (bilim bazasi indeksi uchun).
/// </summary>
public enum ELawCategory
{
    /// <summary>Fuqarolik kodeksi.</summary>
    CivilCode = 0,

    /// <summary>Ma'muriy javobgarlik to'g'risidagi kodeks.</summary>
    AdministrativeCode = 1,

    /// <summary>Iste'molchilar huquqlarini himoya qilish to'g'risidagi qonun.</summary>
    ConsumerProtection = 2,

    /// <summary>Mehnat kodeksi.</summary>
    LaborCode = 3,

    /// <summary>Fuqarolik protsessual kodeksi.</summary>
    CivilProcedureCode = 4,

    /// <summary>Boshqa qonun yoki qaror.</summary>
    Other = 99
}
