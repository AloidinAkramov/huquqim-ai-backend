using Huquqim.Api.Extensions;
using Huquqim.Application.Cases;
using Huquqim.Application.Cases.Contracts;
using Huquqim.Application.Conversations;
using Huquqim.Application.Conversations.Contracts;
using Huquqim.Application.Documents;
using Huquqim.Application.Documents.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Huquqim.Api.Controllers;

[ApiController]
[Route("api/cases")]
public class CasesController(
    ICaseService caseService,
    IConversationService conversationService,
    IDocumentService documentService,
    ITriageService triageService) : AuthorizedController
{
    /// <summary>Foydalanuvchining ishlari (filtr + sahifalash).</summary>
    [HttpGet]
    public async Task<IResult> GetAll([FromQuery] CaseFilter filter, CancellationToken ct)
    {
        var result = await caseService.GetAllAsync(UserId, filter, ct);
        return result.ToOk();
    }

    /// <summary>Ishni huquqiy toifalarga ajratish (triage) — foizlar bilan.</summary>
    [HttpPost("{caseId:long}/triage")]
    public async Task<IResult> Triage(long caseId, CancellationToken ct)
    {
        var result = await triageService.AnalyzeAsync(UserId, caseId, ct);
        return result.ToOk();
    }

    /// <summary>Saqlangan triage natijasini olish.</summary>
    [HttpGet("{caseId:long}/triage")]
    public async Task<IResult> GetTriage(long caseId, CancellationToken ct)
    {
        var result = await triageService.GetAsync(UserId, caseId, ct);
        return result.ToOk();
    }

    /// <summary>Bitta ish.</summary>
    [HttpGet("{caseId:long}")]
    public async Task<IResult> GetById(long caseId, CancellationToken ct)
    {
        var result = await caseService.GetByIdAsync(UserId, caseId, ct);
        return result.ToOk();
    }

    /// <summary>Yangi ish ochish (muammo tavsifi bilan).</summary>
    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateCaseRequest request, CancellationToken ct)
    {
        var result = await caseService.CreateAsync(UserId, request, ct);
        return result.ToOk();
    }

    /// <summary>Ishni yangilash.</summary>
    [HttpPut("{caseId:long}")]
    public async Task<IResult> Update(long caseId, [FromBody] UpdateCaseRequest request, CancellationToken ct)
    {
        var result = await caseService.UpdateAsync(UserId, caseId, request, ct);
        return result.ToOk();
    }

    /// <summary>Ishni o'chirish.</summary>
    [HttpDelete("{caseId:long}")]
    public async Task<IResult> Delete(long caseId, CancellationToken ct)
    {
        var result = await caseService.DeleteAsync(UserId, caseId, ct);
        return result.ToOk();
    }

    // --- Suhbat (chat + RAG) ---

    /// <summary>Ish bo'yicha AI ga xabar yuborish.</summary>
    [HttpPost("{caseId:long}/messages")]
    public async Task<IResult> SendMessage(long caseId, [FromBody] SendMessageRequest request, CancellationToken ct)
    {
        var result = await conversationService.SendMessageAsync(UserId, caseId, request, ct);
        return result.ToOk();
    }

    /// <summary>Ish suhbatini (xabarlari bilan) olish.</summary>
    [HttpGet("{caseId:long}/conversation")]
    public async Task<IResult> GetConversation(long caseId, CancellationToken ct)
    {
        var result = await conversationService.GetByCaseAsync(UserId, caseId, ct);
        return result.ToOk();
    }

    // --- Hujjatlar ---

    /// <summary>Ish hujjatlari ro'yxati.</summary>
    [HttpGet("{caseId:long}/documents")]
    public async Task<IResult> GetDocuments(long caseId, CancellationToken ct)
    {
        var result = await documentService.GetByCaseAsync(UserId, caseId, ct);
        return result.ToOk();
    }

    /// <summary>Shablon asosida hujjat generatsiya qilish.</summary>
    [HttpPost("{caseId:long}/documents")]
    public async Task<IResult> GenerateDocument(long caseId, [FromBody] GenerateDocumentRequest request, CancellationToken ct)
    {
        var result = await documentService.GenerateAsync(UserId, caseId, request, ct);
        return result.ToOk();
    }
}
