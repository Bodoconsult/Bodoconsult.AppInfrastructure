// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataExportServices;
using System.Text;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.DataExportServices;

[TestFixture]
internal class ByteArrayDataExportServiceTests
{

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorretctly()
    {
        // Arrange 

        // Act  
        var service = new ByteArrayDataExportService();

        // Assert
        Assert.That(service.CurrentFilePath, Is.Null);
    }

    [Test]
    public void Ctor_ValidNonDefaultSetup_PropsSetCorretctly()
    {
        // Arrange 

        // Act  
        var service = new ByteArrayDataExportService()
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
        var service = new ByteArrayDataExportService();

        // Act  
        var result = service.CreateCurrentFilePath();

        // Assert
        Assert.That(string.IsNullOrEmpty(result), Is.False);
    }

    [Test]
    public void Start_ValidDefaultSetup_CurrentFilePathSet()
    {
        // Arrange 
        var service = new ByteArrayDataExportService();

        // Act  
        service.Start();

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
    }

    [Test]
    public void Add_ValidDefaultSetup_CurrentFilePathSet()
    {
        // Arrange 
        const string text = "Blubb";

        var data = Encoding.UTF8.GetBytes(text);

        var service = new ByteArrayDataExportService();
        service.Start();

        // Act  
        service.Add(data);

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);

        Assert.That(File.Exists(service.CurrentFilePath));

        Assert.That(service.RowCounter, Is.EqualTo(1));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }


    [Test]
    public void Add_ValidDefaultSetup1000_CurrentFilePathSet()
    {
        // Arrange 
        const string text = "Blubb\r\n";

        var data = Encoding.UTF8.GetBytes(text);

        var service = new ByteArrayDataExportService();
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

        Assert.That(service.RowCounter, Is.EqualTo(1000));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }

}