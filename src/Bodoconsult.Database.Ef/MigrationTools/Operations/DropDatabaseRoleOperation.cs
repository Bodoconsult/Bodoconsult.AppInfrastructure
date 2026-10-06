// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Bodoconsult.Database.Ef.MigrationTools.Operations
{
    /// <summary>
    /// Drop a database role
    /// </summary>
    public class DropDatabaseRoleOperation : MigrationOperation
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="roleName">Role name</param>
        public DropDatabaseRoleOperation(string roleName)
        {
            RoleName = roleName;
        }

        /// <summary>
        /// Role name
        /// </summary>
        public string RoleName { get; }

        /// <summary>
        ///     Indicates whether or not the operation might result in loss of data in the database.
        /// </summary>
        public override bool IsDestructiveChange => false;
    }
}