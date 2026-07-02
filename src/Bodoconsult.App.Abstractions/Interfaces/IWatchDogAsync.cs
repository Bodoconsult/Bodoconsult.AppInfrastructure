// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Delegate fired by <see cref="IWatchDogAsync"/> implementations
/// </summary>
public delegate Task WatchDogRunnerDelegateAsync();

/// <summary>
/// Interface for watchdog implementations. A watchdog meant here is an interval based polling mechanism.
/// A timer is fired always at the fixed time interval. This may lead to multiple <see cref="WatchDogRunnerDelegate"/> instances running if the
/// runtime of each of thisinstances is longer than the timer interval. A watchdog runs a <see cref="WatchDogRunnerDelegate"/> instance and
/// afterwards it waits for the <see cref="DelayUntilNextRunnerFired"/> interval before running the next instance
/// </summary>
public interface IWatchDogAsync
{
    /// <summary>
    /// The method to run by the watchdog
    /// </summary>
    WatchDogRunnerDelegateAsync WatchDogRunnerDelegate { get; }

    /// <summary>
    /// Is the watchdog activated? If yes, <see cref="WatchDogRunnerDelegate"/> is called.
    /// If no the <see cref="WatchDogRunnerDelegate"/> is NOT called.
    /// </summary>
    bool IsActivated { get; set; }
    
    /// <summary>
    /// The delay after the runner method was running in milliseconds
    /// </summary>
    int DelayUntilNextRunnerFired{ get; set; }
    
    /// <summary>
    /// Start the watchdog
    /// </summary>
    void StartWatchDog();

    /// <summary>
    /// Stop the watchdog
    /// </summary>
    void StopWatchDog();
}