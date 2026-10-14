// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.Text.Model;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace JobExecuter.Test.JobMailerTests;

[TestFixture]
public class FileJobMailerTests
{
    [Test]       
    public void TestFileMailer_Basic()
    {
        var logFile = Path.Combine(TestDataHelper.LocalPath, "logfile.log");
        if (File.Exists(logFile))
        {
            File.Delete(logFile);
        }

        var sr = new StructuredText();
        sr.AddHeader1("Eine Testmail an mich!");

        var s = new FileJobMailer
        {
            LogFileName = logFile
        };

        var result = s.SendMail("robert.leisner@bodoconsult.de", "Testmail", sr, false);

        Assert.That(result);
        Assert.That(File.Exists(logFile));
    }
}