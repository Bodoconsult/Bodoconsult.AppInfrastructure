// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebMailer.Business.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Models;

namespace BodoWebMailer.Business.Services;

/// <summary>
/// Handles the mails received from a database source
/// </summary>
public sealed class MailHandler : IMailHandler
{
    private readonly IAppLoggerProxy _logger;
    private readonly IMailer _mailer;
    private readonly IMailStorageService _service;
    private readonly List<string> _tempFiles = new();

    /// <summary>
    /// Delegate for handling status messages for console
    /// </summary>
    public StatusMessageDelegate StatusChanged { get; }

    /// <summary>
    /// Admin mail address
    /// </summary>
    public string AdminMailAddress { get; set; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="service">Mail provider service</param>
    /// <param name="mailer">Mailer</param>
    /// <param name="logger">Current app logger</param>
    /// <param name="globals">Current app globals</param>
    public MailHandler(IMailStorageService service, IMailer mailer, IAppLoggerProxy logger, IAppGlobals globals)
    {
        _service = service;
        _mailer = mailer;
        _logger = logger;
        StatusChanged = globals.StatusMessageDelegate;

        if (globals is not IBodoWebMailerGlobals bodoWebMailerGlobals)
        {
            throw new ArgumentException("appGlobals is not IBodoWebMailerGlobals");
        }

        AdminMailAddress = bodoWebMailerGlobals.AdminMailAddress;
    }

    /// <summary>
    /// Start mailing
    /// </summary>
    public void StartMailing()
    {
        Status("Open database...");

        var json = _service.GetMailAccontData();
        _mailer.LoadMailAccount(json);
        Status("Mail config loaded...");


        Status("Get mails...");

        var mailItems = _service.GetMails();

        if (!mailItems.Any())
        {
            return;
        }

        Status($"Got {mailItems.Count} mails...");
        _logger.LogInformation("Got mails");

        // Init the mailer
        _mailer.Init();

        // Login to mail server
        _mailer.Logon();

        Status($"Logged to mail server...");
        _logger.LogInformation("Logged to mail server");


        Status($"Start mail processing...");

        // Mails verarbeiten
        foreach (var mailItem in mailItems)
        {
            var item = mailItem;

            var msg = $"Mail to {item.To}: {item.Subject}";
            _logger.LogInformation(msg);
            Status(msg);


            PrepareQueries(item);

            _mailer.CurrentMailAccount.MailAddressSender = item.From;
            try
            {
                var erg = _mailer.SendMail(item);

                if (!erg)
                {
                    _service.ArchiveMail(item);
                }

            }
            catch (Exception ex)
            {
                msg = $"BodoWebMailer:Error:{item.MailGuid}:{msg}";

                Status(msg);
                _logger.LogError(msg, ex);

                if (string.IsNullOrEmpty(AdminMailAddress))
                {
                    return;
                }

                try
                {
                    var adminMail = new MailItem
                    {
                        MailGuid = Guid.NewGuid().ToString(),
                        To = AdminMailAddress,
                        Subject = "BodoWebMailer:Error",
                        Body = msg
                    };

                    _mailer.SendMail(adminMail);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }

                _service.SetError(item);
            }
        }

        // Delete temp files
        foreach (var path in _tempFiles)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
                // ignored
            }
        }

        Status("All mails sent...");
    }

    public void PrepareQueries(MailItem item)
    {

        if (string.IsNullOrEmpty(item.Queries))
        {
            return;
        }

        var queries = (Dictionary<string, string>)JsonConvert.DeserializeObject(item.Queries, typeof(Dictionary<string, string>));

        if (queries == null)
        {
            return;
        }

        // Save Query as file
        if (queries.ContainsKey("ToCsvFile"))
        {

            foreach (var query in queries.Where(x => x.Key != "ToCsvFile"))
            {
                var path = _service.RunSqlQueryToCsvFile(query.Key, query.Value);

                item.Attachments += $"{path};";
                _tempFiles.Add(path);
            }

            return;
        }

        var content = "";

        foreach (var query in queries)
        {
            content += _service.RunSqlQueryAsHtml(query.Key, query.Value);
        }


        item.Body = string.IsNullOrEmpty(item.Body) ?
            content :
            item.Body += content;
    }

    public void Status(string message)
    {
        var x = StatusChanged;
        x?.Invoke(message);
    }
}