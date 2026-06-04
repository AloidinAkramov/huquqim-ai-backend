namespace Huquqim.Application.Commons.Configurations;

/// <summary>
/// AI provayder sozlamalari (appsettings: "Ai"). Vertex AI (Google Gemini) ishlatiladi.
/// Autentifikatsiya service account JSON orqali ($300 Google kredit).
/// </summary>
public class AiOptions
{
    public const string SectionName = "Ai";

    /// <summary>Google Cloud project ID.</summary>
    public string ProjectId { get; set; } = default!;

    /// <summary>Vertex AI region, masalan: us-central1, global.</summary>
    public string Location { get; set; } = "us-central1";

    /// <summary>Model nomi, masalan: gemini-2.0-flash, gemini-2.5-flash.</summary>
    public string Model { get; set; } = "gemini-2.0-flash";

    /// <summary>Service account JSON fayl yo'li (GOOGLE_APPLICATION_CREDENTIALS).</summary>
    public string? CredentialsPath { get; set; }

    public int MaxTokens { get; set; } = 8192;
}
