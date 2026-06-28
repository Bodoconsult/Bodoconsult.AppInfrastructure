// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Default implementation of <see cref="IWatchDog"/>. A watchdog meant here is a interval based polling mechanism
/// </summary>
public class WatchDog : IWatchDog
{

    private CancellationTokenSource _cancellationToken;

    private Thread _watchDogThread;

    private readonly ThreadPriority _threadPriority;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="watchDogRunnerDelegate"></param>
    /// <param name="delayUntilNextRunnerFired">Delay until next run</param>
    public WatchDog(WatchDogRunnerDelegate watchDogRunnerDelegate, int delayUntilNextRunnerFired)
    {
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
    /// The delay after the runner method was running in milliseconds
    /// </summary>
    public int DelayUntilNextRunnerFired { get; set; }

    /// <summary>
    /// Start the watchdog
    /// </summary>
    public void StartWatchDog()
    {

        if (_watchDogThread != null)
        {
            _cancellationToken?.Cancel();
            _watchDogThread.Join();
            _watchDogThread = null;
            _cancellationToken?.Dispose();
            //Debug.Print("WatchDog already alive");
        }

        _cancellationToken = new CancellationTokenSource();
        _watchDogThread = new Thread(RunInternal)
        {
            Priority = _threadPriority,
            IsBackground = true
        };
        _watchDogThread.Start();

        IsActivated = true;
    }

    /// <summary>
    /// Run the watchdog
    /// </summary>
    public void RunInternal()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {

            if (IsActivated)
            {
                //Run the delegate
                WatchDogRunnerDelegate?.Invoke();
            }

            // Proceed only if not cancelled
            if (_cancellationToken.IsCancellationRequested)
            {
                break;
            }

            // Delay the thread as requested
            AsyncHelper.Delay(DelayUntilNextRunnerFired);

        }

        WatchDogRunnerDelegate?.Invoke();

    }

    /// <summary>
    /// Stop the watchdog
    /// </summary>
    public void StopWatchDog()
    {
        try
        {
            _cancellationToken?.Cancel();
            _watchDogThread?.Join();
            _watchDogThread = null;
            _cancellationToken?.Dispose();
            IsActivated = false;
        }
        catch //(Exception e)
        {
            // Do nothing
        }

    }
}