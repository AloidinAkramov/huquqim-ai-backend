namespace Huquqim.Application.Commons.Security;

/// <summary>
/// Joriy autentifikatsiyalangan foydalanuvchi (HttpContext'dan).
/// </summary>
public interface ICurrentUser
{
    long UserId { get; }

    bool IsAuthenticated { get; }
}
