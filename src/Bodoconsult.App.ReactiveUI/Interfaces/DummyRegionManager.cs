// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Dummy implementation of <see cref="RegionManagerBase"/> for testing
/// </summary>
public class DummyRegionManager : RegionManagerBase
{
    /// <summary>
    /// Find the regions for an existing window instance
    /// </summary>
    /// <param name="window">Window</param>
    /// <param name="wwd">UI window definition</param>
    public override void FindRegions(IUiWindow window, UiWindowDefinition wwd)
    {
        // Do nothing
    }
}