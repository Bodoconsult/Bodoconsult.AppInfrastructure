// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;
using JobExecuter.Dal.Interfaces;

namespace JobExecuter.Dal.JobMailer;

/// <summary>
/// Sends mail to Windows event log. Log entries will be placed under Windows protocols in the log file Application.
/// The source is "Bodoconsult.JobExecuter".
/// </summary>
public class EventlogJobMailer : IJobMailer
{
    private const string Source = "Bodoconsult.JobExecuter";
    private const string Log = "Application";

    /// <summary>
    /// default ctor
    /// </summary>
    public EventlogJobMailer()
    {

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{GetType().Name}: Works only on Windows OS.");
        }

        try
        {
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, Log);
            }
        }
        catch
        {
            // ignored
        }
    }


    /// <summary>
    /// Send a mail 
    /// </summary>
    /// <param name="mailTo">receiver of the mail</param>
    /// <param name="subject">subject of the mail</param>
    /// <param name="body">mail body</param>
    /// <param name="priority">true=high priority</param>
    public bool SendMail(string mailTo, string subject, IStructuredText body, bool priority)
    {

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{GetType().Name}: Works only on Windows OS.");
        }


        try
        {
            var f = new PlainTextFormatter
            {
                StructuredText = body
            };
            var result = f.GetFormattedText();

            var mEventLog = new EventLog(string.Empty) { Source = Source };
            mEventLog.WriteEntry($"Subject: {subject}\r\n{result}", EventLogEntryType.Error);
            return true;
        }
        catch
        {
            return false;
        }
    }
}