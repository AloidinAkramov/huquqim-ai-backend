using FluentValidation;

namespace Huquqim.Application.Identity.Contracts;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("F.I.O. kiritilishi shart.")
            .MaximumLength(150);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email kiritilishi shart.")
            .EmailAddress().WithMessage("Email formati noto'g'ri.")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Parol kiritilishi shart.")
            .MinimumLength(8).WithMessage("Parol kamida 8 ta belgidan iborat bo'lishi kerak.")
            .MaximumLength(50);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?998\d{9}$").WithMessage("Telefon raqami noto'g'ri (masalan: +998901234567).")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
