using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth.OAuth2;
using Huquqim.Application.Commons.Ai;
using Huquqim.Application.Commons.Configurations;
using Huquqim.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Huquqim.Infrastructure.Brokers.Ai;

/// <summary>
/// Google Vertex AI (Gemini) bilan ishlovchi broker. IClaudeBroker interfeysini
/// amalga oshiradi. Autentifikatsiya service account JSON orqali ($300 kredit).
/// </summary>
public class VertexBroker : IClaudeBroker
{
    private readonly HttpClient _httpClient;
    private readonly AiOptions _options;
    private readonly GoogleCredential _credential;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private const string Scope = "https://www.googleapis.com/auth/cloud-platform";

    public VertexBroker(HttpClient httpClient, IOptions<AiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        var baseCredential = string.IsNullOrWhiteSpace(_options.CredentialsPath)
            ? GoogleCredential.GetApplicationDefault()
            : GoogleCredential.FromFile(_options.CredentialsPath);

        _credential = baseCredential.CreateScoped(Scope);
    }

    public async Task<AiCompletion> CompleteAsync(
        string systemPrompt,
        IReadOnlyList<ChatMessage> messages,
        CancellationToken cancellationToken = default)
    {
        var token = await _credential.UnderlyingCredential
            .GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);

        var contents = messages
            .Where(m => m.Role != EMessageRole.System)
            .Select(m => new VxContent
            {
                Role = m.Role == EMessageRole.Assistant ? "model" : "user",
                Parts = [new VxPart { Text = m.Content }]
            })
            .ToList();

        var requestBody = new VxRequest
        {
            SystemInstruction = new VxContent { Parts = [new VxPart { Text = systemPrompt }] },
            Contents = contents,
            GenerationConfig = new VxGenConfig { MaxOutputTokens = _options.MaxTokens, Temperature = 0.4 }
        };

        var host = _options.Location == "global"
            ? "aiplatform.googleapis.com"
            : $"{_options.Location}-aiplatform.googleapis.com";

        var url = $"https://{host}/v1/projects/{_options.ProjectId}/locations/{_options.Location}" +
                  $"/publishers/google/models/{_options.Model}:generateContent";

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(requestBody, options: JsonOptions)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"Vertex {(int)response.StatusCode}: {errorBody}");
        }

        var result = await response.Content.ReadFromJsonAsync<VxResponse>(JsonOptions, cancellationToken)
                     ?? throw new InvalidOperationException("Vertex javobi bo'sh.");

        var candidate = result.Candidates?.FirstOrDefault();

        // Barcha matn part'larni birlashtiramiz (Gemini javobni bir nechta part'ga bo'lishi mumkin).
        var text = candidate?.Content?.Parts is { Count: > 0 } parts
            ? string.Concat(parts.Where(p => !string.IsNullOrEmpty(p.Text)).Select(p => p.Text))
            : string.Empty;

        return new AiCompletion
        {
            Content = text,
            InputTokens = result.UsageMetadata?.PromptTokenCount ?? 0,
            OutputTokens = result.UsageMetadata?.CandidatesTokenCount ?? 0,
            Sources = new List<string>()
        };
    }

    // --- Vertex AI Gemini DTO'lari ---

    private record VxRequest
    {
        [JsonPropertyName("system_instruction")]
        public VxContent? SystemInstruction { get; init; }

        public List<VxContent> Contents { get; init; } = new();

        [JsonPropertyName("generationConfig")]
        public VxGenConfig? GenerationConfig { get; init; }
    }

    private record VxContent
    {
        public string? Role { get; init; }
        public List<VxPart> Parts { get; init; } = new();
    }

    private record VxPart
    {
        public string? Text { get; init; }
    }

    private record VxGenConfig
    {
        [JsonPropertyName("maxOutputTokens")]
        public int MaxOutputTokens { get; init; }

        public double Temperature { get; init; }
    }

    private record VxResponse
    {
        public List<VxCandidate>? Candidates { get; init; }

        [JsonPropertyName("usageMetadata")]
        public VxUsage? UsageMetadata { get; init; }
    }

    private record VxCandidate
    {
        public VxContent? Content { get; init; }
    }

    private record VxUsage
    {
        [JsonPropertyName("promptTokenCount")]
        public int PromptTokenCount { get; init; }

        [JsonPropertyName("candidatesTokenCount")]
        public int CandidatesTokenCount { get; init; }
    }
}
