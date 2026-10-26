// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using EmergencyShutdown.Business.Interfaces;

namespace EmergencyShutdown.Business.Servers;

/// <summary>
/// base class for server data for servers to shut down
/// </summary>
public abstract class BaseServer : IServer
{
    ///// <summary>
    ///// default ctor
    ///// </summary>
    //public Server()
    //{
    //    Timeout = 60;
    //    ServerType = ServerType.Windows;
    //}

    /// <summary>
    /// Name or IP address of the server
    /// </summary>
    public string? ServerNameOrIp { get; set; }

    /// <summary>
    /// Username to access the server for shutdown
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Password to access the server for shutdown
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Maximum time in seconds to wait until server has been shutdown. Default 30s
    /// </summary>
    public int Timeout { get; set; } = 30;
}