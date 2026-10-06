// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Database.Ef.Infrastructure;

/// <summary>
/// Current context config
/// </summary>
public class ContextConfig : IContextConfig
{
    /// <summary>
    /// Current connection string
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Turn off migrations. Default: false.
    /// </summary>
    public bool TurnOffMigrations { get; set; }

    /// <summary>
    /// Turn off data converters running after migrations (only for etsting purposes. NOT FOR PRODUCTION)
    /// </summary>
    public bool TurnOffConverters { get; set; }

    /// <summary>
    /// Timeout for database commands in seconds
    /// </summary>
    public int CommandTimeout { get; set; } = 600;

    /// <summary>
    /// Turn off data backup running before migrations (only for testing purposes. NOT FOR PRODUCTION)
    /// </summary>
    public bool TurnOffBackup { get; set; }
}