// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.CentralServices;

/// <summary>
/// Fake implementation of <see cref="IAppDateService"/> for unit testing time dependent tasks
/// </summary>
public class FakeAppDateService : IAppDateService
{
    /// <summary>
    /// Factory method for <see cref="FakeAppDateService"/>
    /// </summary>
    /// <returns>Fresh instance of <see cref="FakeAppDateService"/></returns>
    public IAppDateService CreateInstance()
    {
        var dts = new FakeAppDateService();
        return dts;
    }

    /// <summary>
    /// Minimum date MS Access can handle
    /// </summary>
    private readonly DateTime _accessMinDate = new(1900, 1, 1);

    private readonly Lock _lock = new();

    /// <summary>
    /// return current date and time
    /// </summary>
    public DateTime Now => DateTimeToDeliver + Offset;

    /// <summary>
    /// return the current date only
    /// </summary>
    public DateTime Today => DateTime.Today + Offset;

    /// <summary>
    /// Reset the current offset to a zero time span
    /// </summary>
    public void ResetOffset()
    {
        Offset = TimeSpan.Zero;
    }

    /// <summary>
    /// Set an offset value to be added to the system date fo calculate <see cref="Now"/> and <see cref="Today"/>
    /// </summary>
    public TimeSpan Offset { get; set; }

    /// <summary>
    /// DateTime to deliver
    /// </summary>
    public DateTime DateTimeToDeliver { get; set; } = DateTime.Now;


    /// <returns>Valid access datetime value</returns>
    public DateTime GetValidAccessDate(DateTime? date)
    {

        if (!date.HasValue)
        {
            return DateTime.Now;
        }

        // Dates below 1900/1/1 cannot be handled by Access
        return date.Value >= _accessMinDate ? date.Value : _accessMinDate;
    }

    /// <summary>
    /// Get the number of ticks from the beginning of time. Only one access per time possible!
    /// </summary>
    /// <returns>Ticks from the beginning of time</returns>
    public long GetCurrentTicks()
    {
        lock (_lock)
        {
            return (DateTimeToDeliver + Offset).Ticks;
        }
    }
}