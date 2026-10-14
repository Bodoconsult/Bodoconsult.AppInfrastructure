// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;
using JobExecuter.Dal.Interfaces;

namespace JobExecuter.Dal.JobMailer;

/// <summary>
/// Sends mail to debug window
/// </summary>
public class FakeJobMailer : IJobMailer
{
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
            var s = result;
            Debug.Print("{0}", s);
        }
        else
        {
            Debug.Print("\r\n***************** Begin mail ******************");
            Debug.Print("Mail to: {0}\r\nSubject: {1}\r\nBody:{2}\r\n", mailTo, subject, result);
            Debug.Print("***************** End mail ******************\r\n");
        }

        return true;
    }
}