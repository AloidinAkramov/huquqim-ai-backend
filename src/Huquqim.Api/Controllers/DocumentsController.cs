using Huquqim.Api.Extensions;
using Huquqim.Application.Documents;
using Huquqim.Application.Documents.Contracts;
using Huquqim.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Huquqim.Api.Controllers;

[ApiController]
[Route("api/documents")]
public class DocumentsController(IDocumentService documentService) : AuthorizedController
{
    /// <summary>Mavjud hujjat shablonlari (ish turi bo'yicha filtrlanishi mumkin).</summary>
    [HttpGet("templates")]
    public async Task<IResult> GetTemplates([FromQuery] ECaseType? caseType, CancellationToken ct)
    {
        var result = await documentService.GetTemplatesAsync(caseType, ct);
        return result.ToOk();
    }

    /// <summary>Bitta hujjatni olish (matn bilan).</summary>
    [HttpGet("{documentId:long}")]
    public async Task<IResult> GetById(long documentId, CancellationToken ct)
    {
        var result = await documentService.GetByIdAsync(UserId, documentId, ct);
        return result.ToOk();
    }

    /// <summary>Hujjatni .docx (Word) fayl sifatida yuklab olish.</summary>
    [HttpGet("{documentId:long}/download")]
    public async Task<IResult> Download(long documentId, CancellationToken ct)
    {
        var result = await documentService.DownloadDocxAsync(UserId, documentId, ct);
        if (result.IsFailure)
            return result.ToProblem();

        return Results.File(
            result.Data.Content,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            result.Data.FileName);
    }

    /// <summary>Shablonni to'ldirib to'g'ridan .docx yuklab olish (ish ochmasdan). Premium kerak.</summary>
    [HttpPost("fill")]
    public async Task<IResult> Fill([FromBody] FillTemplateRequest request, CancellationToken ct)
    {
        var result = await documentService.FillTemplateDocxAsync(UserId, request, ct);
        if (result.IsFailure)
            return result.ToProblem();

        return Results.File(
            result.Data.Content,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            result.Data.FileName);
    }
}
