// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Bodoconsult.Database.Ef.MigrationTools.Operations
{
    /// <summary>
    /// Turn database based creation of new values for a primary key column on or off
    /// </summary>
    /// <remarks>Turns Identity on/off for a int primary key column</remarks>
    public class ChangeIdentityOperation : MigrationOperation
    {
        ///// <summary>
        ///// Default ctor
        ///// </summary>
        //public ChangeIdentityOperation()
        //{
        //    DependentColumns = new List<DependentColumn>();
        //}

        /// <summary>
        /// Requested way of identity change
        /// </summary>
        public IdentityChange Change { get; set; }

        /// <summary>
        /// Proncipal table name
        /// </summary>
        public string PrincipalTable { get; set; }

        /// <summary>
        /// Principal column name
        /// </summary>
        public string PrincipalColumn { get; set; }

        /// <summary>
        /// Dependent columns
        /// </summary>
        public List<DependentColumn> DependentColumns { get; } = [];

        /// <summary>
        ///     Indicates whether or not the operation might result in loss of data in the database.
        /// </summary>
        public override bool IsDestructiveChange => false;

        #region Helper objects and enums

        /// <summary>
        /// Identity change enum
        /// </summary>
        public enum IdentityChange
        {
            /// <summary>
            /// Switch identity change on
            /// </summary>
            SwitchIdentityOn,
            /// <summary>
            /// Switch identity change off
            /// </summary>
            SwitchIdentityOff
        }

        #endregion
    }
}