using Huquqim.Application.Commons.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Huquqim.Infrastructure.Authentication;

/// <summary>appsettings'dagi "Jwt" bo'limini JwtOptions ga bog'laydi.</summary>
public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(JwtOptions.SectionName).Bind(options);
    }
}
