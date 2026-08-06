// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Configuration details for the data context
/// </summary>
public interface IContextConfig
{
    /// <summary>
    /// Current connection string
    /// </summary>
    string? ConnectionString { get; set; }

    /// <summary>
    /// Turn off migrations. Default: false.
    /// </summary>
    bool TurnOffMigrations { get; set; }

    /// <summary>
    /// Turn off data converters running after migrations (only for etsting purposes. NOT FOR PRODUCTION)
    /// </summary>
    bool TurnOffConverters { get; set; }

    /// <summary>
    /// Timeout for database commands in seconds
    /// </summary>
    int CommandTimeout { get; set; }

    /// <summary>
    /// Turn off data backup running before migrations (only for testing purposes. NOT FOR PRODUCTION)
    /// </summary>
    bool TurnOffBackup { get; set; }

}