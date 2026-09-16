// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Bodoconsult.Web.Mail.Mailers;

/// <summary>
/// Current token provider
/// </summary>
public class TokenProvider : IAccessTokenProvider
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _tenantId;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <param name="clientSecret">Client secret</param>
    /// <param name="tenantId">Tenenat ID</param>
    public TokenProvider(string clientId, string clientSecret, string tenantId)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _tenantId = tenantId;
    }

    /// <summary>
    ///     This method is called by the <see cref="T:Microsoft.Kiota.Abstractions.Authentication.BaseBearerTokenAuthenticationProvider" /> class to get the access token.
    /// </summary>
    /// <param name="uri">The target URI to get an access token for.</param>
    /// <param name="additionalAuthenticationContext">Additional authentication context to pass to the authentication library.</param>
    /// <param name="cancellationToken">The cancellation token for the task</param>
    /// <returns>A Task that holds the access token to use for the request.</returns>
    public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object> additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        // Configure the MSAL client as a confidential client
        var app = ConfidentialClientApplicationBuilder
            .Create(_clientId)
            .WithAuthority($"https://login.microsoftonline.com/{_tenantId}/v2.0")
            .WithClientSecret(_clientSecret)
            .Build();

        string[] scopes = ["https://graph.microsoft.com/.default"];

        var result = app.AcquireTokenForClient(scopes).ExecuteAsync(cancellationToken).Result;

        return Task.FromResult(result.AccessToken);
    }

    /// <summary>
    /// Returns the <see cref="P:Microsoft.Kiota.Abstractions.Authentication.IAccessTokenProvider.AllowedHostsValidator" /> for the provider.
    /// </summary>
    public AllowedHostsValidator AllowedHostsValidator { get; } = new();
}