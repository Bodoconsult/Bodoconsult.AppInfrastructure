// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Test.Helpers;
using Bodoconsult.App.Zip;

namespace Bodoconsult.App.Test.Zip;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
public class ZipHandlerTests
{
    [Test]
    public void GenerateZip_OneFile_ArchiveCreated()
    {
        var toZip = Path.Combine(TestHelper.TestDataPath, @"logo.jpg");
        var zipFileName = Path.Combine(TestHelper.TempPath, @"zipFile.zip");

        if (File.Exists(zipFileName))
        {
            File.Delete(zipFileName);
        }

        var zh = new ZipHandler([toZip]);

        zh.GenerateZip(zipFileName);

        Assert.That(File.Exists(zipFileName));
    }

    [Test]
    public void GenerateZip_TwoFiles_ArchiveCreated()
    {
        var toZip1 = Path.Combine(TestHelper.TestDataPath, @"logo.jpg");
        var toZip2 = Path.Combine(TestHelper.TestDataPath, @"logo1.jpg");
        var zipFileName = Path.Combine(TestHelper.TempPath, @"zipFile2.zip");

        if (File.Exists(zipFileName)) File.Delete(zipFileName);

        var zh = new ZipHandler([toZip1, toZip2]);

        zh.GenerateZip(zipFileName);

        Assert.That(File.Exists(zipFileName));
    }


    [Test]
    public void GenerateZip_OneFile_ReturnsStream()
    {
        var toZip = Path.Combine(TestHelper.TestDataPath, @"logo.jpg");

        var stream = new MemoryStream();

        var zh = new ZipHandler([toZip]);

        zh.GenerateZip(stream);

        Assert.That(stream, Is.Not.EqualTo(null));
        Assert.That(stream.Position == 0);
        Assert.That(stream.Length > 0);
    }
}