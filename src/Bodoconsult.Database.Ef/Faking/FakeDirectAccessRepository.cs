// Copyright (c) Mycronic. All rights reserved.

using Bodoconsult.Database.Ef.Interfaces;

namespace Bodoconsult.Database.Ef.Faking
{
    /// <summary>
    /// Fake implemenation of <see cref="IDirectAccessRepository"/>
    /// </summary>
    public class FakeDirectAccessRepository : IDirectAccessRepository
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="contextLocator">Current context locator</param>
        public FakeDirectAccessRepository(IAmbientDbContextLocator contextLocator)
        {
            ContextLocator  = contextLocator;
        }

        /// <summary>
        /// Current context locator
        /// </summary>
        public IAmbientDbContextLocator ContextLocator { get; set; }
    }
}