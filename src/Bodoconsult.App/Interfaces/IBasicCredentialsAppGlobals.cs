// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Interfaces;

/// <summary>
/// App globals with basic credentials
/// </summary>
public interface IWebCredentialsAppGlobals: IAppGlobals
{
    /// <summary>
    /// Basic credentials
    /// </summary>
    WebCredentials? Credentials { get; set; }
}