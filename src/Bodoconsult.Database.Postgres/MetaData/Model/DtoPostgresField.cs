using System;

namespace Bodoconsult.Database.Postgres.MetaData.Model;

/// <summary>
/// A DTO representing a Postgres field meta info
/// </summary>
public class DtoPostgresField
{
    /// <summary>
    /// Name of the database table field name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Database data type of the field
    /// </summary>
    public Type DatabaseType { get; set; }

    /// <summary>
    /// Max length for string fields
    /// </summary>
    public int MaxLength { get; set; }

    ///// <summary>
    ///// Auto incremented value
    ///// </summary>
    //public bool AutoWert { get; set; }
}