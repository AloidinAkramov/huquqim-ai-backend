using Huquqim.Domain.Entities.Users;

namespace Huquqim.Application.Commons.Security;

/// <summary>
/// JWT access token generatsiya qiluvchi.
/// </summary>
public interface IJwtProvider
{
    /// <summary>Foydalanuvchi uchun token va uning amal qilish muddatini qaytaradi.</summary>
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
