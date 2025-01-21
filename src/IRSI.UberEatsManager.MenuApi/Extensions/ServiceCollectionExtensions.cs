using IRSI.UberEatsManager.MenuApi.Middleware;
using Microsoft.AspNetCore.Authentication;

namespace IRSI.UberEatsManager.MenuApi.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("x-api-key")
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>("x-api-key", null);
        services.AddAuthorization();
        return services;
    }
}