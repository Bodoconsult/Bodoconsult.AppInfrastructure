// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;
using JobExecuter.Dal.Interfaces;

namespace JobExecuter.Dal.JobMailer;

/// <summary>
/// Sends mail to console window
/// </summary>
public class ConsoleJobMailer : IJobMailer
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
            Console.WriteLine("{0}", s);
            Debug.Print(s);
        }
        else
        {
            Console.WriteLine("\r\n***************** Begin mail ******************");
            Console.WriteLine("Mail to: {0}\r\nSubject: {1}\r\nBody:{2}\r\n", mailTo, subject, result);
            Console.WriteLine("***************** End mail ******************\r\n");
        }

        return true;
    }
}