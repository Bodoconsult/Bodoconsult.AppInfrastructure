// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

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
    /// <param name="visual">Current visual</param>
    public static void AllDescendantsOfType<T>(List<T> result, Visual visual) where T : class
    {
        var visualChildren = visual.GetVisualDescendants().ToList();
        var visualChildrenCount = visualChildren.LongCount();

        for (var i = 0; i < visualChildrenCount; i++)
        {
            var child = visualChildren[i];

            if (child is T item)
            {
                result.Add(item);
                continue;
            }

            AllDescendantsOfType(result, child);
        }
    }
}