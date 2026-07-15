// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Helpers;

/// <summary>
    /// Default implementation of <see cref="IWatchDog"/>. A watchdog meant here is an interval based polling mechanism.
    /// A timer is fired always at the fixed time interval. This may lead to multiple <see cref="WatchDogRunnerDelegate"/> instances running if the
    /// runtime of each of thisinstances is longer than the timer interval. A watchdog runs a <see cref="WatchDogRunnerDelegate"/> instance and
    /// afterwards it waits for the <see cref="DelayUntilNextRunnerFired"/> interval before running the next instance
    /// </summary>
    public class WatchDog : IWatchDog
{
    private CancellationTokenSource _cancellationToken;
    private readonly ThreadPriority _threadPriority;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="watchDogRunnerDelegate"></param>
    /// <param name="delayUntilNextRunnerFired">Delay in ms until next run</param>
    public WatchDog(WatchDogRunnerDelegate watchDogRunnerDelegate, int delayUntilNextRunnerFired)
    {
        ArgumentNullException.ThrowIfNull(watchDogRunnerDelegate);
        WatchDogRunnerDelegate = watchDogRunnerDelegate ?? throw new ArgumentNullException(nameof(watchDogRunnerDelegate));
        DelayUntilNextRunnerFired = delayUntilNextRunnerFired;
        _threadPriority = ThreadPriority.Normal;
    }

    /// <summary>
    /// Ctor with additional thread priority setting
    /// </summary>
    /// <param name="watchDogRunnerDelegate"></param>
    /// <param name="delayUntilNextRunnerFired">Delay until next run</param>
    /// <param name="threadPriority">Thread priority</param>
    public WatchDog(WatchDogRunnerDelegate watchDogRunnerDelegate, int delayUntilNextRunnerFired, ThreadPriority threadPriority)
    {
        ArgumentNullException.ThrowIfNull(watchDogRunnerDelegate);
        WatchDogRunnerDelegate = watchDogRunnerDelegate;
        DelayUntilNextRunnerFired = delayUntilNextRunnerFired;
        _threadPriority = threadPriority;
    }

    /// <summary>
    /// The method to run by the watchdog
    /// </summary>
    public WatchDogRunnerDelegate WatchDogRunnerDelegate { get; }

    /// <summary>
    /// Is the watchdog activated? If yes, <see cref="IWatchDog.WatchDogRunnerDelegate"/> is called.
    /// If no the <see cref="IWatchDog.WatchDogRunnerDelegate"/> is NOT called.
    /// </summary>
    public bool IsActivated { get; set; } = true;

    /// <summary>
    /// The delay after the runner method was running in ms
    /// </summary>
    public int DelayUntilNextRunnerFired { get; set; }

    /// <summary>
    /// Start the watchdog
    /// </summary>
    public void StartWatchDog()
    {
        if (_cancellationToken != null)
        {
            _cancellationToken.Cancel(false);
            _cancellationToken.Dispose();
            _cancellationToken = null;
            //Debug.Print("WatchDog already alive");
        }

        _cancellationToken = new CancellationTokenSource();
        var wait = new AutoResetEvent(false);
        Task.Factory.StartNew(async () =>
        {
            await RunInternal(wait);
        }, TaskCreationOptions.LongRunning);

        wait.WaitOne(500);

        IsActivated = true;
    }

    /// <summary>
    /// Run the watchdog
    /// </summary>
    public async Task RunInternal(AutoResetEvent wait)
    {
        Thread.CurrentThread.Priority = _threadPriority;

        wait.Set();

        while (!_cancellationToken.IsCancellationRequested)
        {
            if (IsActivated)
            {
                //Run the delegate
                WatchDogRunnerDelegate.Invoke();
            }

            // Delay the thread as requested
            await Task.Delay(DelayUntilNextRunnerFired, _cancellationToken.Token);
        }

        WatchDogRunnerDelegate.Invoke();
    }

    /// <summary>
    /// Stop the watchdog
    /// </summary>
    public void StopWatchDog()
    {
        try
        {
            _cancellationToken?.Cancel(false);
            _cancellationToken?.Dispose();
            _cancellationToken = null;
            IsActivated = false;
        }
        catch //(Exception e)
        {
            // Do nothing
        }
    }
}