// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reflection;
using Bodoconsult.App.Security.Hashing;

namespace BodoFtpTransfer.Business.Helpers;

/// <summary>
/// Helper methods for files
/// </summary>
public static class FileHelper
{
    private static readonly Hasher Hasher = new(HashAlgorithmEnum.SHA1);

    /// <summary>
    /// Get a hashcode for a file
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetHashCode(string fileName)
    {
        var fi = new FileStream(fileName, FileMode.Open);
        var hashcode = Hasher.ComputeHashHex(fi);
        fi.Close();

        return hashcode;
    }

    /// <summary>
    /// Get a text from an embedded resource file
    /// </summary>
    /// <param name="resourceName">resource name = file name</param>
    /// <returns></returns>
    public static string? GetTextResource(string resourceName)
    {
        var ass = Assembly.GetExecutingAssembly();
        var str = ass.GetManifestResourceStream(resourceName);

        if (str == null) return null;

        string s;

        using (var file = new StreamReader(str))
        {
            s = file.ReadToEnd();
        }

        return s;
    }
}