// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface to create <see cref="IMonitorLoggerFactory"/> factories
/// </summary>
public interface IMonitorLoggerFactoryFactory
{
    /// <summary>
    /// Create a monitor logger factory
    /// </summary>
    /// <param name="deviceName">Current tower serial number</param>
    /// <returns></returns>
    IMonitorLoggerFactory? CreateInstance(string deviceName);

    /// <summary>
    /// Create a monitor logger factory
    /// </summary>
    /// <param name="clientType">Client or device type as string</param>
    /// <param name="ipAddress">Current IP address of the client</param>
    /// <returns>Monitor logger factory</returns>
    IMonitorLoggerFactory? CreateInstance(string clientType, string ipAddress);
}