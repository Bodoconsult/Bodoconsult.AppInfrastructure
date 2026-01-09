// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.IO;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Charting.DependencyInjection;
using Bodoconsult.Charting.Factories;
using NUnit.Framework;

namespace Bodoconsult.Charting.Test.DependencyInjectionTests;

[TestFixture]

public class ChartingDiContainerServiceProviderTests
{
    [Test]
    public void AddServices_ValidSetup_DependenciesAdded()
    {
        // Arrange 
        var di = new DiContainer();
        var provider = new ChartingDiContainerServiceProvider();

        // Act  
        provider.AddServices(di);
        di.BuildServiceProvider();

        // Assert
        var result = di.Get<IChartHandlerFactory>();
        Assert.That(result, Is.Not.Null);

        // Use DI
        var data = new ChartData
        {
            FileName = Path.Combine(Path.GetTempPath(), "test.png")
            // configuring chartdata as required has to be added here
        }; 

        var ch = result.CreateInstance(data); // Create a chart handler instance now

        ch.Export(); // Export the chart to PNG file
    }

}