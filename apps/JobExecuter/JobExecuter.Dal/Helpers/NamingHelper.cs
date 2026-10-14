// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Helper class for naming issues
/// </summary>
public class NamingHelper
{
    /// <summary>
    /// Get the computer name
    /// </summary>
    /// <returns></returns>
    public static string GetComputerName()
    {
        return Environment.MachineName;
    }

    /// <summary>
    /// Clear a path from possible placeholders
    /// </summary>
    /// <param name="path">path to clear from placeholders</param>
    /// <param name="server">server namme or server and instance name</param>
    /// <param name="database">databse name</param>
    /// <returns></returns>
    public static string ClearPath(string path, string server, string database)
    {
        server = server.Replace("\\", "_")
            .Replace(".", Environment.MachineName)
            .Replace("(local)", Environment.MachineName);

        return path.Replace("??server??", server).Replace("??database??", database)
            .Replace("??date??", $"{DateTime.Now:yyyyMMdd}")
            .Replace("??time??", $"{DateTime.Now:yyyyMMddHHmm}");
    }

}