// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using System.Windows.Controls;

namespace Bodoconsult.App.Wpf.Extensions;

/// <summary>
/// Extended column definition regarding visibility
/// </summary>
public class ColumnDefinitionExtended : ColumnDefinition
{
    /// <summary>
    /// Extended row definition regarding visibility
    /// </summary>
    public static DependencyProperty VisibilityProperty;

    /// <summary>
    /// Visibility
    /// </summary>
    public Visibility Visibility
    {
        get => (Visibility)GetValue(VisibilityProperty);
        set => SetValue(VisibilityProperty, value);
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    static ColumnDefinitionExtended()
    {
        VisibilityProperty = DependencyProperty.Register("Visibility",
            typeof(Visibility),
            typeof(ColumnDefinitionExtended),
            new PropertyMetadata(true, OnVisibleChanged));

        WidthProperty.OverrideMetadata(typeof(ColumnDefinitionExtended),
            new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), null,
                CoerceWidth));

        MinWidthProperty.OverrideMetadata(typeof(ColumnDefinitionExtended),
            new FrameworkPropertyMetadata((double)0, null,
                CoerceMinWidth));
    }

    /// <summary>
    /// Set visibility
    /// </summary>
    /// <param name="obj">Dependecy object</param>
    /// <param name="isVisible">Is visible? True or false</param>
    public static void SetVisible(DependencyObject obj, bool isVisible)
    {
        obj.SetValue(VisibilityProperty, isVisible);
    }

    /// <summary>
    /// Get visibility
    /// </summary>
    /// <param name="obj">Dependecy object</param>
    /// <returns>Is visible? True or false</returns>
    public static Visibility GetVisible(DependencyObject obj)
    {
        return (Visibility)obj.GetValue(VisibilityProperty);
    }

    private static void OnVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        obj.CoerceValue(WidthProperty);
        obj.CoerceValue(MinWidthProperty);
    }

    /// <summary>
    /// Coerce width
    /// </summary>
    /// <param name="obj">Dependecy object</param>
    /// <param name="nValue">Height</param>
    /// <returns>ColumnDefinitionExtended instance</returns>
    public static object CoerceWidth(DependencyObject obj, object nValue)
    {
        return ((ColumnDefinitionExtended)obj).Visibility == Visibility.Visible ? nValue : new GridLength(0);
    }

    /// <summary>
    /// Coerce minimum width
    /// </summary>
    /// <param name="obj">Dependecy object</param>
    /// <param name="nValue">Height</param>
    /// <returns>ColumnDefinitionExtended instance</returns>
    public static object CoerceMinWidth(DependencyObject obj, object nValue)
    {
        return ((ColumnDefinitionExtended)obj).Visibility == Visibility.Visible ? nValue : (double)0;
    }
}