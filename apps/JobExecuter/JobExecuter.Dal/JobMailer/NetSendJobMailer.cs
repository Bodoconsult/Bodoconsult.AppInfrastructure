// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;
using JobExecuter.Dal.Interfaces;

namespace JobExecuter.Dal.JobMailer;

/// <summary>
/// Send a message via msg.exe (formerly net send)
/// </summary>
public class NetSendJobMailer : IJobMailer
{
    /// <summary>
    /// Send a mail 
    /// </summary>
    /// <param name="mailTo">receiver of the mail</param>
    /// <param name="subject">subject of the mail</param>
    /// <param name="body">mail body</param>
    /// <param name="priority">true=high priority</param>
    /// <returns>true on success</returns>
    public bool SendMail(string mailTo, string subject, IStructuredText body, bool priority)
    {

        //try
        //{

        var f = new PlainTextFormatter
        {
            StructuredText = body
        };
        var result = f.GetFormattedText();


        var fileName = Path.Combine(Environment.SystemDirectory, "msg.exe");

        //fileName = "‪C:\\Windows\\System32\\msg.exe";

        var cmd = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = "rleisner Test",
                //WorkingDirectory = Environment.SystemDirectory,
                WindowStyle = ProcessWindowStyle.Normal,
                CreateNoWindow = false,
            }
        };
        cmd.Start();
        cmd.WaitForExit();

        return true;
        //}
        //catch //(Exception ex)
        //{
        //    return false;
        //}

    }
}