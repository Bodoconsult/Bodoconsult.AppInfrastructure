// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataExportServices;

namespace Bodoconsult.App.Test.DataExportServices;

[TestFixture]
internal class FakeDataExportServiceTests
{
    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var service = new FakeDataExportService();

        // Assert
        Assert.That(service.BytesLogged, Is.Zero);
        Assert.That(service.WasLogged, Is.False);
    }

    [Test]
    public void Add_OneItem_PropsSetCorrectly()
    {
        // Arrange 
        var service = new FakeDataExportService();

        var data = new byte[] { 0x0, 0x1 };

        // Act  
        service.Add(data);

        // Assert
        Assert.That(service.BytesLogged, Is.Not.Zero);
        Assert.That(service.WasLogged, Is.True);
    }
}