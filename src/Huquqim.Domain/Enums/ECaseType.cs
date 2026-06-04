namespace Huquqim.Domain.Enums;

/// <summary>
/// Ish (huquqiy nizo) turi.
/// </summary>
public enum ECaseType
{
    /// <summary>Aniqlanmagan (triage hali tugamagan).</summary>
    Unknown = 0,

    /// <summary>Iste'molchi nizosi (buzuq tovar, sifatsiz xizmat).</summary>
    Consumer = 1,

    /// <summary>Mehnat nizosi (ish haqi, noqonuniy ishdan bo'shatish).</summary>
    Labor = 2,

    /// <summary>Ma'muriy huquqbuzarlik (jarima, protokol).</summary>
    Administrative = 3,

    /// <summary>Ijara nizosi (uy-joy, mulk ijarasi).</summary>
    Rent = 4,

    /// <summary>Fuqarolik nizosi (kichik qarz, zarar).</summary>
    Civil = 5,

    /// <summary>Advokat majburiy bo'lgan murakkab/jinoiy ish — yuristga yo'naltiriladi.</summary>
    RequiresLawyer = 99
}
