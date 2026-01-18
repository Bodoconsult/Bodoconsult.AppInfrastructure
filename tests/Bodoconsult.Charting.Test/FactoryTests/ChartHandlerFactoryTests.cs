// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Charting.Factories;
using Bodoconsult.Drawing.SkiaSharp.Factories;
using NUnit.Framework;

namespace Bodoconsult.Charting.Test.FactoryTests;

[TestFixture]
internal class ChartHandlerFactoryTests
{

    [Test]
    public void CreateInstance_validSetup_InstanceCreated()
    {
        // Arrange 
        var bitmapServiceFactory = new BitmapServiceFactory();
        var factory = new ChartHandlerFactory(bitmapServiceFactory);
        var data = new ChartData();

        // Act  
        var result = factory.CreateInstance(data);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ChartData, Is.Not.Null);
    }

}