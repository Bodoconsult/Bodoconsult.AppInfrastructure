// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Database.Ef.MigrationTools.Operations
{
    /// <summary>
    /// Permissions to set on tables, views, stroed procs or functions
    /// </summary>
    public enum DatabasePermission
    {
        /// <summary>
        /// SELECT permission
        /// </summary>
        Select,
        /// <summary>
        /// UPDATE permission
        /// </summary>
        Update,
        /// <summary>
        /// DELETE permission
        /// </summary>
        Delete,
        /// <summary>
        /// INSERT permission
        /// </summary>
        Insert,
        /// <summary>
        /// EXECUTE permission
        /// </summary>
        Execute
    }
}