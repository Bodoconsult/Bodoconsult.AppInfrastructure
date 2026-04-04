// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;

namespace Bodoconsult.App.ReactiveUI.Tests.TestData;

/// <summary>
/// Dummy implementation of <see cref="UiMenuBuilderBase"/> for testing
/// </summary>
public class DummyUiMenuBuilder : UiMenuBuilderBase
{
    /// <summary>
    /// Number of commands built
    /// </summary>
    public int NumberOfCommands { get; private set; }

    /// <summary>
    /// Number of groups built
    /// </summary>
    public int NumberOfGroups { get; private set; }

    /// <summary>
    /// Number of separators built
    /// </summary>
    public int NumberOfSeparators { get; private set; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="translationService">Current translation service</param>
    public DummyUiMenuBuilder(II18N translationService) : base(translationService)
    {
    }

    /// <summary>
    /// Build the final object for a <see cref="CommandUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public override void BuildCommandUiMenuItem(CommandUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        NumberOfCommands++;
    }

    /// <summary>
    /// Build the final object for a <see cref="GroupUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public override void BuildGroupUiMenuItem(GroupUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        NumberOfGroups++;
    }

    /// <summary>
    /// Build the final object for a <see cref="SeparatorUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public override void BuildSeparatorUiMenuItem(SeparatorUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        NumberOfSeparators++;
    }
}