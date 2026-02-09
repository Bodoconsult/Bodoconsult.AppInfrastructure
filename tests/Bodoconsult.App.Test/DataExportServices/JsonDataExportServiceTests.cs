// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.DataExportServices;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Test.Helpers;

namespace Bodoconsult.App.Test.DataExportServices;

[TestFixture]
internal class JsonDataExportServiceTests
{
    [SetUp]
    public void SetUp()
    {
        TestHelper.CleanTempPath();
    }

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorretctly()
    {
        // Arrange 

        // Act  
        var service = new JsonDataExportService<TestData>();

        // Assert
        Assert.That(service.CurrentFilePath, Is.Null);
    }

    [Test]
    public void Ctor_ValidNonDefaultSetup_PropsSetCorretctly()
    {
        // Arrange 

        // Act  
        var service = new JsonDataExportService<TestData>()
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
        var service = new JsonDataExportService<TestData>();

        // Act  
        var result = service.CreateCurrentFilePath();

        // Assert
        Assert.That(string.IsNullOrEmpty(result), Is.False);
    }

    [Test]
    public void Start_ValidDefaultSetup_CurrentFilePathSet()
    {
        // Arrange 
        var service = new JsonDataExportService<TestData>();

        // Act  
        service.Start();

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
    }

    [Test]
    public void Add_ValidDefaultSetup_FileWritten()
    {
        // Arrange 
        var data = new TestData();

        var service = new JsonDataExportService<TestData>();
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

        var service = new JsonDataExportService<TestData>();
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

        var service = new JsonDataExportService<TestData>
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
    public void TestExport_ListOfTestData_JsonExported()
    {
        // Arrange
        var list = new List<TestData>();
        var data = new TestData();

        for (var i = 0; i < 2; i++)
        {
            list.Add(data);
        }

        // Act
        var s = System.Text.Json.JsonSerializer.Serialize(list);

        // Assert
        Assert.That(string.IsNullOrEmpty(s), Is.False);

        Debug.Print(s);
    }

    [Test]
    public void Deserialize_ListOfTestData_JsonExportedDeserialized()
    {
        // Arrange
        var json = "[{\"Text\":\"Some text\",\"Date\":\"2026-02-09T17:27:20.1753682+01:00\",\"IsValid\":false,\"Number\":12345.67},{\"Text\":\"Some text\",\"Date\":\"2026-02-09T17:27:20.1753682+01:00\",\"IsValid\":false,\"Number\":12345.67}]";

        // Act
        var s = System.Text.Json.JsonSerializer.Deserialize<List<TestData>>(json);

        // Assert
        Assert.That(s, Is.Not.Null);
    }
}