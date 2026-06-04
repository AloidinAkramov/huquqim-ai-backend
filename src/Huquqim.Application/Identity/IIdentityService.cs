using Huquqim.Application.Identity.Contracts;
using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Identity;

public interface IIdentityService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<Result<UserResponse>> GetMeAsync(long userId, CancellationToken cancellationToken = default);

    /// <summary>Tarif sotib olish (DEMO: darrov premium qiladi).</summary>
    Task<Result<UserResponse>> UpgradeAsync(long userId, UpgradeRequest request, CancellationToken cancellationToken = default);
}
