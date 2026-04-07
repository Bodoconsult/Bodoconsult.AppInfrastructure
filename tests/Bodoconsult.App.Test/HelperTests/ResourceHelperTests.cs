// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reflection;
using Bodoconsult.App.Abstractions.Helpers;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
internal class ResourceHelperTests
{
    [Test]
    public void GetTextResource_NoAssembly_ReturnsString()
    {
        // Arrange 
        const string resourceName = "Bodoconsult.App.Test.Resources.Test.txt";

        // Act  
        var result = ResourceHelper.GetTextResource(resourceName);

        // Assert
        Assert.That(string.IsNullOrEmpty(result), Is.False);
    }

    [Test]
    public void GetTextResource_WithAssembly_ReturnsString()
    {
        // Arrange 
        const string resourceName = "Bodoconsult.App.Test.Resources.Test.txt";
        var ass = Assembly.GetExecutingAssembly();

        // Act  
        var result = ResourceHelper.GetTextResource(ass, resourceName);

        // Assert
        Assert.That(string.IsNullOrEmpty(result), Is.False);
    }

    [Test]
    public void GetByteResource_NoAssembly_ReturnsString()
    {
        // Arrange 
        const string resourceName = "Bodoconsult.App.Test.Resources.Test.txt";

        // Act  
        var result = ResourceHelper.GetByteResource(resourceName);

        // Assert
        Assert.That(result.Length, Is.Not.Zero);
    }

    [Test]
    public void GetByteResource_WithAssembly_ReturnsString()
    {
        // Arrange 
        const string resourceName = "Bodoconsult.App.Test.Resources.Test.txt";
        var ass = Assembly.GetExecutingAssembly();

        // Act  
        var result = ResourceHelper.GetByteResource(ass, resourceName);

        // Assert
        Assert.That(result.Length, Is.Not.Zero);
    }
}