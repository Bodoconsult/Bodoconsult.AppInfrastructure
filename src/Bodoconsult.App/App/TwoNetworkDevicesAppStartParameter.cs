// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reflection;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App;

/// <summary>
/// Base iplementation of <see cref="IAppStartParameter"/>
/// </summary>
public class TwoNetworkDevicesAppStartParameter : I2NetworkDevicesAppStartParameter
{
    /// <summary>
    /// Is the app started as singleton?
    /// </summary>
    public bool IsSingletonApp { get; set; }

    /// <summary>
    /// Clear text name of the app to show in windows and message boxes
    /// </summary>
    public string AppName { get; set; } = "MyApp";

    /// <summary>
    /// String with the current app version
    /// </summary>
    public string AppVersion { get; set; }

    /// <summary>
    /// Current software version
    /// </summary>
    public Version SoftwareRevision { get; set; }

    /// <summary>
    /// Should the logging of performance counters to logfile be activated
    /// </summary>
    public bool IsPerformanceLoggingActivated { get; set; }

    /// <summary>
    /// Application path
    /// </summary>
    public string AppPath { get; set; }

    /// <summary>
    /// App executable file name
    /// </summary>
    public string AppExe { get; set; }

    /// <summary>
    /// Full path to the current config file
    /// </summary>
    public string ConfigFile { get; set; }

    /// <summary>
    /// Default conenction string
    /// </summary>
    public string DefaultConnectionString { get; set; }

    /// <summary>
    /// Load a fake license
    /// </summary>
    public bool LoadFakeLicense { get; set; }

    /// <summary>
    /// The software team
    /// </summary>
    public string SoftwareTeam { get; set; }

    /// <summary>
    /// Ressource path for the app logo (NO file system path!!!)
    /// </summary>
    public string LogoRessourcePath { get; set; }

    /// <summary>
    /// Assembly to load the logo from
    /// </summary>
    public Assembly LogoAssembly { get; set; }

    /// <summary>
    /// The folder name of the app in C:\ProgramData\
    /// </summary>
    public string AppFolderName { get; set; }

    /// <summary>
    /// IP address of the network device 1 or null if no device is connected via IP
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// Port the app is listening on
    /// </summary>
    public int Port { get; set; }


    /// <summary>
    /// Base path to a folder in C:\ProgramData\ where the app stores data created by the app like backups, migrations logs and normal log files: C:\ProgramData\<see cref="IAppStartParameter.AppFolderName"/>
    /// </summary>
    public string DataPath { get; set; }

    /// <summary>
    /// Folder to store log files. Normally a subfolder of the folder <see cref="DataPath"/> 
    /// </summary>
    public string LogfilePath { get; set; }

    /// <summary>
    /// Folder to store migration log files and SQL scripts in. Normally a subfolder of the folder <see cref="DataPath"/> 
    /// </summary>
    public string MigrationLogfilePath { get; set; }

    /// <summary>
    /// Folder to store backups in. Normally a subfolder of the folder <see cref="DataPath"/>
    /// </summary>
    public string BackupPath { get; set; }

    /// <summary>
    /// Number of old backups to keep
    /// </summary>
    public int NumberOfBackupsToKeep { get; set; } = 30;

    /// <summary>
    /// IP address of the network device 2
    /// </summary>
    public string IpAddress2 { get; set; }

    /// <summary>
    /// Port of the network device 2
    /// </summary>
    public int Port2 { get; set; }
}