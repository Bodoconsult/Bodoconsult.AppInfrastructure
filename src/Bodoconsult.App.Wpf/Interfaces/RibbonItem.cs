// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Bodoconsult.App.Wpf.Interfaces;

/// <summary>
/// Implements an simple ribbon button
/// </summary>
public class RibbonItem: INotifyPropertyChanged
{
    private string _header;
    private string _smallImagePath;
    private string _largeImagePath;
    private ICommand _command;
    private string _tabName;
    private string _tabGroupName;

    /// <summary>
    /// Header text for the ribbon item
    /// </summary>
    public string Header
    {
        get => _header;
        set
        {
            if (value == _header)
            {
                return;
            }
            SetField(ref _header, value);
        }
    }

    /// <summary>
    /// Path to a small 16x16 icon
    /// </summary>
    public string SmallImagePath
    {
        get => _smallImagePath;
        set
        {
            if (value == _smallImagePath)
            {
                return;
            }
            SetField(ref _smallImagePath, value);
            //OnPropertyChanged();
        }
    }

    /// <summary>
    /// Path to a large 32x32 icon
    /// </summary>
    public string LargeImagePath
    {
        get => _largeImagePath;
        set
        {
            if (value == _largeImagePath)
            {
                return;
            }
            SetField(ref _largeImagePath, value);
            //OnPropertyChanged();
        }
    }

    /// <summary>
    /// Command related to the ribbon item
    /// </summary>
    public ICommand Command
    {
        get => _command;
        set
        {
            if (Equals(value, _command))
            {
                return;
            }
            SetField(ref _command, value);
            //OnPropertyChanged();
        }
    }

    /// <summary>
    /// Name of the tab the ribbon item will be placed. Reserved names are QuickAccess and ApplicationMenu
    /// </summary>
    public string TabName
    {
        get => _tabName;
        set
        {
            if (value == _tabName)
            {
                return;
            }
            SetField(ref _tabName, value);
            //OnPropertyChanged();
        }
    }

    /// <summary>
    /// Name of the group the ribbon item will be placed in the tab. Reserved names are QuickAccess and ApplicationMenu
    /// </summary>
    public string TabGroupName
    {
        get => _tabGroupName;
        set
        {
            if (value == _tabGroupName)
            {
                return;
            }
            SetField(ref _tabGroupName, value);
            //OnPropertyChanged();
        }
    }

    /// <summary>Occurs when a property value changes.</summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// On property changed event
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Set field value
    /// </summary>
    /// <param name="field">Field</param>
    /// <param name="value">Value to set</param>
    /// <param name="propertyName">Property name</param>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>True if the value was set else false</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}