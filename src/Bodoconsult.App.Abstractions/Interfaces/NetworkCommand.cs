// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Data structure for sending or receiving commands from the network
/// </summary>
public class NetworkCommand
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="command">Current command</param>
    public NetworkCommand(string command)
    {
        Command = command;
    }

    /// <summary>
    /// Current command
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// Current command parameters
    /// </summary>
    public List<NetworkCommandParameter> Parameters { get; } = new();
}