namespace Huquqim.Application.Commons.Security;

/// <summary>
/// Parolni xeshlash va tekshirish.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string passwordHash);
}
