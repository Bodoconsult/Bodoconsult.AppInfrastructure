// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bodoconsult.App.Avalonia.ViewModels;

/// <summary>
/// Viewmodel for a info dialog
/// </summary>
public partial class InfoDialogViewModel : ObservableObject
{
    private readonly Window _dialog;

    /// <summary>
    /// Message to show
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Window title for the dialog
    /// </summary>
    public string Title { get;  }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dialog">Dialog window</param>
    /// <param name="message">Dialog message to show</param>
    /// <param name="title">Title. Default: Info</param>
    public InfoDialogViewModel(Window dialog, string message, string title= "Info")
    {
        _dialog = dialog;
        Message = message;
        Title = title;
    }

    /// <summary>
    /// Close command
    /// </summary>
    [RelayCommand]
    private void Confirm() => _dialog.Close(true);

    //[RelayCommand]
    //private void Cancel() => _dialog.Close(false);
}