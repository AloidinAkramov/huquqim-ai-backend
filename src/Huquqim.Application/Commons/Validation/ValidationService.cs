using FluentValidation;
using Huquqim.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Huquqim.Application.Commons.Validation;

public class ValidationService(IServiceProvider provider) : IValidationService
{
    public async Task<Result> ValidateAsync<T>(T instance)
    {
        var validator = provider.GetService<IValidator<T>>();
        if (validator is null)
            return Result.Success();

        var result = await validator.ValidateAsync(instance);
        if (result.IsValid)
            return Result.Success();

        var messages = result.Errors
            .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
            .ToList();

        return Error.Validation("Validation.Failed", string.Join("; ", messages));
    }
}
