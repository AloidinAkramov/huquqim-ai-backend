using Huquqim.Application.Commons.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Huquqim.Api.Controllers;

/// <summary>
/// Avtorizatsiya talab qiluvchi controllerlar uchun asos. JWT claim'dan UserId beradi.
/// </summary>
[ApiController]
[Authorize]
public abstract class AuthorizedController : ControllerBase
{
    private long? _userId;

    protected long UserId
    {
        get
        {
            if (_userId.HasValue)
                return _userId.Value;

            var raw = User.FindFirst(CustomClaims.UserId)?.Value
                      ?? throw new UnauthorizedAccessException("UserId claim topilmadi.");

            _userId = long.Parse(raw);
            return _userId.Value;
        }
    }
}
