// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.App.DataExportServices;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.DataExportServices;

[TestFixture]
internal class StringDataExportServiceTests
{

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorretctly()
    {
        // Arrange 

        // Act  
        var service = new StringDataExportService();

        // Assert
        Assert.That(service.CurrentFilePath, Is.Null);
    }

    [Test]
    public void Ctor_ValidNonDefaultSetup_PropsSetCorretctly()
    {
        // Arrange 

        // Act  
        var service = new StringDataExportService()
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
        var service = new StringDataExportService();

        // Act  
        var result = service.CreateCurrentFilePath();

        // Assert
        Assert.That(string.IsNullOrEmpty(result), Is.False);
    }

    [Test]
    public void Start_ValidDefaultSetup_CurrentFilePathSet()
    {
        // Arrange 
        var service = new StringDataExportService();

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

        var service = new StringDataExportService();
        service.Start();

        // Act  
        service.Add(text);

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

        var service = new StringDataExportService();
        service.Start();

        // Act
        for (var i = 0; i < 1000; i++)
        {
            service.Add(text);
        }

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);

        Assert.That(File.Exists(service.CurrentFilePath));

        Assert.That(service.RowCounter, Is.EqualTo(1000));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }
}
