// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using Bodoconsult.App.Helpers;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.App;

/// <summary>
/// Application global values
/// </summary>
public static class GlobalValues
{
    /// <summary>
    /// Current app settings
    /// </summary>
    public static AppSettings CurrentAppSettings { get; private set; }

    /// <summary>
    /// Load app settings from app directory
    /// </summary>
    public static void LoadAppSettings()
    {
        var path = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(path);

        path = new FileInfo(path).DirectoryName;
        ArgumentNullException.ThrowIfNull(path);

        var fileName = Path.Combine(path, "appSettings.json");

        CurrentAppSettings = SimpleJsonHelper.LoadJsonFile<AppSettings>(fileName);
    }
}