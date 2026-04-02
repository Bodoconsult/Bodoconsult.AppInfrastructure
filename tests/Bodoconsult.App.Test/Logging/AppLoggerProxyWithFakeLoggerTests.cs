// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Logging;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
internal class AppLoggerProxyWithFakeLoggerTests : BaseFakeLoggerTests
{
    private AppLoggerProxy _log;

    private readonly LogDataFactory _logDataFactory = new();


    [SetUp]
    public void Setup()
    {
        LoggedMessages.Clear();
    }

    [Test]
    public void CheckQueue_WithExternalDelegateLogWarning_Success()
    {
        // Arrange 
        var appLoggerFactory = new FakeLoggerFactory
        {
            FakeLogDelegate = FakeLogDelegate
        };

        _log = new AppLoggerProxy(appLoggerFactory, _logDataFactory);

        // Act
        _log.LogWarning("Hallo");

        Wait.Until(() => LoggedMessages.Count > 0, 2000);

        // Assert
        Assert.That(LoggedMessages.Count == 1);

        appLoggerFactory.Dispose();
    }

    [Test]
    public void CheckQueue_WithExternalDelegateLogError_Success()
    {
        // Arrange 
        var appLoggerFactory = new FakeLoggerFactory
        {
            FakeLogDelegate = FakeLogDelegate
        };

        _log = new AppLoggerProxy(appLoggerFactory, _logDataFactory);

        var ex = new Exception("Blubb message");

        // Act
        _log.LogError(ex, "Hallo", [ex]);
        Wait.Until(() => LoggedMessages.Count > 0, 2000);

        // Assert
        Assert.That(LoggedMessages.Count == 1);

        appLoggerFactory.Dispose();
    }

    [Test]
    public void CheckQueue_WithExternalDelegateLogCritical_Success()
    {
        // Arrange 
        var appLoggerFactory = new FakeLoggerFactory
        {
            FakeLogDelegate = FakeLogDelegate
        };

        _log = new AppLoggerProxy(appLoggerFactory, _logDataFactory);

        var ex = new Exception("Blubb message");

        // Act
        _log.LogCritical(ex, "Hallo", [ex]);
        Wait.Until(() => LoggedMessages.Count > 0, 2000);

        // Assert
        Assert.That(LoggedMessages.Count == 1);

        appLoggerFactory.Dispose();
    }

    //[Test]
    //public void CheckQueueWithInternalDelegateSuccess()
    //{
    //    // Arrange 
    //    var appLoggerFactory = new FakeLoggerFactory();
    //    _log = new AppLoggerProxy(appLoggerFactory);

    //    // Act
    //    _log.LogWarning("Hallo");
    //    var task = AsyncHelper.Delay(2 * AppLoggerProxy.DelayTimeQueueAccess);
    //    task.Wait();

    //    // Assert
    //    Assert.IsTrue(appLoggerFactory.LoggedMessages.Count == 1);

    //    appLoggerFactory.Dispose();
    //}

    //[Test]
    //public void TestCheckQueueWithInternalDelegateSuccessMultipleLogs()
    //{
    //    // Arrange 
    //    var queryLogger = new FakeLoggerFactory();
    //    _log = new AppLoggerProxy(queryLogger);

    //    // Act
    //    _log.LogWarning("Hallo");
    //    //_log.CheckQueue();

    //    _log.LogWarning("Hallo");
    //    //_log.CheckQueue();

    //    _log.LogWarning("Hallo");
    //    //_log.CheckQueue();

    //    var task = AsyncHelper.Delay(2 * AppLoggerProxy.DelayTimeQueueAccess);
    //    task.Wait();

    //    // Assert
    //    Assert.IsTrue(queryLogger.LoggedMessages.Count == 3);
    //    queryLogger.Dispose();
    //}


    [Test]
    public void CheckQueue_WithExternalDelegate_NoSuccess()
    {
        // Arrange 
        var appLogger = new FakeLoggerFactory
        {
            FakeLogDelegate = FakeLogDelegate
        };

        _log = new AppLoggerProxy(appLogger, _logDataFactory);

        // Act
        _log.LogInformation("Hallo");

        Wait.Until(() => LoggedMessages.Count > 0, 2000);

        // Assert
        Assert.That(LoggedMessages.Count == 0);
        appLogger.Dispose();
    }

    [Test]
    public void CheckQueue_WithInternalDelegate_NoSuccess()
    {
        // Arrange 
        var appLogger = new FakeLoggerFactory();
        _log = new AppLoggerProxy(appLogger, _logDataFactory);

        // Act
        _log.LogInformation("Hallo");

        Wait.Until(() => LoggedMessages.Count > 0, 2000);

        // Assert
        Assert.That(appLogger.LoggedMessages.Count == 0);
        appLogger.Dispose();
    }


    [Test]
    public void FormatArgs_StringInput_StringIsFormatted()
    {
        // Arrange 
        const string input = "test";
        var result = new StringBuilder();

        // Act  
        AppLoggerProxy.FormatArgs([input], result);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(string.IsNullOrEmpty(result.ToString()), Is.False);

    }

    [Test]
    public void FormatArgs_ObjectInput_StringIsFormatted()
    {
        // Arrange 
        var input = new LoggingConfig();
        var result = new StringBuilder();

        // Act  
        AppLoggerProxy.FormatArgs([input], result);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(string.IsNullOrEmpty(result.ToString()), Is.False);

    }


    [Test]
    public void FormatArgs_Exception_StringIsFormatted()
    {
        // Arrange 
        var input = new ArgumentException("Hallo");
        var result = new StringBuilder();

        // Act  
        AppLoggerProxy.FormatArgs([input], result);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(string.IsNullOrEmpty(result.ToString()), Is.False);

    }

}