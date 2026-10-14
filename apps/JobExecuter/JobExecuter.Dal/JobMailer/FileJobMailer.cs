// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;
using JobExecuter.Dal.Interfaces;
using log4net;

namespace JobExecuter.Dal.JobMailer;

/// <inheritdoc />
/// <summary>
/// Sends mail to log file
/// </summary>
public class FileJobMailer : IJobMailer
{
    private string _logFileName;
    private static ILog _logger;

    /// <summary>
    /// Full path of the log file
    /// </summary>
    public string LogFileName
    {
        get => _logFileName;
        set
        {
            _logFileName = value;
            // ReSharper disable once RedundantNameQualifier
            log4net.GlobalContext.Properties["LogFileName"] = _logFileName; //log file path
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            _logger =  LogManager.GetLogger(typeof (FileJobMailer));
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
            _logger.Info($"{result}");
        }
        else
        {
            _logger.Info("\r\n***************** Begin mail ******************");
            _logger.Info($"Mail to: {mailTo}\r\nSubject: {subject}\r\nBody:\r\n{result}\r\n");
            _logger.Info("***************** End mail ******************\r\n");
        }

        return true;
    }
}