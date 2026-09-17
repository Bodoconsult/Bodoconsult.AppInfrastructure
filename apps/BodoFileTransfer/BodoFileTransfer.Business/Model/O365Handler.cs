// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;

namespace BodoFileTransfer.Business.Model;

public class O365Handler
{

    // Even if this is a console application here, a daemon application is a confidential client application
    GraphServiceClient _app;

    public O365Handler(O365Account account)
    {
        Account = account;
    }

    /// <summary>
    /// Current O365 Graph API config
    /// </summary>
    public O365Account Account { get; }

    /// <summary>
    /// Login to O365 Graph API
    /// </summary>
    /// <returns>Awaitable task</returns>
    public void Login()
    {

        try
        {
            //var scopes = new[] { "https://graph.microsoft.com/.default" };
            var tenantId = Account.Tenant;

            var authenticationProvider = new BaseBearerTokenAuthenticationProvider(new TokenProvider(Account.ClientId, Account.ClientSecret, tenantId));

            _app = new GraphServiceClient(authenticationProvider);

            //// Configure the MSAL client as a confidential client
            //var confidentialClient = ConfidentialClientApplicationBuilder
            //    .Create(MailAccount.ClientId)
            //    .WithAuthority($"https://login.microsoftonline.com/{tenantId}/v2.0")
            //    .WithClientSecret(MailAccount.ClientSecret)
            //    .Build();

            // Build the Microsoft Graph client. As the authentication provider, set an async lambda
            // which uses the MSAL client to obtain an app-only access token to Microsoft Graph,
            // and inserts this access token in the Authorization header of each API request. 

            //_app = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
            //{

            //    // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
            //    var authResult = await confidentialClient
            //        .AcquireTokenForClient(scopes)
            //        .ExecuteAsync();

            //    // Add the access token in the Authorization header of the API request.
            //    requestMessage.Headers.Authorization =
            //        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            //}));

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public void GetAllEmailAttachments()
    {

        // ToDO: reactivate

        try
        {

            //hasAttachments eq true and received
            var filter = $"receivedDateTime ge {Account.SentDateFilter:yyyy-MM-dd}";

            var mails = GetMessages(filter).GetAwaiter().GetResult();

      //.Select(m => new
      //          {
      //              // Only request specific properties
      //              m.From,
      //              m.ReceivedDateTime,
      //              m.Subject,
      //              m.HasAttachments,
      //              m.Attachments
      //          })
                //// Search
                //.Filter(filter)
                //.Expand("attachments")
                //.Top(100)
                //// Sort by received time, newest first
                //.OrderBy("ReceivedDateTime DESC")
                //.GetAsync().GetAwaiter().GetResult();

            foreach (var mail in mails)
            {
                if (mail.HasAttachments != true)
                {
                    continue;
                }

                // Subject filtering mail subject
                if (!Account.CheckSubject(mail.Subject))
                {
                    Debug.Print(mail.Subject);
                    continue;
                }

                if (mail.Attachments == null)
                {
                    continue;
                }

                foreach (var attachment in mail.Attachments)
                {
                    if (attachment is not FileAttachment attach)
                    {
                        continue;
                    }

                    // Subject filtering attachment name
                    if (!Account.CheckSubject(attach.Name))
                    {
                        continue;
                    }

                    Debug.Print(attach.Name);

                    var ext = new FileInfo(attach.Name!).Extension.ToLower();

                    if (!string.IsNullOrEmpty(Account.FileExtensionFilter) &&
                        !Account.FileExtensionFilter.Contains(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    //Status($"  -> {attach.Name}");

                    var fi = new FileInfo(attach.Name);

                    var fileName = $"{fi.Name.Replace(fi.Extension, "")}_{mail.ReceivedDateTime:yyyyMMddhhmmss}{fi.Extension}";

                    var content = attach.ContentBytes;
                    Account.FileHandling(fileName, content);

                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<List<Message>> GetMessages(string filter)
    {
        var result = new List<Message>();

        var messages = await _app.Users[Account.UserName]

            // Only messages from Inbox folder
            .MailFolders["Inbox"]
            .Messages
            .GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = filter;
                requestConfiguration.QueryParameters.Expand = ["attachments"];
                requestConfiguration.QueryParameters.Top = 100;
                requestConfiguration.QueryParameters.Orderby = ["ReceivedDateTime DESC"];
                requestConfiguration.QueryParameters.Select = ["From", "ReceivedDateTime", "Subject", "HasAttachments", "Attachments"];
            }); ;

        while (messages?.Value != null)
        {
            result.AddRange(messages.Value);

            // If OdataNextLink has a value, there is another page
            if (!string.IsNullOrEmpty(messages.OdataNextLink))
            {
                // Pass the OdataNextLink to the WithUrl method
                // to request the next page
                messages = await _app.Me.Messages
                    .WithUrl(messages.OdataNextLink)
                    .GetAsync();
            }
            else
            {
                // No more results, exit loop
                break;
            }
        }

        return result;
    }
    
    /// <summary>
    /// Current token provider
    /// </summary>
    internal class TokenProvider : IAccessTokenProvider
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

            //var app = ConfidentialClientApplicationBuilder.Create(_clientId)
            //    .WithClientSecret(_clientSecret)
            //    .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}"))
            //    .Build();

            string[] scopes = ["https://graph.microsoft.com/.default"];

            var result = app.AcquireTokenForClient(scopes).ExecuteAsync(cancellationToken).Result;

            return Task.FromResult(result.AccessToken);
        }

        /// <summary>
        /// Returns the <see cref="P:Microsoft.Kiota.Abstractions.Authentication.IAccessTokenProvider.AllowedHostsValidator" /> for the provider.
        /// </summary>
        public AllowedHostsValidator AllowedHostsValidator { get; } = new();
    }
}