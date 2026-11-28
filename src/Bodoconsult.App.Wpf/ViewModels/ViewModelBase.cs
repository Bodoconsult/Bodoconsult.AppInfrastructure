// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Bodoconsult.App.Wpf.ViewModels;

/// <summary>
/// Base class for view models
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
    /// <summary>Occurs when a property value changes</summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// On property changed
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void OnPropertyChanged(string propertyName = null)
    {
        var x = PropertyChanged;

        x?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private static bool? _isInDesignMode;

    /// <summary>
    /// Gets a value indicating whether the control is in design mode (running in Blend
    /// or Visual Studio).
    /// </summary>
    public static bool IsInDesignModeStatic
    {
        get
        {
            if (!_isInDesignMode.HasValue)
            {
#if SILVERLIGHT
            _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                var prop = DesignerProperties.IsInDesignModeProperty;
                _isInDesignMode
                    = (bool)DependencyPropertyDescriptor
                    .FromProperty(prop, typeof(FrameworkElement))
                    .Metadata.DefaultValue;
#endif
            }

            return _isInDesignMode.Value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the control is in design mode (running under Blend
    /// or Visual Studio).
    /// </summary>
    [SuppressMessage(
        "Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "Non static member needed for data binding")]
    public bool IsInDesignMode => IsInDesignModeStatic;
}
