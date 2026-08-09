// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.ComponentModel;
using System.Windows;

namespace Bodoconsult.App.Wpf.Models;

/// <summary>
/// Resource dictionary which loads each resource key only once
/// </summary>
public class SharedResourceDictionary : ResourceDictionary
{
    /// <summary>
    /// Internal cache of loaded dictionaries 
    /// </summary>
    public static Dictionary<Uri, ResourceDictionary> SharedDictinaries = new();

    /// <summary>
    /// Local member of the source uri
    /// </summary>
    private Uri _sourceUri;

    private static bool IsInDesignMode =>
        (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty,
            typeof(DependencyObject)).Metadata.DefaultValue;

    /// <summary>
    /// Gets or sets the uniform resource identifier (URI) to load resources from.
    /// </summary>
    public new Uri Source
    {
        get => IsInDesignMode ? base.Source : _sourceUri;
        set
        {
            if (value is null)
            {
                return;
            }

            _sourceUri = value;

            if (IsInDesignMode)
            {
                //try
                //{
                var dict = Application.LoadComponent(_sourceUri) as ResourceDictionary;
                MergedDictionaries.Add(dict);

                //}
                //catch
                //{
                //    // ignored
                //}

                return;
            }
            
            if (!SharedDictinaries.TryGetValue(value, out var dictinary))
            {
                try
                {
                    //If the dictionary is not yet loaded, load it by setting
                    //the source of the base class
                    base.Source = _sourceUri;
                }
                catch
                {
                    //only throw exception @runtime to avoid "Exception has been 
                    //thrown by the target of an invocation."-Error@DesignTime
                    if (!IsInDesignMode)
                    {
                        throw;
                    }
                }
                SharedDictinaries.Add(value, this);
                //MergedDictionaries.Add(SharedDictinaries[value]);
            }
            else
            {
                MergedDictionaries.Add(dictinary);
            }
        }
    }
}