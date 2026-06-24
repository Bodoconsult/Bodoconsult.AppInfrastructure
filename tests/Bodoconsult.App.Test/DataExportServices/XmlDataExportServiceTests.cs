// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataExportServices;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Test.Helpers;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Bodoconsult.App.Test.DataExportServices;

[TestFixture]
internal class XmlDataExportServiceTests
{
    [SetUp]
    public void SetUp()
    {
        TestHelper.CleanTempPath();
    }

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var service = new XmlDataExportService<TestData>();

        // Assert
        Assert.That(service.CurrentFilePath, Is.Null);
    }

    [Test]
    public void Ctor_ValidNonDefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var service = new XmlDataExportService<TestData>()
        {
            FileName = "Export"
        };

        // Assert
        Assert.That(service.CurrentFilePath, Is.Null);
    }

    [Test]
    public void CreateCurrentFilePath_ValidDefaultSetup_ValidPathReturned()
    {
        // Arrange 
        var service = new XmlDataExportService<TestData>();

        // Act  
        var result = service.CreateCurrentFilePath();

        // Assert
        Assert.That(string.IsNullOrEmpty(result), Is.False);
    }

    [Test]
    public void Add_ValidDefaultSetup_FileWritten()
    {
        // Arrange 
        var data = new TestData();

        var service = new XmlDataExportService<TestData>();
        service.Start();

        // Act  
        service.Add(data);

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
        Assert.That(File.Exists(service.CurrentFilePath));
        Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(1));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }


    [Test]
    public void Add_ValidDefaultSetup1000_FileWritten()
    {
        // Arrange 
        var data = new TestData();

        var service = new XmlDataExportService<TestData>
        {
            MaxFileSize = 5000
        };
        service.Start();

        // Act
        for (var i = 0; i < 1000; i++)
        {
            service.Add(data);
        }

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
        Assert.That(File.Exists(service.CurrentFilePath));
        Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(1000));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }

    [Test]
    public void Add_ValidDefaultSetup1000000_FileWritten()
    {
        // Arrange 
        var data = new TestData();

        var service = new XmlDataExportService<TestData>
        {
            MaxFileSize = 3000
        };
        service.Start();

        // Act
        for (var i = 0; i < 1000000; i++)
        {
            service.Add(data);
        }

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
        Assert.That(File.Exists(service.CurrentFilePath));
        Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(1000000));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }


    [Test]
    public void TestExport_ListOfTestData_XmlExported()
    {
        // Arrange
        var list = new List<TestData>();
        var data = new TestData();

        for (var i = 0; i < 2; i++)
        {
            list.Add(data);
        }

        // Act
        var xmlSerializer = new XmlSerializer(list.GetType());

        using var textWriter = new StringWriter();
        xmlSerializer.Serialize(textWriter, list);
        var s = textWriter.ToString();

        // Assert
        Assert.That(string.IsNullOrEmpty(s), Is.False);

        Debug.Print(s);
    }
}