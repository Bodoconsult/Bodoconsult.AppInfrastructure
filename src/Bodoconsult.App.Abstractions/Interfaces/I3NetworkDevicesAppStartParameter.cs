// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// App start parameters with connection to three network devices
/// </summary>
public interface I3NetworkDevicesAppStartParameter : IAppStartParameter
{
    /// <summary>
    /// IP address of the network device 2
    /// </summary>
    public string IpAddress2 { get; set; }

    /// <summary>
    /// Port of the network device 2
    /// </summary>
    public int Port2 { get; set; }

    /// <summary>
    /// IP address of the network device 3
    /// </summary>
    public string IpAddress3 { get; set; }

    /// <summary>
    /// Port of the network device 3
    /// </summary>
    public int Port3 { get; set; }
}