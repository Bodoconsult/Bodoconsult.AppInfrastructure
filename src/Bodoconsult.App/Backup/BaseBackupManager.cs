// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using System.Web;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Backup;

/// <summary>
/// The main unit of the backup system connecting the job task and the backup system
/// </summary>
public abstract class BaseBackupManager : IBackupManager
{
    // Interne Fehlervariable zur Vermeidung von Folgefehlern
    private int _error;
    private readonly List<Task> _tasks = new();
    private readonly IBackupTargetFactory _backupTargetFactory;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="backupConfig">Current backup config</param>
    /// <param name="backupTargetFactory">Current backup target factory</param>
    protected BaseBackupManager(IBackupConfig backupConfig, IBackupTargetFactory backupTargetFactory)
    {
        StatusChanged = backupConfig.StatusChanged;
        Errors = backupConfig.Errors;
        BackupConfig = backupConfig;
        _backupTargetFactory = backupTargetFactory;
    }

    /// <summary>
    /// Current list of errors
    /// </summary>
    public IList<Exception>? Errors { get; }

    /// <summary>
    /// Status changed event
    /// </summary>
    public StatusMessageDelegate? StatusChanged { get; }

    /// <summary>
    /// Current backup config
    /// </summary>
    public IBackupConfig BackupConfig { get; }

    /// <summary>
    /// Activate shadowing if necessary
    /// </summary>
    public virtual void ActivateShadowing()
    {
        // Do nothing
    }

    /// <summary>
    /// Deactivate shadowing
    /// </summary>
    public virtual void DeactivateShadowing()
    {
        // Do nothing
    }

    /// <summary>
    /// Create a summary file for the backup process
    /// </summary>
    public void CreateSummaryFile()
    {
        var summaryFile = BackupConfig.SummaryFile;

        if (string.IsNullOrEmpty(summaryFile))
        {
            return;
        }

        if (File.Exists(summaryFile))
        {
            File.Delete(summaryFile);
        }

        var computername = Environment.MachineName;

        var content = new StringBuilder();

        content.Append("<h1>JobExecuter: File system backup</h1>\r\n");

        content.AppendLine("<h2>General information</h2>");

        content.Append($"<p>Computername: {computername}<br/>&nbsp;</p>\r\n");

        content.Append($"<p>Run time: {DateTime.Now.ToString("HH:mm")}<br/>&nbsp;</p>\r\n");

        content.AppendLine("<p></p><h2>Configuration</h2><p></p>");
        var json = System.Text.Json.JsonSerializer.Serialize( BackupConfig);
        content.AppendLine(HtmlEncode(json));
        content.AppendLine("<p></p>");

        content.AppendLine("<p></p><h2>Backup logfiles</h2><p></p>");

        foreach (var backup in BackupConfig.BackupTargets)
        {
            content.AppendLine($"<p><a href=\"{backup.Logfile}\">{backup.Logfile}</a></p>");
        }

        content.Replace("<p></p>", "<p>&nbsp;</p>");
        content.Replace("<p>&nbsp;</p>\r\n<p>&nbsp;</p>", "<p>&nbsp;</p>");

        var sw = new StreamWriter(summaryFile, false, Encoding.GetEncoding("utf-8"));
        sw.Write(content);
        sw.Close();
    }

    private static string HtmlEncode(string text)
    {
        var chars = HttpUtility.HtmlEncode(text).ToCharArray();
        var result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

        foreach (var c in chars)
        {
            var value = Convert.ToInt32(c);
            if (value > 127)
                result.AppendFormat("&#{0};", value);
            else
                result.Append(c);
        }

        return result.ToString();
    }

    /// <summary>
    /// Check if the storage media is available
    /// </summary>
    public void CheckStorageMedia()
    {
        if (string.IsNullOrEmpty(BackupConfig.GetAliveFolder))
        {
            return;
        }

        try
        {
            var f = new DirectoryInfo(BackupConfig.GetAliveFolder);
            Status($"Storage media checked: {f.GetDirectories().GetLength(0)} subdirectories found");
        }
        catch (Exception ex)
        {
            _error = 77;
            Errors?.Add(ex);
        }
    }

    /// <summary>
    /// Start the backups
    /// </summary>
    public void StartBackup()
    {
        if (_error != 0)
        {
            return;
        }

        var numThreads = BackupConfig.BackupTargets.Where(backup => !string.IsNullOrEmpty(backup.Target)).Select(backup => backup.Thread).Concat([1]).Max();

        for (var i = 0; i <= numThreads - 1; i++)
        {
            Status($"Starting thread {i + 1}...");

            var z = i + 1;

            var task = new Task(() => RunBackupsForThread(z), TaskCreationOptions.LongRunning);

            _tasks.Add(task);
            task.Start();
        }

        Task.WaitAll(_tasks.ToArray());
    }


    private void RunBackupsForThread(int threadNumber)
    {
        foreach (var backup in BackupConfig.BackupTargets.Where(backup => !string.IsNullOrEmpty(backup.Target) && backup.Thread == threadNumber))
        {
            try
            {
                var b = backup;

                if (!string.IsNullOrEmpty(BackupConfig.Command))
                {
                    b.Command = BackupConfig.Command;
                }

                b.DaysOffset = BackupConfig.DaysOffset;

                Backup(b);

            }
            catch (Exception ex)
            {
                Errors?.Add(ex);
            }
        }
    }

    private void Backup(IBackupTargetSettings backupSettings)
    {
        switch (backupSettings.BackupMode)
        {
            case BackupModeEnum.Simple:
            case BackupModeEnum.Days:
            case BackupModeEnum.DaysDate:
                if (backupSettings.Count == 0)
                {
                    backupSettings.Count = BackupConfig.Days;
                }
                break;
            case BackupModeEnum.Week:
            case BackupModeEnum.Week1:
            case BackupModeEnum.Month:
                if (backupSettings.Count == 0)
                {
                    backupSettings.Count = BackupConfig.Weeks;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (backupSettings.Count == 0)
        {
            backupSettings.Count = 5;
        }

        backupSettings.Errors = Errors;
        backupSettings.StatusChanged = StatusChanged;

        Status($"Run backup for {backupSettings.Source}...");

        var backup = _backupTargetFactory.CreateInstance(backupSettings);
        backup.StartBackupProcess();

        Status($"Backup {backupSettings.Target} done!");
    }

    /// <summary>
    /// Send state changed message
    /// </summary>
    /// <param name="msg">State changed message to be sent</param>
    private void Status(string msg)
    {
        StatusChanged?.Invoke(msg);
    }
}