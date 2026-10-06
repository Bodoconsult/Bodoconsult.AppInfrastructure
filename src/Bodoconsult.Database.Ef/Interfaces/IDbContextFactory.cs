// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bodoconsult.Database.Ef.Interfaces;

/// <summary>
/// Intefrace for DB contexts with config
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDbContextWithConfigFactory<T> : IDbContextFactory<T> where T : DbContext
{
    /// <summary>
    /// Current app globals with database settings
    /// </summary>
    IAppGlobalsWithDatabase AppGlobals { get; }
}
