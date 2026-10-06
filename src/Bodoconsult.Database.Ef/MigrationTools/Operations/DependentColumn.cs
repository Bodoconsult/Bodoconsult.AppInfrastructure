// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Database.Ef.MigrationTools.Operations;

/// <summary>
/// Dependent columns data
/// </summary>
public class DependentColumn
{
    /// <summary>
    /// Dependent table name
    /// </summary>
    public string DependentTable { get; set; }

    /// <summary>
    /// Foreign key column name
    /// </summary>
    public string ForeignKeyColumn { get; set; }
}