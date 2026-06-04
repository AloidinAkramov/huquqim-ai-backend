namespace Huquqim.Application.Commons.Configurations;

/// <summary>
/// JWT sozlamalari (appsettings: "Jwt").
/// </summary>
public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = default!;

    public string Audience { get; set; } = default!;

    public string SecretKey { get; set; } = default!;

    /// <summary>Token amal qilish muddati (daqiqalarda).</summary>
    public int ExpiryMinutes { get; set; } = 60 * 24 * 7; // 7 kun
}
