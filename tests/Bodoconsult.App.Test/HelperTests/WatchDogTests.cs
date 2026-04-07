// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Test.DataExportServices;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
internal class WatchDogTests
{
    private bool _isFired;
    private int _firedCount;

    [SetUp]
    public void TestSetup()
    {
        _isFired = false;
        _firedCount = 0;
    }


    /// <summary>
    /// Runner method for the watchdog
    /// </summary>
    private void Runner()
    {
        _isFired = true;
        _firedCount++;
    }

    /// <summary>
    /// Runner emthod for the watchdog
    /// </summary>
    private async void RunnerAsync()
    {
        await Task.Run(() =>
        {
            _isFired = true;
            _firedCount++;
        });

    }


    [Test]
    public void TestCtor()
    {
        // Arrange 
        _isFired = false;
        _firedCount = 0;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = Runner;

        var w = new WatchDog(runner, delayTime);

        // Act  
        AsyncHelper.Delay((int)(delayTime * 1.5));

        // Assert
        Assert.That(w.WatchDogRunnerDelegate, Is.EqualTo(runner));
        Assert.That(w.DelayUntilNextRunnerFired, Is.EqualTo(delayTime));
        Assert.That(!_isFired);

    }

    [Test]
    public void TestStart()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = Runner;

        var w = new WatchDog(runner, delayTime);
        w.StartWatchDog();

        // Act  
        AsyncHelper.Delay((int)(delayTime * 1.5));

        // Assert
        w.StopWatchDog();
        Assert.That(_isFired);
        Assert.That(_firedCount > 1);
    }

    [Test]
    public void TestStart2TimesFired()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = Runner;

        var w = new WatchDog(runner, delayTime);
        w.StartWatchDog();

        // Act  
        AsyncHelper.Delay((int)(delayTime * 2.5));

        // Assert
        w.StopWatchDog();
        Assert.That(_isFired);
        Assert.That(_firedCount > 1);
    }


    [Test]
    public void TestStartAsync()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = RunnerAsync;

        var w = new WatchDog(runner, delayTime);
        w.StartWatchDog();

        // Act  
        AsyncHelper.Delay((int)(delayTime * 1.5));

        // Assert
        w.StopWatchDog();
        Assert.That(_isFired);
        Assert.That(_firedCount > 1);
    }


    [Test]
    public void TestRestart()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = Runner;

        var w = new WatchDog(runner, delayTime);

        // Act  1
        w.StartWatchDog();
        AsyncHelper.Delay((int)(delayTime * 2.5));
        w.StopWatchDog();

        // Act  2
        w.StartWatchDog();
        AsyncHelper.Delay((int)(delayTime * 2.5));
        w.StopWatchDog();

        // Act  3
        w.StartWatchDog();
        AsyncHelper.Delay((int)(delayTime * 2.5));
        w.StopWatchDog();

        // Assert

        Assert.That(_isFired);
        Assert.That(_firedCount, Is.GreaterThanOrEqualTo(10));
    }

}