//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System;
//using Bodoconsult.App.Abstractions.Delegates;
//using Bodoconsult.App.Avalonia.ReactiveUI.Delegates;
//using System.Windows.Media;
//using Bodoconsult.App.Avalonia.ReactiveUI.Helper;
//using ReactiveUI;
//using ReactiveUI.SourceGenerators;

//namespace Bodoconsult.App.Avalonia.ReactiveUI.Models;

///// <summary>
///// Contains all data needed to fill and handle a ChangePasswordWindow
///// </summary>
//public partial class ChangePasswordData: ReactiveObject
//{
//    /// <summary>
//    /// Default ctor with translation delegate
//    /// </summary>
//    /// <param name="translateDelegate"></param>
//    /// <exception cref="ArgumentNullException"></exception>
//    public ChangePasswordData(TranslateDelegate translateDelegate)
//    {
//        if (translateDelegate == null)
//        {
//            throw new ArgumentNullException(nameof(translateDelegate));
//        }

//        _titleLabel = translateDelegate.Invoke("Avalonia.Base.ChangePasswordDialogTitle");
//        _newPasswordRepeatLabel = translateDelegate.Invoke("Avalonia.Base.PasswordRepeatLabelText");
//        _newPasswordRepeatTooltip = translateDelegate.Invoke("Avalonia.Base.PasswordRepeatTooltipText");
//        _passwordLabel = translateDelegate.Invoke("Avalonia.Base.PasswordLabelText");
//        _passwordTooltip = translateDelegate.Invoke("Avalonia.Base.PasswordTooltipText");
//        _cancelButtonLabel = translateDelegate.Invoke("Avalonia.Base.CancelButtonText");
//        _changePasswordButtonLabel = translateDelegate.Invoke("Avalonia.Base.ChangePasswordButtonText");
//        _background = ResourceFinder.FindResource<Brush>("HighlightBrush");
//        _newPasswordLabel = translateDelegate.Invoke("Avalonia.Base.NewPasswordLabelText");
//        _newPasswordTooltip = translateDelegate.Invoke("Avalonia.Base.NewPasswordTooltipText");
//    }


//    #region Tooltip properties

//    /// <summary>
//    /// Password tooltip
//    /// </summary>
//    [Reactive] private string _passwordTooltip;

//    /// <summary>
//    /// New password repeat tooltip
//    /// </summary>
//    [Reactive] private string _newPasswordRepeatTooltip;

//    /// <summary>
//    /// New password tooltip
//    /// </summary>
//    [Reactive] private string _newPasswordTooltip;

//    #endregion

//    #region Label properties

//    /// <summary>
//    /// Title label
//    /// </summary>
//    [Reactive] private string _titleLabel;

//    /// <summary>
//    /// Password label
//    /// </summary>
//    [Reactive] private string _passwordLabel;

//    /// <summary>
//    /// New password repeat label
//    /// </summary>
//    [Reactive] private string _newPasswordRepeatLabel;

//    /// <summary>
//    /// New password label
//    /// </summary>
//    [Reactive] private string _newPasswordLabel;

//    /// <summary>
//    /// Cancel button label
//    /// </summary>
//    [Reactive] private string _cancelButtonLabel;

//    /// <summary>
//    /// Chnage password button label
//    /// </summary>
//    [Reactive] private string _changePasswordButtonLabel;
//    #endregion


//    #region Delegate variables

//    /// <summary>
//    /// Delegate for checking the change password
//    /// </summary>
//    [Reactive] private LoginChangePasswordDataDelegate _checkChangePasswordData;

//    #endregion

//    #region Layout properties

//    /// <summary>
//    /// Background brush
//    /// </summary>
//    [Reactive] private Brush _background;

//    #endregion

//}