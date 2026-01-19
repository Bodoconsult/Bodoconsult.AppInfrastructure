// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using System.Reflection;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for app start parameter
/// </summary>
public interface IAppStartParameter
{
    /// <summary>
    /// Is the app started as singleton?
    /// </summary>
    public bool IsSingletonApp { get; set; }

    /// <summary>
    /// Clear text name of the app to show in windows and message boxes
    /// </summary>
    string AppName { get; set; }

    /// <summary>
    /// String with the current app version
    /// </summary>
    string AppVersion { get; set; }

    /// <summary>
    /// Current software version
    /// </summary>
    Version SoftwareRevision { get; set; }

    /// <summary>
    /// Should the logging of performance counters to logfile be activated
    /// </summary>
    bool IsPerformanceLoggingActivated { get; set; }

    /// <summary>
    /// Application path
    /// </summary>
    string AppPath { get; set; }

    /// <summary>
    /// App executable file name
    /// </summary>
    string AppExe { get; set; }

    /// <summary>
    /// Full path to the current config file
    /// </summary>
    string ConfigFile { get; set; }

    /// <summary>
    /// Default conenction string
    /// </summary>
    string DefaultConnectionString { get; set; }

    /// <summary>
    /// Load a fake license
    /// </summary>
    bool LoadFakeLicense { get; set; }

    /// <summary>
    /// The software team
    /// </summary>
    string SoftwareTeam { get; set; }

    /// <summary>
    /// Ressource path for the app logo (NO file system path!!!)
    /// </summary>
    string LogoRessourcePath { get; set; }

    /// <summary>
    /// Assembly to load the logo from
    /// </summary>
    Assembly LogoAssembly { get; set; }

    /// <summary>
    /// The folder name of the app in C:\ProgramData\
    /// </summary>
    string AppFolderName { get; set; }

    /// <summary>
    /// Port the app is listening on
    /// </summary>
    int Port { get; set; }

    /// <summary>
    /// Base path to a folder in C:\ProgramData\ where the app stores data created by the app like backups, migrations logs and normal log files: C:\ProgramData\<see cref="AppFolderName"/>
    /// </summary>
    string DataPath { get; set; }

    /// <summary>
    /// Folder to to store log files. Normally the folder <see cref="DataPath"/> to make log dump creation easier
    /// </summary>
    string LogfilePath { get; set; }

    /// <summary>
    /// Folder to store migration log files and SQL scripts in. Normally a subfolder of the folder <see cref="DataPath"/> 
    /// </summary>
    string MigrationLogfilePath { get; set; }

    /// <summary>
    /// Folder to store backups in. Normally a subfolder of the folder <see cref="DataPath"/>
    /// </summary>
    string BackupPath { get; set; }

    /// <summary>
    /// Number of old backups to keep
    /// </summary>
    int NumberOfBackupsToKeep { get; set; }
}