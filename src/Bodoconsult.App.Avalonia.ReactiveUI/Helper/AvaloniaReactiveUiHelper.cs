// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Helper;

/// <summary>
/// Helper class for Avalonia issues tied to ReactiveUi
/// </summary>
public static class AvaloniaReactiveUiHelper
{
    /// <summary>
    /// Find a descendant for a window
    /// </summary>
    /// <typeparam name="T">Type of the requested descendant</typeparam>
    /// <param name="window"></param>
    /// <returns></returns>
    public static List<T> FindChildren<T>(Window window) where T : class
    {
        var result = new List<T>();
        AllDescendantsOfType<T>(result, window);
        return result;
    }

    /// <summary>
    /// Find all descendants of a certain type for a visual
    /// </summary>
    /// <typeparam name="T">Type of the requested descendant</typeparam>
    /// <param name="result">List with all found descendants</param>
    /// <param name="window">Current visual</param>
    public static void AllDescendantsOfType<T>(List<T> result, Window window) where T : class
    {
        var children = window.GetLogicalDescendants().ToArray();
        var childrenCount = children.LongCount();

        for (var i = 0; i < childrenCount; i++)
        {
            var child = children[i];

            if (child is not T item)
            {
                continue;
            }
            
            result.Add(item);
        }
    }
}