// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Reflection;

namespace Bodoconsult.App.Abstractions.Helpers;

/// <summary>
/// Helper class to read text or SQL resources
/// </summary>
public static class ResourceHelper
{
    /// <summary>
    /// Get a byte array from an embedded resource file in the calling assembly
    /// </summary>
    /// <param name="resourceName">resource name = file name</param>
    /// <returns></returns>
    public static byte[] GetByteResource(string resourceName)
    {
        return GetByteResource(Assembly.GetCallingAssembly(), resourceName);
    }

    /// <summary>
    /// Get a byte array from an embedded resource file from an assembly
    /// </summary>
    /// <param name="assembly">Assembly to load the resource from</param>
    /// <param name="resourceName">resource name = file name</param>
    /// <returns></returns>
    public static byte[] GetByteResource(Assembly assembly, string resourceName)
    {
        var str = assembly.GetManifestResourceStream(resourceName);

        if (str is null) return [];

        var data = new byte[str.Length];

        str.ReadExactly(data, 0, (int)str.Length);

        return data;
    }

    /// <summary>
    /// Get a text from an embedded resource file
    /// </summary>
    /// <param name="resourceName">resource name = file name</param>
    /// <returns></returns>
    public static string? GetTextResource(string resourceName)
    {
        var ass = Assembly.GetCallingAssembly();
        return GetTextResource(ass, resourceName);
    }

    /// <summary>
    /// Get a text from an embedded resource file by its full resource name
    /// </summary>
    /// <param name="assembly">Assembly the resource to load from</param>
    /// <param name="resourceName">resource name = plain file name with extension and path</param>
    /// <returns></returns>
    public static string? GetTextResource(Assembly assembly, string resourceName)
    {
        var str = assembly.GetManifestResourceStream(resourceName);

        if (str is null)
        {
            return null;
        }

        using var file = new StreamReader(str);
        var s = file.ReadToEnd();

        return s;
    }

    /// <summary>
    /// Save an embedded resource file to an external file
    /// </summary>
    /// <param name="assembly">Assembly to load the resources from</param>
    /// <param name="resourceName">resource name = file name</param>
    /// <param name="fileName">File path to save the binary ressource</param>
    /// <returns></returns>
    public static void SaveBinaryResource(Assembly assembly, string resourceName, string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        using var input = assembly.GetManifestResourceStream(resourceName);
        using var output = File.Open(fileName, FileMode.CreateNew);
        if (input == null)
        {
            throw new FileNotFoundException($"{resourceName}: Embedded resoure file not found");
        }

        var buffer = new byte[32768];
        int read;
        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, read);
        }
        output.Flush();
    }

    ///// <summary>
    ///// Get a text from an embedded resource file
    ///// </summary>
    ///// <param name="assembly">Assembly to load ressources from</param>
    ///// <param name="resourceName">resource name = file name</param>
    ///// <returns></returns>
    //public static string? GetTextResource(Assembly assembly, string resourceName)
    //{
    //    var str = assembly.GetManifestResourceStream(resourceName);

    //    if (str == null)
    //    {
    //        return null;
    //    }

    //    using var file = new StreamReader(str);
    //    var s = file.ReadToEnd();

    //    return s;
    //}
}