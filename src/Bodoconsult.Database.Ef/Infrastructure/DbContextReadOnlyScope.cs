// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

/* 
 * Copyright (C) 2014 Mehdi El Gueddari
 * http://mehdi.me
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */

using System.Data;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Database.Ef.Enums;
using Bodoconsult.Database.Ef.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bodoconsult.Database.Ef.Infrastructure
{
    /// <summary>
    /// Readonly context scope
    /// </summary>
    /// <typeparam name="T">Type representing the database</typeparam>
    public class DbContextReadOnlyScope<T> : IDbContextReadOnlyScope<T> where T : DbContext
    {
        private readonly DbContextScope<T> _internalScope;

        /// <summary>
        /// The DbContext instances that this DbContextScope manages.
        /// </summary>
        public IDbContextCollection<T> DbContexts => _internalScope.DbContexts;

        /// <summary>
        /// Current context config
        /// </summary>
        public IContextConfig ContextConfig { get; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public DbContextReadOnlyScope()
            : this(joiningOption: DbContextScopeOption.JoinExisting, isolationLevel: null, dbContextFactory: null)
        { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dbContextFactory">Current context factory</param>
        public DbContextReadOnlyScope(IDbContextWithConfigFactory<T> dbContextFactory)
            : this(joiningOption: DbContextScopeOption.JoinExisting, isolationLevel: null, dbContextFactory: dbContextFactory)
        { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="isolationLevel">Requested isolation level</param>
        public DbContextReadOnlyScope(IsolationLevel isolationLevel)
            : this(joiningOption: DbContextScopeOption.ForceCreateNew, isolationLevel: isolationLevel, dbContextFactory: null)
        { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="isolationLevel">Requested isolation level</param>
        /// <param name="dbContextFactory">Current context factory</param>
        public DbContextReadOnlyScope(IsolationLevel isolationLevel, IDbContextWithConfigFactory<T> dbContextFactory)
            : this(joiningOption: DbContextScopeOption.ForceCreateNew, isolationLevel: isolationLevel, dbContextFactory: dbContextFactory)
        { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="joiningOption">Scope joing option</param>
        /// <param name="isolationLevel">Requested isolation level</param>
        /// <param name="dbContextFactory">Current context factory</param>
        public DbContextReadOnlyScope(DbContextScopeOption joiningOption, IsolationLevel? isolationLevel, IDbContextWithConfigFactory<T> dbContextFactory)
        {
            ArgumentNullException.ThrowIfNull(dbContextFactory);
            
            ContextConfig = dbContextFactory.AppGlobals.ContextConfig;
            _internalScope = new DbContextScope<T>(joiningOption: joiningOption, readOnly: true, isolationLevel: isolationLevel, dbContextFactory: dbContextFactory);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dtor
        /// </summary>
        ~DbContextReadOnlyScope()
        {
            Dispose(false);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }


            try
            {
                _internalScope.Dispose();
            }
            catch //(Exception e)
            {
                // ignored
            }


        }

    }
}