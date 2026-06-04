using Huquqim.Domain.Enums;

namespace Huquqim.Application.Commons.Ai;

/// <summary>
/// AI ga yuboriladigan bitta suhbat xabari.
/// </summary>
public record ChatMessage(EMessageRole Role, string Content);
