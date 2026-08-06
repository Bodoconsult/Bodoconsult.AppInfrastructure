// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Helper class for file system relevante methods
/// </summary>
public static class FileSystemHelper
{
    /// <summary>
    /// Checks if a string contains invalid chars and returns the first invalid char
    /// </summary>
    /// <param name="valueToCheck">Value to check</param>
    /// <returns>Invalid char or null</returns>
    public static string CheckForInvalidChars(string valueToCheck)
    {
        var s = new StringBuilder();

        foreach (var invalidFileNameChar in Path.GetInvalidFileNameChars())
        {
            if (valueToCheck.Contains(invalidFileNameChar.ToString(CultureInfo.InvariantCulture)))
            {
                s.Append( $"{invalidFileNameChar} (\\u{(int)invalidFileNameChar:0000}), ");
            }
        }

        var result = s.ToString();

        return result.EndsWith(", ", StringComparison.OrdinalIgnoreCase) ? result[..^2] : result;
    }

    /// <summary>
    /// Get the plain name of a file without extension and folder path
    /// </summary>
    /// <param name="fullPath">Full path to the file</param>
    /// <returns>Plain file name</returns>
    public static string GetFileNameWithoutExtension(string? fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            return string.Empty;
        }

        var indexOfSlash = fullPath.LastIndexOf("\\", StringComparison.Ordinal);
        var indexOfPoint = fullPath.LastIndexOf(".", StringComparison.Ordinal);
        var fileName = fullPath.Substring(indexOfSlash + 1, indexOfPoint - 1 - indexOfSlash);
        return fileName;
    }

    /// <summary>
    /// Run a file in debug  mode only
    /// </summary>
    /// <param name="fileName">Full file path</param>
    public static void RunInDebugMode(string fileName)
    {
        if (!Debugger.IsAttached)
        {
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }

    /// <summary>
    /// Run a file
    /// </summary>
    /// <param name="fileName">Full file path</param>
    public static void Run(string fileName)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }
}