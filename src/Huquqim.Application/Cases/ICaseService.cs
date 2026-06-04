using Huquqim.Application.Cases.Contracts;
using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Cases;

public interface ICaseService
{
    Task<Result<PagedList<CaseResponse>>> GetAllAsync(long userId, CaseFilter filter, CancellationToken cancellationToken = default);

    Task<Result<CaseResponse>> GetByIdAsync(long userId, long caseId, CancellationToken cancellationToken = default);

    Task<Result<CaseResponse>> CreateAsync(long userId, CreateCaseRequest request, CancellationToken cancellationToken = default);

    Task<Result<CaseResponse>> UpdateAsync(long userId, long caseId, UpdateCaseRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(long userId, long caseId, CancellationToken cancellationToken = default);
}
