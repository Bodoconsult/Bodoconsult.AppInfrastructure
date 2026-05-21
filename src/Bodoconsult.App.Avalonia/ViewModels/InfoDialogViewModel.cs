// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodoconsult.App.Avalonia.ViewModels
{
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
        /// Default ctor
        /// </summary>
        /// <param name="dialog">Dialog window</param>
        /// <param name="message">Dialog message to show</param>
        public InfoDialogViewModel(Window dialog, string message)
        {
            _dialog = dialog;
            Message = message;
        }

        /// <summary>
        /// Close command
        /// </summary>
        [RelayCommand]
        private void Confirm() => _dialog.Close(true);

        //[RelayCommand]
        //private void Cancel() => _dialog.Close(false);
    }
}
