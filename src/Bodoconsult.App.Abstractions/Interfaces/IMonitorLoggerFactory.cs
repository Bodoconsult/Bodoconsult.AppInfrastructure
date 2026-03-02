// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface to separate monitor loggers from an app logger
/// </summary>
public interface IMonitorLoggerFactory : ILoggerFactory
{
    /// <summary>
    /// Current full file path to log int
    /// </summary>
    string FileName { get;  }
}