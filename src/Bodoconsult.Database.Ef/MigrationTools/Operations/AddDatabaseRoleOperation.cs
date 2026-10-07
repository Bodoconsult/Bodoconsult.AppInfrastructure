// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Bodoconsult.Database.Ef.MigrationTools.Operations;

/// <summary>
/// Add a database role
/// </summary>
public class AddDatabaseRoleOperation : MigrationOperation
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="roleName">Role name</param>
    public AddDatabaseRoleOperation(string roleName)
    {
        RoleName = roleName;
    }

    /// <summary>
    /// Tole name
    /// </summary>
    public string RoleName { get; }

    /// <summary>
    ///     Indicates whether or not the operation might result in loss of data in the database.
    /// </summary>
    public override bool IsDestructiveChange => false;
}