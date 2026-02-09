// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using System.Reflection;
using Bodoconsult.App.Abstractions.EventCounters;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Benchmarking;
using Bodoconsult.App.ClientNotifications;
using Bodoconsult.App.Factories;
using Bodoconsult.App.Logging;
using Bodoconsult.App.Test.App;

namespace Bodoconsult.App.Test.Helpers;

public static class TestHelper
{
    /// <summary>
    /// Create a <see cref="IAppEventSource"/> instance
    /// </summary>
    /// <returns><see cref="IAppEventSource"/> instance based on <see cref="AppApmEventSource"/></returns>
    internal static AppApmEventSourceFactory CreateAppEventSourceFactory()
    {
        var logger = GetFakeAppLoggerProxy();

        var aes = new AppApmEventSourceFactory(logger);

        return aes;
    }

    public static IClientNotificationLicenseManager GetFakeLicenceManager()
    {
        return new FakeClientNotificationLicenseManager();
    }

    private static string _testDataPath;

    public static string TempPath = Path.GetTempPath();

    public static string TestDataPath
    {
        get
        {

            if (!string.IsNullOrEmpty(_testDataPath))
            {
                return _testDataPath;
            }

            var path = new DirectoryInfo(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName).Parent.Parent.Parent.FullName;

            _testDataPath = Path.Combine(path, "TestData");

            if (!Directory.Exists(_testDataPath))
            {
                Directory.CreateDirectory(_testDataPath);
            }

            return _testDataPath;
        }
    }

    /// <summary>
    /// Start an app by file name
    /// </summary>
    /// <param name="fileName"></param>
    public static void StartFile(string fileName)
    {

        if (!Debugger.IsAttached)
        {
            return;
        }

        Assert.That(File.Exists(fileName));

        var p = new Process { StartInfo = new ProcessStartInfo { UseShellExecute = true, FileName = fileName } };

        p.Start();

    }

    /// <summary>
    /// Get a fully set up fake logger
    /// </summary>
    /// <returns>Logger instance</returns>
    public static AppLoggerProxy GetFakeAppLoggerProxy()
    {
        if (_logger != null)
        {
            return _logger;
        }
        _logger = new AppLoggerProxy(new FakeLoggerFactory(), Globals.Instance.LogDataFactory);
        return _logger;
    }

    private static AppLoggerProxy _logger;

    /// <summary>
    /// Get a fully setup fake bench logger
    /// </summary>
    /// <returns>Bench logger instance</returns>
    public static AppBenchProxy GetFakeAppBenchProxy()
    {
        if (_bench != null)
        {
            return _bench;
        }
        _bench = new AppBenchProxy(new FakeLoggerFactory(), Globals.Instance.LogDataFactory);
        return _bench;
    }

    private static AppBenchProxy _bench;

    /// <summary>
    /// Clean the TEMP path from txt and bin files
    /// </summary>
    public static void CleanTempPath()
    {
        var path = Path.GetTempPath();

        var dir = new DirectoryInfo(path);

        foreach (var file in dir.GetFiles("DataExport*.txt"))
        {
            try
            {
                file.Delete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        foreach (var file in dir.GetFiles("DataExport*.bin"))
        {
            try
            {
                file.Delete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}