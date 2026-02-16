// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Wpf.ReactiveUI.Controls;

/// <summary>
/// WPF default menu user control
/// </summary>
public partial class ContextMenuControl: UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Menu"/> class.
    /// Sets menu alignment on initialization.
    /// </summary>
    public ContextMenuControl()
    {
        Initialize();
        DataContext = new ContextMenuControlViewModel();
    }

    private static void Initialize()
    {
        if (!SystemParameters.MenuDropAlignment)
        {
            return;
        }

        var fieldInfo = typeof(SystemParameters).GetField(
            "_menuDropAlignment",
            BindingFlags.NonPublic | BindingFlags.Static);
        fieldInfo?.SetValue(null, false);
    }
}