using FluentValidation;
using Huquqim.Application.Cases;
using Huquqim.Application.Commons.Validation;
using Huquqim.Application.Conversations;
using Huquqim.Application.Documents;
using Huquqim.Application.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Huquqim.Application;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        // Validatorlar
        services.AddValidatorsFromAssemblyContaining<IIdentityService>(includeInternalTypes: true);
        services.AddScoped<IValidationService, ValidationService>();

        // Feature servislari
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<ITriageService, TriageService>();
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<IDocumentService, DocumentService>();

        return services;
    }
}
