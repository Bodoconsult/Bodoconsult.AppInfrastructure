// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Network command parameter
/// </summary>
public struct NetworkCommandParameter
{
    /// <summary>
    /// Parameter name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Parameter value
    /// </summary>
    public string Value { get; set; }
}