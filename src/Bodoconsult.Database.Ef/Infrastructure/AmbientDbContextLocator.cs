// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

/* 
 * Copyright (C) 2014 Mehdi El Gueddari
 * http://mehdi.me
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */

using Bodoconsult.Database.Ef.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bodoconsult.Database.Ef.Infrastructure
{
    /// <summary>
    /// Ambient DB context locator
    /// </summary>
    public class AmbientDbContextLocator : IAmbientDbContextLocator
    {
        /// <summary>
        /// If called within the scope of a DbContextScope, gets or creates 
        /// the ambient DbContext instance for the provided DbContext type. 
        /// 
        /// Otherwise returns null. 
        /// </summary>
        public T GetContext<T>() where T : DbContext
        {
            var ambientDbContextScope = DbContextScope<T>.GetAmbientScope();
            return ambientDbContextScope?.DbContexts.GetContext();
        }
    }
}