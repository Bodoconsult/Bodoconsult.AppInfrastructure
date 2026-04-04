// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Models;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bodoconsult.App.Wpf.Helpers;

/// <summary>
/// General utilities for WPF
/// </summary>
public static class WpfHelper
{
    // https://learn.microsoft.com/en-us/answers/questions/746124/how-to-disable-and-enable-window-close-button-(x)

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll")]
    private static extern bool EnableMenuItem(IntPtr hMenu, uint uIdEnableItem, uint uEnable);

    private const int GwlStyle = -16;
    private const int WsMaximizebox = 0x10000; //maximize button
    private const int WsMinimizebox = 0x20000; //minimize button
    private const uint MfGrayed = 0x00000001;
    private const uint MfEnabled = 0x00000000;
    private const uint ScClose = 0xF060;

    /// <summary>
    /// Disable minimize button
    /// </summary>
    /// <param name="window">Window to disable the minimize button for</param>
    /// <exception cref="InvalidOperationException">The window has not yet been completely initialized</exception>
    public static void DisableMinimizeButton(Window window)
    {
        var windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("The window has not yet been completely initialized");
        }

        SetWindowLong(windowHandle, GwlStyle, GetWindowLong(windowHandle, GwlStyle) & ~WsMinimizebox);
    }

    /// <summary>
    /// Disable maximize button
    /// </summary>
    /// <param name="window">Window to disable the minimize button for</param>
    /// <exception cref="InvalidOperationException">The window has not yet been completely initialized</exception>
    public static void DisableMaximizeButton(Window window)
    {
        var windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("The window has not yet been completely initialized");
        }

        SetWindowLong(windowHandle, GwlStyle, GetWindowLong(windowHandle, GwlStyle) & ~WsMaximizebox);
    }

    /// <summary>
    /// Disable close button
    /// </summary>
    /// <param name="window">Window to disable the close button for</param>
    /// <exception cref="InvalidOperationException">The window has not yet been completely initialized</exception>
    public static void DisableCloseButton(Window window)
    {
        var windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("The window has not yet been completely initialized");
        }


        var hMenu = GetSystemMenu(windowHandle, false);
        EnableMenuItem(hMenu, ScClose, MfGrayed);
    }

    /// <summary>
    /// Enable close button
    /// </summary>
    /// <param name="window">Window to enable the close button for</param>
    /// <exception cref="InvalidOperationException">The window has not yet been completely initialized</exception>
    public static void EnableCloseButton(Window window)
    {
        var windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("The window has not yet been completely initialized");
        }

        var hMenu = GetSystemMenu(windowHandle, false);
        EnableMenuItem(hMenu, ScClose, MfEnabled);
    }

    /// <summary>
    /// List of available image formats for export of visuals
    /// </summary>
    public enum ImageFormat
    {
#pragma warning disable 1591
        Jpeg,
        Png,
        Bmp,
        Gif,
        Tif
#pragma warning restore 1591
    }

    /// <summary>
    /// Get WPF color from System.Drawing color
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color GetColor(TypoColor color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    /// <summary>
    /// Find the last element in a container
    /// </summary>
    /// <param name="element">Container element</param>
    /// <returns>Last element in the container</returns>
    public static DependencyObject FindLastContainerVisual(DependencyObject element)
    {
        while (true)
        {
            var children = VisualTreeHelper.GetChildrenCount(element) - 1;

            if (children < 0)
            {
                return element;
            }
            element = VisualTreeHelper.GetChild(element, children);
        }
    }

    /// <summary>
    /// Find a resource in a ressource dictionary
    /// </summary>
    /// <param name="ressourceName">name of the requested ressource</param>
    /// <param name="path">path to load the ressource dictionary from</param>
    /// <returns></returns>
    public static object FindResource(string ressourceName, string path)
    {
        try
        {
            var myResourceDictionary = new ResourceDictionary
            {
                Source = new Uri(path, UriKind.RelativeOrAbsolute)
            };

            return myResourceDictionary[ressourceName];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Find a resource in a ressource dictionary
    /// </summary>
    /// <typeparam name="T">Type the resource has to be converted to</typeparam>
    /// <param name="ressourceName">name of the requested ressource</param>
    /// <param name="path">path to load the ressource dictionary from</param>
    /// <returns></returns>
    public static T FindResource<T>(string ressourceName, string path)
    {
        try
        {
            var myResourceDictionary = new ResourceDictionary
            {
                Source = new Uri(path, UriKind.RelativeOrAbsolute)
            };

            return (T)myResourceDictionary[ressourceName];
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Find a resource in the current application
    /// </summary>
    /// <param name="ressourceName">name of the requested ressource</param>
    /// <returns>resource object</returns>
    public static object FindResourceCurrent(string ressourceName)
    {
        try
        {
            return Application.Current.Resources[ressourceName];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Find a resource in the current application
    /// </summary>
    /// <typeparam name="T">Type the resource has to be converted to</typeparam>
    /// <param name="ressourceName">Name of the requested ressource</param>
    /// <returns></returns>
    public static T FindResourceCurrent<T>(string ressourceName)
    {
        try
        {
            return (T)Application.Current.Resources[ressourceName];
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Dump the visual tree to debug window
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="level"></param>
    public static void DumpVisualTree(DependencyObject parent, int level)
    {
        var typeName = parent.GetType().Name;
        var name = (string)(parent.GetValue(FrameworkElement.NameProperty) ?? string.Empty);

        Trace.WriteLine($"{string.Empty.PadLeft(level)}{typeName}: {name}");

        //if (parent == null) return;

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            DumpVisualTree(child, level + 1);
        }
    }

    /// <summary>
    /// Dump logical tree to debug window
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="level"></param>
    public static void DumpLogicalTree(object parent, int level)
    {
        var typeName = parent.GetType().Name;
        string name;
        var doParent = parent as DependencyObject;
        // Not everything in the logical tree is a dependency object
        if (doParent != null)
        {
            name = (string)(doParent.GetValue(FrameworkElement.NameProperty) ?? string.Empty);
        }
        else
        {
            name = parent.ToString();
        }

        Trace.WriteLine(string.Empty.PadLeft(level) + $"{typeName}: {name}".PadLeft(level));

        if (doParent == null) return;

        foreach (var child in LogicalTreeHelper.GetChildren(doParent))
        {
            DumpLogicalTree(child, level + 1);
        }
    }

    /// <summary>
    /// Renders a visual into a memory stream
    /// </summary>
    /// <param name="visual">the visual to render</param>
    /// <param name="width">requested width for the bitmap</param>
    /// <param name="height">requested height for the bitmap</param>
    /// <param name="format">image format of the bitmap file</param>
    /// <returns>memory stream of the image file</returns>
    public static MemoryStream RenderVisualToImageStream(Visual visual, int width, int height, ImageFormat format)
    {
        var stream = new MemoryStream();

        Application.Current.Dispatcher.Invoke(() =>
        {
            BitmapEncoder encoder = format switch
            {
                ImageFormat.Jpeg => new JpegBitmapEncoder(),
                ImageFormat.Png => new PngBitmapEncoder(),
                ImageFormat.Bmp => new BmpBitmapEncoder(),
                ImageFormat.Gif => new GifBitmapEncoder(),
                ImageFormat.Tif => new TiffBitmapEncoder(),
                _ => throw new ArgumentOutOfRangeException("format", format, null)
            };

            var rtb = RenderVisualToBitmap(visual, width, height);

            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);
        });

        return stream;
    }

    /// <summary>
    /// Renders a visual into a bitmap
    /// </summary>
    /// <param name="visual">the visual to render</param>
    /// <param name="width">requested width for the bitmap</param>
    /// <param name="height">requested height for the bitmap</param>
    /// <returns></returns>
    public static RenderTargetBitmap RenderVisualToBitmap(Visual visual, int width, int height)
    {
        var rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
        rtb.Render(visual);
        return rtb;
    }

    /// <summary>
    /// Renders a canvas into a memory stream
    /// </summary>
    /// <param name="canvas">the canvas to render</param>
    /// <param name="width">requested width for the bitmap</param>
    /// <param name="height">requested height for the bitmap</param>
    /// <param name="format">image format of the bitmap file</param>
    /// <returns>memory stream of the image file</returns>
    public static MemoryStream RenderCanvasToImageStream(Canvas canvas, int width, int height, ImageFormat format)
    {
        var stream = new MemoryStream();

        Application.Current.Dispatcher.Invoke(() =>
        {
            BitmapEncoder encoder = format switch
            {
                ImageFormat.Jpeg => new JpegBitmapEncoder(),
                ImageFormat.Png => new PngBitmapEncoder(),
                ImageFormat.Bmp => new BmpBitmapEncoder(),
                ImageFormat.Gif => new GifBitmapEncoder(),
                ImageFormat.Tif => new TiffBitmapEncoder(),
                _ => throw new ArgumentOutOfRangeException("format", format, null)
            };

            // Save current canvas transform
            var transform = canvas.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            canvas.LayoutTransform = null;

            // Get the size of canvas
            var size = new Size(canvas.Width, canvas.Height);

            // Measure and arrange the surface
            // VERY IMPORTANT
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));
            canvas.UpdateLayout();

            // Create a render bitmap and push the surface to it
            var renderBitmap = new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            for (var i = 0; i < canvas.Children.Count; i++)
            {
                renderBitmap.Render(canvas.Children[i]);
            }

            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(stream);

            // Restore previously saved layout
            canvas.LayoutTransform = transform;
        });

        return stream;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="path"></param>
    /// <param name="format"></param>
    public static void RenderCanvasToFile(Canvas canvas, int width, int height, string path, ImageFormat format)
    {
        ArgumentNullException.ThrowIfNull(path);

        //Matrix transform = PresentationSource.FromVisual(canvas).CompositionTarget.TransformToDevice;
        //var stream = RenderCanvasToImageStream(canvas, (int)(width * transform.M11), (int)(height * transform.M22), format);

        var stream = RenderCanvasToImageStream(canvas, width, height, format);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var fstream = File.OpenWrite(path);
        stream.WriteTo(fstream);
        fstream.Flush();
        fstream.Close();
    }

    /// <summary>
    /// Render a canvas to a image file
    /// </summary>
    /// <param name="data">Needed data for the export of a Canvas to a file</param>
    public static void RenderCanvasToFile(VisualExportData data)
    {
        RenderCanvasToFile((Canvas)data.Visual, data.Width, data.Height, data.Path, data.ImageFormat);
    }

    /// <summary>
    /// Save an WPF element as XAML file
    /// </summary>
    /// <param name="element">Framework element providing the XAML</param>
    /// <param name="fileName">Path to the XAML to</param>
    public static void SaveElementAsXamlFile(FrameworkElement element, string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);

        var xamlString = XamlWriter.Save(element);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        using var sr = new StreamWriter(fileName, false, Encoding.UTF8);
        sr.Write(xamlString);
    }

    /// <summary>
    /// Load an WPF element from a XAML file
    /// </summary>
    /// <param name="fileName">File containing the XAML code</param>
    /// <returns></returns>
    public static object LoadElementFromXamlFile(string fileName)
    {
        var s = new FileStream(fileName, FileMode.Open);
        var erg = XamlReader.Load(s);
        s.Close();
        return erg;
    }

    /// <summary>
    /// Load a plain text file with UTF-8 coding into a string
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string LoadPlainTextFile(string path)
    {
        path = path.Replace("pack://siteOfOrigin:,,,", AppDomain.CurrentDomain.BaseDirectory).Replace("/", "\\");

        var erg = File.ReadAllText(path, Encoding.UTF8);

        return erg;
    }

    /// <summary>
    /// Print a visual
    /// </summary>
    /// <param name="visual">Visual object to print</param>
    public static void PrintVisual(Visual visual)
    {
        PrintVisual(visual, "The picture is drawn dynamically");
    }

    /// <summary>
    /// Print a visual
    /// </summary>
    /// <param name="visual">Visual object to print</param>
    /// <param name="message">Message to print on printed Visual</param>
    public static void PrintVisual(Visual visual, string message)
    {
        var dlg = new PrintDialog();
        dlg.PrintVisual(visual, message);
    }

    // http://www.hardcodet.net/2008/02/find-wpf-parent

    /// <summary>
    /// Finds a parent of a given item on the visual tree.
    /// </summary>
    /// <typeparam name="T">The type of the queried item.</typeparam>
    /// <param name="child">A direct or indirect child of the
    /// queried item.</param>
    /// <returns>The first parent item that matches the submitted
    /// type parameter. If not matching item can be found, a null
    /// reference is being returned.</returns>
    public static T TryFindParent<T>(this DependencyObject child)
        where T : DependencyObject
    {
        //get parent item
        var parentObject = GetParentObject(child);

        return parentObject switch
        {
            //we've reached the end of the tree
            null => null,
            //check if the parent matches the type we're looking for
            T parent => parent,
            //use recursion to proceed with next level
            _ => TryFindParent<T>(parentObject)
        };
    }

    /// <summary>
    /// This method is an alternative to WPF's
    /// <see cref="VisualTreeHelper.GetParent"/> method, which also
    /// supports content elements. Keep in mind that for content element,
    /// this method falls back to the logical tree of the element!
    /// </summary>
    /// <param name="child">The item to be processed.</param>
    /// <returns>The submitted item's parent, if available. Otherwise
    /// null.</returns>
    public static DependencyObject GetParentObject(this DependencyObject child)
    {
        switch (child)
        {
            case null:
                return null;
            //handle content elements separately
            case ContentElement contentElement:
                {
                    var parent = ContentOperations.GetParent(contentElement);
                    if (parent != null) return parent;

                    return contentElement is FrameworkContentElement fce ? fce.Parent : null;
                }
            //also try searching for parent in framework elements (such as DockPanel, etc)
            case FrameworkElement frameworkElement:
                {
                    var parent = frameworkElement.Parent;
                    if (parent != null) return parent;
                    break;
                }
        }

        //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
        return VisualTreeHelper.GetParent(child);
    }

    /// <summary>
    /// Find the logical childs of a certain type for an objects 
    /// </summary>
    /// <typeparam name="T">Requested type</typeparam>
    /// <param name="depObj">Object to search in</param>
    /// <returns>List with the found objects</returns>
    public static List<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        var result = new List<T>();

        FindLogicalChildrenInternal(result, depObj);

        return result;
    }

    /// <summary>
    /// Find the logical childs of a certain type for an objects 
    /// </summary>
    /// <typeparam name="T">Requested type</typeparam>
    /// <param name="result">List with the found objects</param>
    /// <param name="depObj">Object to search in</param>
    public static void FindLogicalChildrenInternal<T>(List<T> result, DependencyObject depObj) where T : DependencyObject
    {
        if (depObj == null)
        {
            return;
        }

        var childs = LogicalTreeHelper.GetChildren(depObj);

        foreach (var child in childs)
        {
            switch (child)
            {
                case T dependencyObject:
                    result.Add(dependencyObject);
                    break;
                case DependencyObject depChild:
                    FindLogicalChildrenInternal(result, depChild);
                    break;
            }
        }
    }

    /// <summary>
    /// Find the visual childs of a certain type for an objects 
    /// </summary>
    /// <typeparam name="T">Requested type</typeparam>
    /// <param name="depObj">Object to search in</param>
    /// <returns>List with the found objects</returns>
    public static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        var result = new List<T>();

        FindVisualChildrenInternal(result, depObj);

        return result;
    }

    /// <summary>
    /// Find the visual childs of a certain type for an objects 
    /// </summary>
    /// <typeparam name="T">Requested type</typeparam>
    /// <param name="result">List with the found objects</param>
    /// <param name="depObj">Object to search in</param>
    public static void FindVisualChildrenInternal<T>(List<T> result, DependencyObject depObj) where T : DependencyObject
    {
        if (depObj == null)
        {
            return;
        }

        var childCount = VisualTreeHelper.GetChildrenCount(depObj);

        for (var i = 0; i < childCount; i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);
            if (child is T dependencyObject)
            {
                result.Add(dependencyObject);
            }
            else
            {
                FindVisualChildrenInternal(result, child);
            }
        }
    }

    /// <summary>
    /// Get a <see cref="BitmapImage"/> from a byte array
    /// </summary>
    /// <param name="bytes">Byte array with the image data</param>
    /// <returns><see cref="BitmapImage"/></returns>
    public static BitmapImage GetBitmapImageFromBytes(ReadOnlySpan<byte> bytes)
    {
        using var ms = new MemoryStream(bytes.ToArray());
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.StreamSource = ms;

        // To save significant application memory, set the DecodePixelWidth or
        // DecodePixelHeight of the BitmapImage value of the image source to the desired
        // height or width of the rendered image. If you don't do this, the application will
        // cache the image as though it were rendered as its normal size rather than just
        // the size that is displayed.
        // Note: In order to preserve aspect ratio, set DecodePixelWidth
        // or DecodePixelHeight but not both.
        bitmap.DecodePixelHeight = 300;
        // Below code for caching is crucial.
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();
        bitmap.Freeze();
        return bitmap;
    }

    /// <summary>
    /// Get a <see cref="BitmapImage"/> from a byte array
    /// </summary>
    /// <param name="bytes">Byte array with the image data</param>
    /// <returns><see cref="BitmapImage"/></returns>
    public static BitmapImage GetBitmapImageFromBytes(byte[] bytes)
    {
        using var ms = new MemoryStream(bytes.ToArray());
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.StreamSource = ms;

        // To save significant application memory, set the DecodePixelWidth or
        // DecodePixelHeight of the BitmapImage value of the image source to the desired
        // height or width of the rendered image. If you don't do this, the application will
        // cache the image as though it were rendered as its normal size rather than just
        // the size that is displayed.
        // Note: In order to preserve aspect ratio, set DecodePixelWidth
        // or DecodePixelHeight but not both.
        bitmap.DecodePixelHeight = 300;
        // Below code for caching is crucial.
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();
        bitmap.Freeze();
        return bitmap;
    }
}