// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.IO;
using Bodoconsult.App.Helpers;
using Bodoconsult.Text.Model;
using JobExecuter.Dal.Helpers;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.HelperTests;

[TestFixture]
public class FileHelperTests
{
    [Test]
    public void TestGetTextResource()
    {
        var result = FileHelper.GetTextResource("JobExecuter.Dal.Resources.TableInfo.sql");

        Debug.Print(result);

        Assert.That(!string.IsNullOrEmpty(result));
    }

    [Test]
    public void TestSaveAsPdf()
    {
        var fileName = $@"{TestDataHelper.LocalPath}\SaveAsPdf.pdf";

        FileSystemHelper.DeleteFile(fileName);

        var st = new StructuredText();

        st.AddHeader1("Überschrift 1");
        st.AddParagraph("Hallo Welt!");

        FileHelper.SaveAsPdf(fileName, st, "Hallo");

        Assert.That(File.Exists(fileName));

        FileSystemHelper.DeleteFile(fileName);
    }

    [Test]
    public void TestSaveBinaryResource()
    {
        var fileName = Path.Combine(TestDataHelper.LocalPath, "Backup.cab");

        FileSystemHelper.DeleteFile(fileName);

        Assert.That(!File.Exists(fileName));

        FileHelper.SaveBinaryResource("JobExecuter.Dal.Resources.Backup.cab", fileName);

        Assert.That(File.Exists(fileName));
    }
}