// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace BaseTests;

/// <summary>
/// Credentials to be used for unit testing
/// </summary>
public class TestCredentials
{
    /// <summary>
    /// Admin credentials
    /// </summary>
    public AdCredentials? AdminCredentials { get; set; }
}