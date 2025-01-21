using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace IRSI.UberEatsManager.MenuApi.Middleware;

internal sealed class ApiKeyAuthenticationSchemeHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) :
    AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string ApiKeyHeaderName = "x-api-key";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var configuration = Context.RequestServices.GetRequiredService<IConfiguration>();
        var expectedApiKey = configuration.GetValue<string>("ApiKey") ?? string.Empty;
        if (apiKey != expectedApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
        }

        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, "Api-Key-User")], Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}