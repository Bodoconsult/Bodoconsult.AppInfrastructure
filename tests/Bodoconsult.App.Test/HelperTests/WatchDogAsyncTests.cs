// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
internal class WatchDogAsyncTests
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
    private Task Runner()
    {
        _isFired = true;
        _firedCount++;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Runner emthod for the watchdog
    /// </summary>
    private void RunnerAsync()
    {
        _isFired = true;
        _firedCount++;
    }
    
    [Test]
    public void Ctor_DefaultSetup_PropsSetCorrectly()
    {
        // Arrange 
        _isFired = false;
        _firedCount = 0;
        const int delayTime = 500;

        WatchDogRunnerDelegateAsync runner = Runner;

        var w = new WatchDogAsync(runner, delayTime);

        // Act  
        AsyncHelper.Delay((int)(delayTime * 1.5));

        // Assert
        Assert.That(w.WatchDogRunnerDelegate, Is.EqualTo(runner));
        Assert.That(w.DelayUntilNextRunnerFired, Is.EqualTo(delayTime));
        Assert.That(!_isFired);

    }

    [Test]
    public void StartWatchDog_ValidSetup_IsFired()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 500;

        WatchDogRunnerDelegateAsync runner = Runner;

        var w = new WatchDogAsync(runner, delayTime);
        w.StartWatchDog();

        // Act  
        AsyncHelper.Delay((int)(delayTime * 5));

        // Assert
        w.StopWatchDog();
        Assert.That(_isFired);
        Assert.That(_firedCount is > 3 and < 6);
    }

    [Test]
    public void StartWatchDog_ValidSetup2TimesStarted_IsFired()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 500;

        WatchDogRunnerDelegateAsync runner = Runner;

        var w = new WatchDogAsync(runner, delayTime);
        w.StartWatchDog();

        // Act  
        AsyncHelper.Delay((int)(delayTime * 2.5));

        // Assert
        w.StopWatchDog();
        Assert.That(_isFired);
        Assert.That(_firedCount > 1);
    }

    [Test]
    public void Restart_ValidSetup_IsFired()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 500;

        WatchDogRunnerDelegateAsync runner = Runner;

        var w = new WatchDogAsync(runner, delayTime);

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
        Assert.That(_firedCount, Is.GreaterThanOrEqualTo(7));
    }
}