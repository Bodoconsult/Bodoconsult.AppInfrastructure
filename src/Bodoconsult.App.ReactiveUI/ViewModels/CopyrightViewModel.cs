// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.ReactiveUI.ViewModels;

/// <summary>
/// Viewmodel for a copyright dialog
/// </summary>
public partial class CopyrightViewModel : ReactiveObject
{
    private readonly List<string> _modules = new();
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    public CopyrightViewModel(IAppGlobals appGlobals)
    {
        _appGlobals = appGlobals;

        var modulInfo = $"{AppTitle} {_appGlobals.AppStartParameter.AppVersion}";
        AppTitle = modulInfo;
        _modulesInfo = string.Empty;
        _licenseInfo = string.Empty;
        _toolInfo = string.Empty;

        LoadModule(modulInfo);
    }

    /// <summary>
    /// Menu text for open menu in system tray bar
    /// </summary>
    [Reactive] public partial string AppTitle { get; set; }

    /// <summary>
    /// Module info
    /// </summary>
    [Reactive] public partial string ModulesInfo { get; set; }

    /// <summary>
    /// License info
    /// </summary>
    [Reactive] public partial string LicenseInfo { get; set; }

    /// <summary>
    /// Tool info
    /// </summary>
    [Reactive] public partial string ToolInfo { get; set; }

    /// <summary>
    /// Load a module
    /// </summary>
    /// <param name="modulInfo">Module info string</param>
    public void LoadModule(string modulInfo)
    {
        modulInfo = modulInfo.Trim();

        if (string.IsNullOrEmpty(modulInfo) || _modules.Contains(modulInfo))
        {
            return;
        }

        _modules.Add(modulInfo);
        ModulesInfo = string.Join("\r\n", _modules);
    }

    /// <summary>
    /// Load used libraries info from LICENSE.md in app directory
    /// </summary>
    public void LoadLicenseInfo()
    {
        if (string.IsNullOrEmpty(_appGlobals.AppStartParameter.AppPath))
        {
            return;
        }
        var path = Path.Combine(_appGlobals.AppStartParameter.AppPath, "LICENSE.md");
        LoadLicenseInfo(path);
    }

    /// <summary>
    /// Load used libraries info from a Markdown file
    /// </summary>
    /// <param name="filePath">Path to a Markdown file</param>
    public void LoadLicenseInfo(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        LicenseInfo = ReadMarkDownFile(filePath);
    }

    /// <summary>
    /// Load used libraries info from LICENSE.md in app directory
    /// </summary>
    public void LoadToolInfo()
    {
        if (string.IsNullOrEmpty(_appGlobals.AppStartParameter.AppPath))
        {
            return;
        }
        var path = Path.Combine(_appGlobals.AppStartParameter.AppPath, "TOOLS.md");
        LoadToolInfo(path);
    }

    /// <summary>
    /// Load used libraries info from a Markdown file
    /// </summary>
    /// <param name="filePath">Path to a Markdown file</param>
    public void LoadToolInfo(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        var s = ReadMarkDownFile(filePath);
        ToolInfo = s;
    }

    private static string ReadMarkDownFile(string filePath)
    {
        var content = File.ReadAllText(filePath).Replace("\r", string.Empty);

        var rows = content.Split('\n');
        var result = new List<string>();

        foreach (var row in rows)
        {
            string msg;
            string fill;
            if (row.StartsWith("# ", StringComparison.OrdinalIgnoreCase))
            {
                msg = $"***** {row.Replace("# ", string.Empty, StringComparison.OrdinalIgnoreCase)} *****";
                fill = new string('*', msg.Length);
                result.Add("\n");

                result.Add("\n");
                result.Add("\n");
                result.Add(fill);
                result.Add(fill);
                result.Add(msg);
                result.Add(fill);
                result.Add(fill);
                continue;
            }

            if (row.StartsWith("## ", StringComparison.OrdinalIgnoreCase))
            {
                msg = $"*** {row.Replace("## ", string.Empty, StringComparison.OrdinalIgnoreCase)} ***";
                fill = new string('*', msg.Length);
                result.Add("\n");
                result.Add(fill);
                result.Add(msg);
                result.Add(fill);
                continue;
            }

            result.Add(row);
        }

        foreach (var row in result.ToList())
        {
            if (row == "\n")
            {
                result.Remove(row);
                continue;
            }

            break;
        }

        var s = string.Join('\n', result);
        return s;
    }
}