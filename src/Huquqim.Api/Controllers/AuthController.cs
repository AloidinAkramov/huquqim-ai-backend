using Huquqim.Api.Extensions;
using Huquqim.Application.Identity;
using Huquqim.Application.Identity.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Huquqim.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IIdentityService identityService) : AuthorizedController
{
    /// <summary>Ro'yxatdan o'tish (email/parol).</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await identityService.RegisterAsync(request, ct);
        return result.ToOk();
    }

    /// <summary>Tizimga kirish.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await identityService.LoginAsync(request, ct);
        return result.ToOk();
    }

    /// <summary>Joriy foydalanuvchi ma'lumotlari.</summary>
    [HttpGet("me")]
    public async Task<IResult> Me(CancellationToken ct)
    {
        var result = await identityService.GetMeAsync(UserId, ct);
        return result.ToOk();
    }

    /// <summary>Tarif sotib olish (DEMO: darrov premium qiladi).</summary>
    [HttpPost("upgrade")]
    public async Task<IResult> Upgrade([FromBody] UpgradeRequest request, CancellationToken ct)
    {
        var result = await identityService.UpgradeAsync(UserId, request, ct);
        return result.ToOk();
    }
}
