// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for building a <see cref="NetworkCommand"/> instance
/// </summary>
public interface INetworkCommandBuilder
{
    /// <summary>
    /// Build a <see cref="NetworkCommand"/> instance from a command string and a list of parameters
    /// </summary>
    /// <param name="command">Command string</param>
    /// <param name="parameters">Dictionary with parameter anem and parameter value</param>
    /// <returns><see cref="NetworkCommand"/> instance with the parsed command string</returns>
    public string BuildIt(string command, List<NetworkCommandParameter> parameters);
}