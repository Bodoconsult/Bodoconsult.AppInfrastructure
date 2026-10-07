// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Bodoconsult.Database.Ef.MigrationTools.Operations;

/// <summary>
/// Deny permissions on tables, views, stored procedures or functions
/// </summary>
public class DenyPermissionOperation : MigrationOperation
{



    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="databaseObject">Table, view, stored procedure or function name including schema</param>
    /// <param name="userOrRole">User or role name</param>
    /// <param name="permission">Permission to set</param>
    public DenyPermissionOperation(string databaseObject, string userOrRole, DatabasePermission permission)
    {
        DatabaseObject = databaseObject;
        UserOrRole = userOrRole;
        Permission = permission;
    }

    /// <summary>
    /// Name of the database object
    /// </summary>
    public string DatabaseObject { get; }

    /// <summary>
    /// Name of the user role
    /// </summary>
    public string UserOrRole { get; }

    /// <summary>
    /// Requested permission
    /// </summary>
    public DatabasePermission Permission { get; }

    /// <summary>
    ///     Indicates whether or not the operation might result in loss of data in the database.
    /// </summary>
    public override bool IsDestructiveChange => false;
}