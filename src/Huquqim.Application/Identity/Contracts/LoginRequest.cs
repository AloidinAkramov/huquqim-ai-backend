namespace Huquqim.Application.Identity.Contracts;

public record LoginRequest
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}
