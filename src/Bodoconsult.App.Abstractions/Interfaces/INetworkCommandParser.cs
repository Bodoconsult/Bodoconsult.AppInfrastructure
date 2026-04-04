// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for parsing a command string to a <see cref="NetworkCommand"/> instance
/// </summary>
public interface INetworkCommandParser
{
    /// <summary>
    /// Parse the command string to a <see cref="NetworkCommand"/> instance
    /// </summary>
    /// <param name="commandString">Command string to parse</param>
    /// <returns><see cref="NetworkCommand"/> instance with the parsed command string</returns>
    public NetworkCommand Parse(string commandString);
}