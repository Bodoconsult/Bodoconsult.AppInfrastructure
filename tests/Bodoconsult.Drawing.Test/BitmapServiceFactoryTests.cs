// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Drawing.Factories;
using NUnit.Framework;

namespace Bodoconsult.Drawing.Test;

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