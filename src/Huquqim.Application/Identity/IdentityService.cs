using Huquqim.Application.Commons.Persistence;
using Huquqim.Application.Commons.Security;
using Huquqim.Application.Commons.Validation;
using Huquqim.Application.Identity.Contracts;
using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Entities.Users;
using Huquqim.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Huquqim.Application.Identity;

public class IdentityService(
    IAppDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    IValidationService validationService) : IIdentityService
{
    public async Task<Result<AuthResponse>> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = await validationService.ValidateAsync(request);
        if (validation.IsFailure)
            return validation.Error;

        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await dbContext.Users
            .AnyAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
        if (exists)
            return IdentityErrors.EmailAlreadyExists;

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = passwordHasher.Hash(request.Password),
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return BuildAuthResponse(user);
    }

    public async Task<Result<AuthResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = await validationService.ValidateAsync(request);
        if (validation.IsFailure)
            return validation.Error;

        var email = request.Email.Trim().ToLowerInvariant();

        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return IdentityErrors.InvalidCredentials;

        user.LastLoginAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return BuildAuthResponse(user);
    }

    public async Task<Result<UserResponse>> GetMeAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken);

        if (user is null)
            return IdentityErrors.UserNotFound;

        return MapToUserResponse(user);
    }

    public async Task<Result<UserResponse>> UpgradeAsync(
        long userId,
        UpgradeRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken);

        if (user is null)
            return IdentityErrors.UserNotFound;

        // DEMO: to'lov tekshirilmaydi, darrov premium qilinadi.
        // TODO: Click/Payme integratsiyasi qo'shilganda bu yerda to'lov tasdiqlanadi.
        var tier = request.Tier == ESubscriptionTier.Monthly
            ? ESubscriptionTier.Monthly
            : ESubscriptionTier.OneTime;

        user.SubscriptionTier = tier;
        user.SubscriptionExpiresAt = tier == ESubscriptionTier.Monthly
            ? DateTime.UtcNow.AddMonths(1)
            : null;
        user.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToUserResponse(user);
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        var (token, expiresAt) = jwtProvider.GenerateToken(user);
        return new AuthResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = MapToUserResponse(user)
        };
    }

    private static UserResponse MapToUserResponse(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        SubscriptionTier = user.SubscriptionTier,
        SubscriptionExpiresAt = user.SubscriptionExpiresAt,
        IsPremium = SubscriptionLimits.IsPremium(
            user.SubscriptionTier, user.SubscriptionExpiresAt, DateTime.UtcNow)
    };
}
