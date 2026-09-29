// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection.ConsoleTools;

namespace Bodoconsult.App.DataProtection.Interfaces;

/// <summary>
/// App globals with basic credentials
/// </summary>
public interface IBasicCredentialsAppGlobals: IAppGlobals
{
    /// <summary>
    /// Basic credentials
    /// </summary>
    BasicCredentials? Credentials { get; set; }
}