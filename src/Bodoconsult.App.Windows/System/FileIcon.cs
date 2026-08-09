// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace Bodoconsult.App.Windows.System;

/// <summary>
/// Class to extract file icons from file system files
/// </summary>
[SupportedOSPlatform("windows")]
public static class FileIcon
{

    [DllImport("shell32.dll", EntryPoint = "ExtractIcon")]
    private static extern IntPtr ExtractIconA(IntPtr hInst, string lpszExeFileName, int nIconIndex);

    [DllImport("user32.dll", EntryPoint = "DestroyIcon")]
    private static extern void DestroyIcon(IntPtr hInst);

    /// <summary>
    /// Dictionary collection that contains file extensions
    /// </summary>
    private static readonly Dictionary<string, Image> Icons = new(50);

    /// <summary>
    /// Get an icon as <see cref="Image"/> for file
    /// </summary>
    /// <param name="filepath">Full path for the file to get the icon for</param>
    /// <returns><see cref="Image"/> object with the icon</returns>
    public static Image GetIcon(string filepath)
    {
        // if specified file path != null and string length > 0 
        if (filepath is null || string.IsNullOrEmpty(filepath))
        {
            return null;
        }

        //get file info of image that can be found with specified filepath
        var file = new FileInfo(filepath);
        // get file extension of image
        var extension = file.Extension.ToLower();

        //if dictionary contains specified file extension -->return extension
        if (Icons.TryGetValue(extension, out var icon))
        {
            return icon;
        }
        // if specified file extension == .dir --> get and return default folder icon
        if (extension == ".dir")
        {
            GetFolderIcon();
        }
        // if specified exttension neither exist in the dictionary nor matchs with default folder icon --> get and return default unknown file icon
        else
        {
            GetFileIcon(filepath);
        }
        return GetIcon(extension);
    }

    /// <summary>
    /// Gets and adds default folder icon to dictionary
    /// </summary>
    private static void GetFolderIcon()
    {
        // set search flag to file format=icon, to USEFILEATTRIBUTES=yes & icon size = large icons
        var flags = Shell32.ShgfiIcon | Shell32.ShgfiUsefileattributes;
        flags += Shell32.ShgfiLargeicon;

        // Get the folder icon from the file information
        var shfi = new Shell32.ShFileInfo();
        Shell32.SHGetFileInfo(null,
            Shell32.FileAttributeDirectory,
            ref shfi,
            (uint)Marshal.SizeOf(shfi),
            flags);

        Icon.FromHandle(shfi.HIcon);	// Load the icon from an HICON handle

        // Now clone the icon, so that it can be successfully stored in an ImageList
        var icon = (Icon)Icon.FromHandle(shfi.HIcon).Clone();

        DestroyIcon(shfi.HIcon);		// Cleanup
        // add default folder icon to dictionary
        Icons.Add(".dir", icon.ToBitmap());
    }

    /// <summary>
    /// Gets and adds file icon from file that is specified by filepath
    /// </summary>
    /// <param name="filepath">Full path for the file to get the icon for</param>
    private static void GetFileIcon(string filepath)
    {

        var extension = Path.GetExtension(filepath);
        if (extension is null)
        {
            return;
        }

        var regKey = Registry.ClassesRoot.OpenSubKey(extension);

        var value = regKey?.GetValue("");

        if (value is null)
        {
            return;
        }

        var className = value.ToString();

        var subRegKey = Registry.ClassesRoot.OpenSubKey($@"{className}\DefaultIcon");

        if (subRegKey is null)
        {
            return;
        }

        value = subRegKey.GetValue("");

        var server = value?.ToString();
        if (server is null)
        {
            return;
        }

        if (server.Contains(','))
        {
            var i = server.IndexOf(',', StringComparison.Ordinal);

            var exe = server[..i];

            var index = Convert.ToInt32(server[(i + 1)..]);


            var p = ExtractIconA(IntPtr.Zero, exe, index);

            var image = Bitmap.FromHicon(p);

            Icons.Add(extension, image);

        }
        else
        {
            Icons.Add(extension, new Bitmap(server));
        }

        //
        //var shfi = new Shell32.SHFILEINFO();
        //// set search flag to file format=icon, to USEFILEATTRIBUTES=yes
        //var flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

        //// if linkoverlay is set to true add additional flag conditions (SHGFI_LINKOVERLAY)
        //if (linkOverlay) flags += Shell32.SHGFI_LINKOVERLAY;
        //flags += Shell32.SHGFI_LARGEICON;  // include the large icon flag

        //// Get the file icon from the file information
        //Shell32.SHGetFileInfo(filepath,
        //    Shell32.FILE_ATTRIBUTE_NORMAL,
        //    ref shfi,
        //    (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
        //    flags);




        //// Now clone the icon, so that it can be successfully stored in an ImageList
        //using (var icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone())
        //{
        //    User32.DestroyIcon(shfi.hIcon); // Cleanup
        //    // add default file icon to dictionary
        //    var file = new FileInfo(filepath);
        //    try
        //    {
        //        Icons.Add(file.Extension, icon.ToBitmap());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(String.Format("Error:GetFileIcon:{0}:{1}", filepath, ex.Message));
        //    }
        //}
    }

    /// <summary>
    /// Get an icon for a certain extension
    /// </summary>
    /// <param name="extension">File extension</param>
    /// <returns>Icon image</returns>
    public static Image GetIconForExtension(string extension)
    {
        return Icons.GetValueOrDefault(extension);
    }
}