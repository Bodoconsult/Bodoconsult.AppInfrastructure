// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bodoconsult.App.Logging;
using Bodoconsult.App.Test.DataExportServices;

namespace Bodoconsult.App.Test.Logging.LoggerProvider;

[TestFixture]
internal class Log4NetProviderTests
{
    [Test]
    public void CreateLogger_DefaultCtor_ReturnsLogger()
    {
        // Arrange 
        var provider = new Log4NetProvider();

        // Act  
        var logger = provider.CreateLogger("Default");

        // Assert
        Assert.That(logger, Is.Not.Null);
    }

    [Test]
    public void CreateLogger_CtorProvidingConfigFile_ReturnsLogger()
    {
        // Arrange 
        var s = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(s);

        var dir = new FileInfo(s).DirectoryName;
        ArgumentNullException.ThrowIfNull(dir);

        // ReSharper disable once AssignNullToNotNullAttribute
        s = Path.Combine(dir, "log4net.config");

        var provider = new Log4NetProvider(s);

        // Act  
        var logger = provider.CreateLogger("Default");

        // Assert
        Assert.That(logger, Is.Not.Null);
    }
}