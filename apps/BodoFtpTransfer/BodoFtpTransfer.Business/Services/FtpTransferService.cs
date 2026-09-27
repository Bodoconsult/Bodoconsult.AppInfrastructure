// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoFtpTransfer.Business.Interfaces;
using System.Reflection;

namespace BodoFtpTransfer.Business.Services;

public class FtpTransferService : IFtpTransferService
{ 
    private readonly string _batch = string.Empty;
    private readonly IFtpBatchService _ftpBatch;
    private string _excludedFiles = string.Empty;
    private int _baseLen;
    private string _baseDir = string.Empty;
    private readonly IAppLoggerProxy _logger;

    /// <summary>
    /// Current status message delegate
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; }

    public FtpTransferService(IAppGlobals appGlobals, IFtpBatchService ftpBatchService)
    {
        ArgumentNullException.ThrowIfNull(appGlobals.StatusMessageDelegate);

        if (appGlobals is not IBodoFtpTransferAppGlobals globals)
        {
            throw new ArgumentException("appGlobals is not IBodoFtpTransferAppGlobals");
        }

        ArgumentNullException.ThrowIfNull(appGlobals.Logger);
        _logger = appGlobals.Logger;

        ArgumentNullException.ThrowIfNull(appGlobals.StatusMessageDelegate);
        StatusMessageDelegate = appGlobals.StatusMessageDelegate;

        SetMessage("Starting FtpTransfer...");

        _ftpBatch = ftpBatchService;
        Modus = 0;

        var fileName = Assembly.GetExecutingAssembly().Location;
        if (fileName == null)
        {
            throw new Exception("No application directory found (1)");
        }

        var curDir = new FileInfo(fileName).DirectoryName;
        if (curDir != null)
        {
            _batch = Path.Combine(curDir, "ftp.txt");
        }

        BaseDirectory = globals.BaseDirectory;
        ExcludedFiles = globals.ExcludeFiles;
        ExcludeDirs = globals.ExcludeDirs;
        RemoteDirectory = globals.RemoteDirectory;
    }

    /// <summary>
    /// Transfer mode
    /// 0 = transfer all
    /// 1 = mark all files and directories as transferred already
    /// </summary>
    public int Modus { get; set; }

    /// <summary>
    /// Directory names to exclude from transfer
    /// </summary>
    public string ExcludeDirs { get; set; }

    /// <summary>
    /// File names to exclude from transfer
    /// </summary>
    public string ExcludedFiles
    {
        get => _excludedFiles;
        set => _excludedFiles = value.ToLowerInvariant();
    }

    /// <summary>
    /// Relative path of the remote base directory to transfer to
    /// </summary>
    public string RemoteDirectory { get; set; }

    /// <summary>
    /// Local base directory to transfer
    /// </summary>
    public string BaseDirectory
    {
        get => _baseDir;
        set
        {
            _baseDir = value;
            _baseLen = value.Length;
        }
    }

    /// <summary>
    /// Create batch file for FTP transfer
    /// </summary>
    public void CreateBatch()
    {
        SetMessage("Update database...");
        _ftpBatch.BaseDir = _baseDir;
        _ftpBatch.FileName = _batch;
        _ftpBatch.RemoteDirectory = RemoteDirectory;
        _ftpBatch.ExcludedFiles = _excludedFiles;
        _ftpBatch.ExcludeDirs = ExcludeDirs;
        _ftpBatch.Open();

        // Get all remote files and directories
        _ftpBatch.LoadAllFilePaths();
        _ftpBatch.StartCommandCollecting();
        _ftpBatch.GetRemoteData(
            $"{RemoteDirectory}{_baseDir.Substring(_baseLen, _baseDir.Length - _baseLen).Replace(@"\", "/", StringComparison.OrdinalIgnoreCase)}/");
        SetMessage("Save results to database...");
        _ftpBatch.RunCommandsFromCollection();

        // Get all local files and directories
        _ftpBatch.LoadAllFilePaths();
        _ftpBatch.StartCommandCollecting();
        _ftpBatch.GetLocalData(new DirectoryInfo(_baseDir));
        SetMessage("Save results to database...");
        _ftpBatch.RunCommandsFromCollection();

        // Get file hashcodes to know updated files
        _ftpBatch.GetHashCodes();

        _ftpBatch.SetzeHashcodesFürRemoteFiles();

        _ftpBatch.Open();

        // Transfer new or updated data to website...
        SetMessage("Transfer new or updated data to website...");
        _ftpBatch.CreateRemoteDirectory();
        _ftpBatch.CopyLocalFiles();

        // Remove old data from website...
        SetMessage("Remove old data from website...");
        _ftpBatch.RemoveRemoteFiles();
        _ftpBatch.RemoveRemoteDirectory();


        _ftpBatch.Quit();

        SetMessage("Transfer done!");
    }

    /// <summary>
    /// Status-Nachricht setzen
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    private void SetMessage(string msg)
    {
        _logger.LogInformation(msg);
        StatusMessageDelegate.Invoke(msg);
    }
}