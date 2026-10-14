// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Reflection;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Functionaltity to load and handle plug-in assemblies
/// </summary>
/// <remarks>Plug-ins must be placed in a subdirectory called 'PlugIns' in the application path. There every plug-in must have its one subfolder. Plug-in assemblies  must be named *PlugIn.dll.</remarks>
public class PlugInHelper
{
    /// <summary>
    /// Load plug-ins from default folder appPath\PlugIns
    /// </summary>
    public static void LoadPlugIns()
    {

        var path = Path.Combine(FileHelper.GetAppPath(), "PlugIns");

        if (!Directory.Exists(path)) return;

        foreach (var dir in new DirectoryInfo(path).GetDirectories())
        {

            foreach (var file in dir.GetFiles("*.dll"))
            {
                if (file.Name.ToLower().Contains("plugin.dll"))
                {
                    LoadPlugIn(file.FullName);
                }
            }
        }

    }

    /// <summary>
    /// Load a plug-in by its file path
    /// </summary>
    /// <param name="assemblyPath">Full path to the plug-ins assembly path</param>
    public static void LoadPlugIn(string assemblyPath)
    {
        try
        {
            Assembly.LoadFrom(assemblyPath);
        }
        catch (Exception e)
        {
            throw new Exception($"LoadPlugIn:{assemblyPath}:{e}");
        }
    }
}