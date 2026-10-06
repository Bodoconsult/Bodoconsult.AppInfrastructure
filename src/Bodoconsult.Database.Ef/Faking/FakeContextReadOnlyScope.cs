// Copyright (c) Mycronic. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Database.Ef.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bodoconsult.Database.Ef.Faking
{
    /// <summary>
    /// Read only context scope for faking
    /// </summary>
    public class FakeContextReadOnlyScope<T> :  IDbContextReadOnlyScope<T> where T : DbContext
    {
        /// <summary>
        /// Current context config
        /// </summary>
        public IContextConfig ContextConfig { get; }

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="config">Current config</param>
        public FakeContextReadOnlyScope(IContextConfig config)
        {
            ContextConfig = config;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ///// <summary>
        ///// Dtor
        ///// </summary>
        //~FakeContextReadOnlyScope()
        //{
        //    Dispose(false);
        //}

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
        }

        /// <summary>
        /// The DbContext instances that this DbContextScope manages.
        /// </summary>
        public IDbContextCollection<T> DbContexts { get; set; }
    }
}