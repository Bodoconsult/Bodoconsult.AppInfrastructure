// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for a central app date service providing current date in a testable manner
/// </summary>
public interface IAppDateService
{
    /// <summary>
    /// Deliver the current date the app is running on
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// return the current date only
    /// </summary>
    DateTime Today { get; }

    /// <summary>
    /// Get a valid MS Access datetime value. Values before 1/1/1900 are set to this value
    /// </summary>
    /// <param name="date">A given date value or null</param>
    /// <returns>Valid MS Access datetime value</returns>
    public DateTime GetValidAccessDate(DateTime? date);

    /// <summary>
    /// Get the number of ticks from the beginning of time. Only one access per time possible!
    /// </summary>
    /// <returns>Ticks from the beginning of time</returns>
    public long GetCurrentTicks();
}