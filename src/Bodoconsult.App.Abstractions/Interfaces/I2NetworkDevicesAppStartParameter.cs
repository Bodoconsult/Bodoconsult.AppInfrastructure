// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// App start parameters with connection to two network devices
/// </summary>
public interface I2NetworkDevicesAppStartParameter : IAppStartParameter
{
    /// <summary>
    /// IP address of the network device 2
    /// </summary>
    public string IpAddress2 { get; set; }

    /// <summary>
    /// Port of the network device 2
    /// </summary>
    public int Port2 { get; set; }
}