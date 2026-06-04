using Huquqim.Application.Commons.Security;
using Microsoft.AspNetCore.Http;

namespace Huquqim.Infrastructure.Security;

/// <summary>
/// HttpContext'dagi JWT claim'laridan joriy foydalanuvchini o'qiydi.
/// </summary>
public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public long UserId
    {
        get
        {
            var raw = httpContextAccessor.HttpContext?.User?.FindFirst(CustomClaims.UserId)?.Value;
            return long.TryParse(raw, out var id)
                ? id
                : throw new UnauthorizedAccessException("Foydalanuvchi identifikatori topilmadi.");
        }
    }
}
