// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using BodoFileTransferCore.Business.Enums;
using BodoFileTransferCore.Business.Logging;
using BodoFileTransferCore.Business.Model;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test.Logging;

[TestFixture]
internal class UnitTestsAppLoggerProxy : BaseFakeLoggerTests
{
    private AppLoggerProxy _log;


    [SetUp]
    public void Setup()
    {
        LoggedMessages.Clear();
    }

    [Test]
    public void TestCheckQueueWithExternalDelegateSuccess()
    {
        // Arrange 
        var appLoggerFactory = new FakeLoggerFactory
        {
            FakeLogDelegate = FakeLogDelegate
        };

        _log = new AppLoggerProxy(appLoggerFactory);

        // Act
        _log.LogWarning("Hallo");
        _log.CheckQueue();

        // Assert
        Assert.That(LoggedMessages.Count == 1);

        appLoggerFactory.Dispose();
    }

    [Test]
    public void TestCheckQueueWithInternalDelegateSuccess()
    {
        // Arrange 
        var appLoggerFactory = new FakeLoggerFactory();
        _log = new AppLoggerProxy(appLoggerFactory);

        // Act
        _log.LogWarning("Hallo");
        _log.CheckQueue();

        // Assert
        Assert.That(appLoggerFactory.LoggedMessages.Count == 1);

        appLoggerFactory.Dispose();
    }

    [Test]
    public void TestCheckQueueWithInternalDelegateSuccessMultipleLogs()
    {
        // Arrange 
        var queryLogger = new FakeLoggerFactory();
        _log = new AppLoggerProxy(queryLogger);

        // Act
        _log.LogWarning("Hallo");
        _log.CheckQueue();

        _log.LogWarning("Hallo");
        _log.CheckQueue();

        _log.LogWarning("Hallo");
        _log.CheckQueue();

        // Assert
        Assert.That(queryLogger.LoggedMessages.Count == 3);
        queryLogger.Dispose();
    }


    [Test]
    public void TestCheckQueueWithExternalDelegateNoSuccess()
    {
        // Arrange 
        var appLogger = new FakeLoggerFactory
        {
            FakeLogDelegate = FakeLogDelegate
        };

        _log = new AppLoggerProxy(appLogger);

        // Act
        _log.LogInformation("Hallo");
        _log.CheckQueue();

        // Assert
        Assert.That(LoggedMessages.Count == 0);
        appLogger.Dispose();
    }

    [Test]
    public void TestCheckQueueWithInternalDelegateNoSuccess()
    {
        // Arrange 
        var appLogger = new FakeLoggerFactory();
        _log = new AppLoggerProxy(appLogger);

        // Act
        _log.LogInformation("Hallo");
        _log.CheckQueue();

        // Assert
        Assert.That(appLogger.LoggedMessages.Count == 0);
        appLogger.Dispose();
    }


    [Test]
    public void TestFormatArgsString()
    {
        // Arrange 
        var input = "test";

        // Act  
        var result = AppLoggerProxy.FormatArgs([input]);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(!string.IsNullOrEmpty(result));

    }

    [Test]
    public void TestFormatArgsObject()
    {
        // Arrange 
        var input = new TraceEntry
        {
            Id = Guid.NewGuid(),
            MessageCode = TraceMessageCode.FileSent,
            Message = "Blubb"
        };

        // Act  
        var result = AppLoggerProxy.FormatArgs([input]);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(!string.IsNullOrEmpty(result));

    }


    [Test]
    public void TestFormatArgsException()
    {
        // Arrange 
        var input = new ArgumentException("Hallo");

        // Act  
        var result = AppLoggerProxy.FormatArgs([input]);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(!string.IsNullOrEmpty(result));

    }

}