// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;
using JobExecuter.Dal.App;
using JobExecuter.Dal.Interfaces;
using log4net;
using System.IO;

namespace JobExecuter.Dal.JobMailer;

/// <inheritdoc />
/// <summary>
/// Sends mail to log file
/// </summary>
public class FileJobMailer : IJobMailer
{
    private string _logFileName;
    private IAppLoggerProxy _logger;

    private IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    public FileJobMailer(IAppGlobals appGlobals)
    {
        _appGlobals = appGlobals;
    }
    
    /// <summary>
    /// Full path of the log file
    /// </summary>
    public string LogFileName
    {
        get => _logFileName;
        set
        {
            _logFileName = value;

            var factory = new MonitorLoggerFactory(_logFileName);
            factory.LoggingConfig = _appGlobals.LoggingConfig;

            _logger = new AppLoggerProxy(factory, _appGlobals.LogDataFactory);
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Send a mail 
    /// </summary>
    /// <param name="mailTo">receiver of the mail</param>
    /// <param name="subject">subject of the mail</param>
    /// <param name="body">mail body</param>
    /// <param name="priority">true=high priority</param>
    public bool SendMail(string mailTo, string subject, IStructuredText body, bool priority)
    {
        var f = new PlainTextFormatter
        {
            StructuredText = body
        };
        var result = f.GetFormattedText();

        if (string.IsNullOrEmpty(mailTo))
        {
            if (result.StartsWith("\r\n", StringComparison.OrdinalIgnoreCase))
            {
                result = result[2..];
            }

            if (result.EndsWith("\r\n\r\n", StringComparison.OrdinalIgnoreCase))
            {
                result = result[..^2];
            }

            _logger.LogInformation($"{result}");
        }
        else
        {
            _logger.LogInformation("\r\n***************** Begin mail ******************");
            _logger.LogInformation($"Mail to: {mailTo}\r\nSubject: {subject}\r\nBody:\r\n{result}\r\n");
            _logger.LogInformation("***************** End mail ******************\r\n");
        }

        return true;
    }
}