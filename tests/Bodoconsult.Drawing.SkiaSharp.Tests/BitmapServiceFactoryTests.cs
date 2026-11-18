// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Drawing.SkiaSharp.Factories;

namespace Bodoconsult.Drawing.SkiaSharp.Tests;

[TestFixture]
internal class BitmapServiceFactoryTests
{
    [Test]
    public void CreateInstance_ValidSetup_ReturnsService()
    {
        // Arrange 
        var factory = new BitmapServiceFactory();

        // Act  
        var result = factory.CreateInstance();

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}