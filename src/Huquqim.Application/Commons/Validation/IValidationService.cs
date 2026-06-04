using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Commons.Validation;

/// <summary>
/// FluentValidation orqali so'rovlarni tekshiruvchi xizmat.
/// </summary>
public interface IValidationService
{
    Task<Result> ValidateAsync<T>(T instance);
}
