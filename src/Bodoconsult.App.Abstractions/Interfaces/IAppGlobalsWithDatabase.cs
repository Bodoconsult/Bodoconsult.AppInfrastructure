// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for app globals for apps with database usage
/// </summary>
public interface IAppGlobalsWithDatabase: IAppGlobals
{
    /// <summary>
    /// Current database context or null
    /// </summary>
    IContextConfig? ContextConfig { get; set; }
}