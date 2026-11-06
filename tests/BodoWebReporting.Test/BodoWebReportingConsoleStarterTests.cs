// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using NUnit.Framework;

namespace BodoWebReporting.Test;

[TestFixture]
internal class BodoWebReportingConsoleStarterTests
{
    [Test]
    public void Start_ValidSetup_Success()
    {
        // Arrange 
        var starter = new BodoWebReportingConsoleStarter([]);

        // Act  
        var result = starter.Start();

        // Assert
        Assert.That(result, Is.Zero);
    }
}