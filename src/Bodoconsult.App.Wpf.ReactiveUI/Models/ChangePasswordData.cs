// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Wpf.ReactiveUI.Delegates;
using System.Windows.Media;
using Bodoconsult.App.Wpf.ReactiveUI.Helper;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.Wpf.ReactiveUI.Models
{
    /// <summary>
    /// Contains all data needed to fill and handle a ChangePasswordWindow
    /// </summary>

    public partial class ChangePasswordData: ReactiveObject
    {
        /// <summary>
        /// Default ctor with translation delegate
        /// </summary>
        /// <param name="translateDelegate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ChangePasswordData(TranslateDelegate translateDelegate)
        {
            if (translateDelegate == null)
            {
                throw new ArgumentNullException(nameof(translateDelegate));
            }

            _titleLabel = translateDelegate.Invoke("Wpf.Base.ChangePasswordDialogTitle");
            _newPasswordRepeatLabel = translateDelegate.Invoke("Wpf.Base.PasswordRepeatLabelText");
            _newPasswordRepeatTooltip = translateDelegate.Invoke("Wpf.Base.PasswordRepeatTooltipText");
            _passwordLabel = translateDelegate.Invoke("Wpf.Base.PasswordLabelText");
            _passwordTooltip = translateDelegate.Invoke("Wpf.Base.PasswordTooltipText");
            _cancelButtonLabel = translateDelegate.Invoke("Wpf.Base.CancelButtonText");
            _changePasswordButtonLabel = translateDelegate.Invoke("Wpf.Base.ChangePasswordButtonText");
            _background = ResourceFinder.FindResource<Brush>("HighlightBrush");
           _newPasswordLabel = translateDelegate.Invoke("Wpf.Base.NewPasswordLabelText");
            _newPasswordTooltip = translateDelegate.Invoke("Wpf.Base.NewPasswordTooltipText");
        }


        #region Tooltip properties

        /// <summary>
        /// Password tooltip
        /// </summary>
        [Reactive] private string _passwordTooltip;

        /// <summary>
        /// New password repeat tooltip
        /// </summary>
        [Reactive] private string _newPasswordRepeatTooltip;

        /// <summary>
        /// New password tooltip
        /// </summary>
        [Reactive] private string _newPasswordTooltip;

        #endregion

        #region Label properties

        /// <summary>
        /// Title label
        /// </summary>
        [Reactive] private string _titleLabel;

        /// <summary>
        /// Password label
        /// </summary>
        [Reactive] private string _passwordLabel;

        /// <summary>
        /// New password repeat label
        /// </summary>
        [Reactive] private string _newPasswordRepeatLabel;

        /// <summary>
        /// New password label
        /// </summary>
        [Reactive] private string _newPasswordLabel;

        /// <summary>
        /// Cancel button label
        /// </summary>
        [Reactive] private string _cancelButtonLabel;

        /// <summary>
        /// Chnage password button label
        /// </summary>
        [Reactive] private string _changePasswordButtonLabel;
        #endregion


        #region Delegate variables

        /// <summary>
        /// Delegate for checking the change password
        /// </summary>
        [Reactive] private LoginChangePasswordDataDelegate _checkChangePasswordData;

        #endregion

        #region Layout properties

        /// <summary>
        /// Background brush
        /// </summary>
        [Reactive] private Brush _background;

        #endregion

    }
}