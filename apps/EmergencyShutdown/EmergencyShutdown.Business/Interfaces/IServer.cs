// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace EmergencyShutdown.Business.Interfaces;

/// <summary>
/// Interface for server data for servers to shut down
/// </summary>
public interface IServer
{
    /// <summary>
    /// Name or IP address of the server
    /// </summary>
    string? ServerNameOrIp { get; set; }
        
    /// <summary>
    /// Username to access the server for shutdown
    /// </summary>
    string? UserName { get; set; }

    /// <summary>
    /// Password to access the server for shutdown
    /// </summary>
    string? Password { get; set; }

    /// <summary>
    /// Maximum time in seconds to wait until server has been shutdown
    /// </summary>
    int Timeout { get; set; }
}