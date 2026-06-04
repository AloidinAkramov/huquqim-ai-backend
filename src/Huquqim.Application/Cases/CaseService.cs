using Huquqim.Application.Cases.Contracts;
using Huquqim.Application.Commons.Persistence;
using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Entities.Cases;
using Huquqim.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Huquqim.Application.Cases;

public class CaseService(IAppDbContext dbContext) : ICaseService
{
    public async Task<Result<PagedList<CaseResponse>>> GetAllAsync(
        long userId,
        CaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Cases
            .Where(c => c.UserId == userId && !c.IsDeleted)
            .AsNoTracking();

        if (filter.Type.HasValue)
            query = query.Where(c => c.Type == filter.Type.Value);

        if (filter.Status.HasValue)
            query = query.Where(c => c.Status == filter.Status.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(c => c.Title.Contains(filter.Search));

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(c => new CaseResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Type = c.Type,
                Status = c.Status,
                ResponsibleAuthority = c.ResponsibleAuthority,
                Deadline = c.Deadline,
                CreatedAt = c.CreatedAt,
                DocumentCount = c.Documents.Count(d => !d.IsDeleted)
            })
            .ToListAsync(cancellationToken);

        return new PagedList<CaseResponse>(items, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<Result<CaseResponse>> GetByIdAsync(
        long userId,
        long caseId,
        CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Cases
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (entity is null)
            return CaseErrors.NotFound;

        if (entity.UserId != userId)
            return CaseErrors.Forbidden;

        return MapToResponse(entity);
    }

    public async Task<Result<CaseResponse>> CreateAsync(
        long userId,
        CreateCaseRequest request,
        CancellationToken cancellationToken = default)
    {
        var title = string.IsNullOrWhiteSpace(request.Title)
            ? BuildTitleFromDescription(request.Description)
            : request.Title.Trim();

        var entity = new Case
        {
            UserId = userId,
            Title = title,
            Description = request.Description.Trim(),
            Type = ECaseType.Unknown,
            Status = ECaseStatus.Triage,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        dbContext.Cases.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(entity);
    }

    public async Task<Result<CaseResponse>> UpdateAsync(
        long userId,
        long caseId,
        UpdateCaseRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Cases
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (entity is null)
            return CaseErrors.NotFound;

        if (entity.UserId != userId)
            return CaseErrors.Forbidden;

        if (!string.IsNullOrWhiteSpace(request.Title))
            entity.Title = request.Title.Trim();

        if (request.Type.HasValue)
            entity.Type = request.Type.Value;

        if (request.Status.HasValue)
            entity.Status = request.Status.Value;

        if (request.ResponsibleAuthority is not null)
            entity.ResponsibleAuthority = request.ResponsibleAuthority;

        if (request.Deadline.HasValue)
            entity.Deadline = request.Deadline.Value;

        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = userId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(entity);
    }

    public async Task<Result> DeleteAsync(
        long userId,
        long caseId,
        CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Cases
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (entity is null)
            return CaseErrors.NotFound;

        if (entity.UserId != userId)
            return CaseErrors.Forbidden;

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = userId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static string BuildTitleFromDescription(string description)
    {
        var trimmed = description.Trim();
        return trimmed.Length <= 60 ? trimmed : trimmed[..60].TrimEnd() + "...";
    }

    private static CaseResponse MapToResponse(Case c) => new()
    {
        Id = c.Id,
        Title = c.Title,
        Description = c.Description,
        Type = c.Type,
        Status = c.Status,
        ResponsibleAuthority = c.ResponsibleAuthority,
        Deadline = c.Deadline,
        CreatedAt = c.CreatedAt,
        DocumentCount = 0
    };
}
