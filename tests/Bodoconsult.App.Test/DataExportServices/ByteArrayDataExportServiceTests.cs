// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataExportServices;
using System.Text;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Test.Helpers;

namespace Bodoconsult.App.Test.DataExportServices;

[TestFixture]
internal class ByteArrayDataExportServiceTests
{
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        TestHelper.CleanTempPath();
    }

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };

        // Assert
        Assert.That(service.CurrentFilePath, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Ctor_ValidNonDefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var service = new ByteArrayDataExportService
        {
            FileName = "Export",
            FileExtension = "bin"
        };

        // Assert
        Assert.That(service.CurrentFilePath, Is.EqualTo(string.Empty));
    }

    [Test]
    public void CreateCurrentFilePath_ValidDefaultSetup_ValidPathReturned()
    {
        // Arrange 
        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };

        // Act  
        var result = service.CreateCurrentFilePath();

        // Assert
        Assert.That(string.IsNullOrEmpty(result), Is.False);
    }

    [TestCase(10, TestName = "Add_ValidDefaultSetup10_FileWritten")]
    [TestCase(1000, TestName= "Add_ValidDefaultSetup1000_FileWritten")]
    [TestCase(1000000, TestName = "Add_ValidDefaultSetup1000000_FileWritten")]
    public void Add_ValidDefaultSetup_FileWritten(int count)
    {
        // Arrange 
        const string text = "Blubb\r\n";

        var data = Encoding.UTF8.GetBytes(text);

        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };
        service.Start();

        // Act
        for (var i = 0; i < count; i++)
        {
            service.Add(data);
        }

        service.Stop();

        // Assert
        var path = service.CurrentFilePath;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(path, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(path), Is.False);

            var fi = new FileInfo(path);
            Assert.That(fi, Is.Not.Null);
            Assert.That(fi.Exists);

            Wait.Until(() => fi.Length > 0);

            Assert.That(service.RowCounter2, Is.GreaterThanOrEqualTo(count));
            Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(count));

            Assert.That(fi.Length, Is.GreaterThanOrEqualTo(count * text.Length));
        }

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }

    [TestCase(10, TestName = "Add_ValidDefaultSetup10List_FileWritten")]
    [TestCase(1000, TestName = "Add_ValidDefaultSetup1000List_FileWritten")]
    [TestCase(1000000, TestName = "Add_ValidDefaultSetup1000000List_FileWritten")]
    public void Add_ValidDefaultSetupList_FileWritten(int count)
    {
        // Arrange 
        const string text = "Blubb\r\n";

        var data = Encoding.UTF8.GetBytes(text);

        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };
        service.Start();

        // Act
        var list = new List<byte[]>();
        for (var i = 0; i < count; i++)
        {
            list.Add(data);
        }
        service.AddRange(list);

        service.Stop();

        // Assert
        var path = service.CurrentFilePath;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(path, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(path), Is.False);

            var fi = new FileInfo(path);
            Assert.That(fi, Is.Not.Null);
            Assert.That(fi.Exists);

            Wait.Until(() => fi.Length > 0);

            Assert.That(service.RowCounter2, Is.GreaterThanOrEqualTo(count));
            Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(count));

            Assert.That(fi.Length, Is.GreaterThanOrEqualTo(count * text.Length));

            FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
        }
    }

    //[Test]
    //public void Add_ValidDefaultSetup1000000_FileWritten()
    //{
    //    // Arrange 
    //    const string text = "BlubbR\r\n";
    //    const int count = 1000000;

    //    var data = Encoding.UTF8.GetBytes(text);

    //    var service = new ByteArrayDataExportService
    //    {
    //        FileExtension = "bin"
    //    };
    //    service.Start();

    //    // Act
    //    for (var i = 0; i < count; i++)
    //    {
    //        service.Add(data);
    //    }

    //    service.Stop();

    //    // Assert
    //    var path = service.CurrentFilePath;

    //    Assert.That(path, Is.Not.Null);
    //    Assert.That(string.IsNullOrEmpty(path), Is.False);

    //    var fi = new FileInfo(path);
    //    Assert.That(fi, Is.Not.Null);

    //    Assert.That(fi.Exists);
    //    Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(count));

    //    Assert.That(fi.Length, Is.GreaterThanOrEqualTo(count * text.Length));

    //    FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    //}
}