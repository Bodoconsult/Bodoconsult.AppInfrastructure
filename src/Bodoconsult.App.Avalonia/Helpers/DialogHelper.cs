// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Bodoconsult.App.Avalonia.Interfaces;
using Bodoconsult.App.Avalonia.ViewModels;
using Bodoconsult.App.Avalonia.Views;

namespace Bodoconsult.App.Avalonia.Helpers;

/// <summary>
/// Helper class for dialogs
/// </summary>
public static class DialogHelper
{
    /// <summary>
    /// Show an info dialog
    /// </summary>
    /// <param name="parent">Parent window</param>
    /// <param name="message">Message to show as info for the user</param>
    /// <returns></returns>
    public static async Task<bool?> ShowInfoDialog(Window parent, string message)
    {
        var dialog = new InfoDialog();
        var vm = new InfoDialogViewModel(dialog, "Delete this item?");
        dialog.DataContext = vm;

        return await dialog.ShowDialog<bool?>(parent);
    }
}