// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.DataExportServices;

[TestFixture]
internal class TestDataExportServiceTests
{
    [Test]
    public void Add_ValidDefaultSetup1000000_FileWritten()
    {
        // Arrange 
        var data = new TestData();

        var service = new TestDataExportService();
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
        Assert.That(service.RowCounter, Is.EqualTo(1000000));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }
}