namespace Huquqim.Application.Cases.Contracts;

public record CreateCaseRequest
{
    /// <summary>Foydalanuvchi muammosining erkin tavsifi (chatning birinchi xabari).</summary>
    public string Description { get; set; } = default!;

    /// <summary>Ixtiyoriy sarlavha. Bo'sh bo'lsa, tavsifdan generatsiya qilinadi.</summary>
    public string? Title { get; set; }
}
