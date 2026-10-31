// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.App.Helpers;
using Bodoconsult.Text.Model;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Test.App;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace JobExecuter.Test.JobMailerTests;

[TestFixture]
public class FileJobMailerTests
{
    [Test]       
    public void SendMail_ValidSetup_LogFileWritten()
    {
        var logFile = Path.Combine(TestDataHelper.LocalPath, "logfile.log");
        FileSystemHelper.DeleteFile(logFile);

        var sr = new StructuredText();
        sr.AddHeader1("Eine Testmail an mich!");

        var s = new FileJobMailer(Globals.Instance)
        {
            LogFileName = logFile
        };

        var result = s.SendMail("robert.leisner@bodoconsult.de", "Testmail", sr, false);

        Assert.That(result);
        Assert.That(File.Exists(logFile));
    }
}