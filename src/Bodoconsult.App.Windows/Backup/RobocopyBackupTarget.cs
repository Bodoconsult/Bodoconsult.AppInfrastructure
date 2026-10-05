// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.ShellTools;
using Bodoconsult.App.Backup;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bodoconsult.App.Windows.Backup;

/// <summary>
/// Implementation of <see cref="IBackupTarget"/> creating a backup target with robocopy
/// </summary>
public class RobocopyBackupTarget : IBackupTarget
{
    private string? _backupTarget;
    //private string _target;
    private readonly IShellProcessManager _shellProcessManager;
    private readonly IBackupTargetSettings _backupTargetSettings;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="shellProcessManager">Current shell process manager</param>
    /// <param name="backupTargetSettings">Current backup target settings</param>
    public RobocopyBackupTarget(IShellProcessManager shellProcessManager, IBackupTargetSettings backupTargetSettings)
    {
        _shellProcessManager = shellProcessManager;
        _backupTargetSettings = backupTargetSettings;
    }

    /// <summary>
    /// Run the backup process for the backup target now
    /// </summary>
    public void StartBackupProcess()
    {
        ArgumentNullException.ThrowIfNull(_backupTargetSettings.Target);
        ArgumentNullException.ThrowIfNull(_backupTargetSettings.Command);

        var command = _backupTargetSettings.Command;

        // Replace source
        command = command.Replace("??source??", _backupTargetSettings.Source, StringComparison.OrdinalIgnoreCase);

        // Replace logfile
        command = string.IsNullOrEmpty(_backupTargetSettings.Logfile) ? command.Replace("/log:??log??", "", StringComparison.OrdinalIgnoreCase) : command.Replace("??log??", _backupTargetSettings.Logfile, StringComparison.OrdinalIgnoreCase);

        // Replace ExcludeDirs
        command = string.IsNullOrEmpty(_backupTargetSettings.ExcludeDirs) ? command.Replace("/xd ??xd??", "", StringComparison.OrdinalIgnoreCase) : command.Replace("??xd??", _backupTargetSettings.ExcludeDirs, StringComparison.OrdinalIgnoreCase);

        // Save command for the diverse backup modes

        // Set target depending on mode
        switch (_backupTargetSettings.BackupMode)
        {
            case BackupModeEnum.Simple:
                _backupTarget = _backupTargetSettings.Target;
                break;
            case BackupModeEnum.Days:
                _backupTarget = _backupTargetSettings.Target + Weekday();
                break;
            case BackupModeEnum.DaysDate:
                _backupTarget = $"{_backupTargetSettings.Target}{BackupDate():yyyyMMdd}";

                // Falls Zielverzeichnis nicht existiert, benenne älteste Woche um in aktuelle Woche
                if (!Directory.Exists(_backupTarget))
                {
                    var endDatum = BackupDate().AddDays(-_backupTargetSettings.Count);

                    var dir = Path.Combine(_backupTargetSettings.Target, $"{endDatum:yyyyMMdd}");

                    if (Directory.Exists(dir))
                    {
                        MoveDirectory(dir, _backupTarget);
                        //dir = $"MOVE /y \"{dir}\" \"{_backupTarget}\"";
                        //dir = dir.Replace("\\\"", "\"", StringComparison.OrdinalIgnoreCase);
                        //RunInShellWait(dir);

                        CheckTarget();
                    }
                }

                break;
            case BackupModeEnum.Week:
                _backupTarget = _backupTargetSettings.Target + CalenderWeek(BackupDate());
                break;
            case BackupModeEnum.Week1:
                _backupTarget = _backupTargetSettings.Target + CalenderWeek(BackupDate());

                // Falls Zielverzeichnis nicht existiert, benenne älteste Woche um in aktuelle Woche
                if (!Directory.Exists(_backupTarget))
                {
                    var d = BackupDate().AddDays(-Convert.ToDouble(_backupTargetSettings.Count * 7));
                    var kw = CalenderWeek(d);
                    var dir = Path.Combine(_backupTargetSettings.Target, $"{kw:000000}");

                    if (Directory.Exists(dir))
                    {
                        MoveDirectory(dir, _backupTarget);

                        //dir = $"MOVE /y \"{dir}\" \"{_backupTarget}\"";
                        //dir = dir.Replace("\\\"", "\"", StringComparison.OrdinalIgnoreCase);
                        //RunInShellWait(dir);

                        CheckTarget();
                    }
                }

                break;
            case BackupModeEnum.Month:
                _backupTarget = _backupTargetSettings.Target + BackupDate().ToString("yyyyMM");

                // Falls Zielverzeichnis nicht existiert, benenne älteste Woche um in aktuelle Woche
                if (!Directory.Exists(_backupTarget))
                {
                    var d = BackupDate().AddMonths(-_backupTargetSettings.Count);
                    var dir = Path.Combine(_backupTargetSettings.Target, $"{d:yyyyMM}");

                    if (Directory.Exists(dir))
                    {
                        MoveDirectory(dir, _backupTarget);

                        //dir = $"MOVE /y \"{dir}\" \"{_backupTarget}\"";
                        //dir = dir.Replace("\\\"", "\"", StringComparison.OrdinalIgnoreCase);
                        //RunInShellWait(dir);

                        CheckTarget();
                    }
                }
                break;
        }


        if (string.IsNullOrEmpty(_backupTarget))
        {
            return;
        }

        Status($"Cleanup for {_backupTargetSettings.Target} is running...");
        ClearBackups();

        //try
        //{

        switch (_backupTargetSettings.BackupMode)
        {
            case BackupModeEnum.Month:
            case BackupModeEnum.Week1:
            case BackupModeEnum.Week:
            case BackupModeEnum.DaysDate:
            case BackupModeEnum.Simple:
                break;
            default:
                RemoveDirectory(_backupTarget);
                break;
        }

        CheckTarget();

        var args = command.Replace("robocopy", "", StringComparison.OrdinalIgnoreCase).Replace("??target??", _backupTarget, StringComparison.OrdinalIgnoreCase);
        Status($"robocopy.exe {args}");
        RunRobocopy(args);
    }

    private void RunRobocopy(string args)
    {
        var p = new RobocopyShellProcessParameters
        {
            Args = args,
            StatusMessageDelegate = Status,
            HandleExceptionDelegate = HandleExceptionDelegateInternal
        };
        _shellProcessManager.RunRobocopy(p);

        // ShellAndWait("robocopy.exe", args);
    }

    private void RemoveDirectory(string path)
    {
        var p = new RemoveDirectoryShellProcessParameters
        {
            Path = path,
            StatusMessageDelegate = Status,
            HandleExceptionDelegate = HandleExceptionDelegateInternal
        };
        _shellProcessManager.RemoveDirectory(p);

        //if (Directory.Exists(_backupTarget))
        //{
        //    var cmd = GetRdCommand(_backupTarget);
        //    RunInShellWait(cmd);
        //}
    }

    private void MoveDirectory(string dir, string backupTarget)
    {
        var p = new MoveDirectoryShellProcessParameters
        {
            SourcePath = dir,
            TargetPath = backupTarget,
            StatusMessageDelegate = Status,
            HandleExceptionDelegate = HandleExceptionDelegateInternal
        };
        _shellProcessManager.MoveDirectory(p);
    }

    private void HandleExceptionDelegateInternal(Exception e)
    {
        var message = e.ToString();
        _backupTargetSettings.Errors?.Add(e);
        _backupTargetSettings.StatusChanged?.Invoke(message);
    }


    private void CheckTarget()
    {
        ArgumentNullException.ThrowIfNull(_backupTarget);

        if (!Directory.Exists(_backupTarget))
        {
            Directory.CreateDirectory(_backupTarget);
        }
    }

    /// <summary>
    /// Clear old backups if they are deprecated already
    /// </summary>
    public void ClearBackups()
    {
        // Alles außer Modus Woche: hier beenden
        switch (_backupTargetSettings.BackupMode)
        {
            case BackupModeEnum.Week:
            case BackupModeEnum.Week1:
                ClearWeeks();
                break;
            case BackupModeEnum.DaysDate:
                ClearDays();
                break;
            case BackupModeEnum.Month:
                ClearMonths();
                break;
            default:
                return;
        }
    }

    private void ClearDays()
    {
        ArgumentNullException.ThrowIfNull(_backupTargetSettings.Target);

        var endDatum = BackupDate().AddDays(-_backupTargetSettings.Count);

        Parallel.For(0, 60, i =>
        {
            var d = endDatum.AddDays(-i - 1);
            var dir = Path.Combine(_backupTargetSettings.Target, $"{d:yyyyMMdd}");

            //Status("ClearBackups: " + dir);

            try
            {
                RemoveDirectory(dir);

                //if (!Directory.Exists(dir))
                //{
                //    return;
                //}
                //var cmd = GetRdCommand(dir);
                //Status($"ClearBackups: {cmd}");
                //RunInShellWait(cmd);
                //Directory.Delete(_Dir, true);
            }
            catch (Exception ex)
            {
                Status(ex.ToString());
            }
        });


        //for (var d = startDatum; d <= endDatum; d = d.AddDays(1))
        //{

        //    var dir = Path.Combine(_target, String.Format("{0:yyyyMMdd}", d));
        //    try
        //    {
        //        if (!Directory.Exists(dir)) continue;
        //        var cmd = GetRdCommand(dir);
        //        Status(cmd);
        //        RunInShellWait(cmd);
        //        //Directory.Delete(_Dir, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Bodoconsult.Logging.FileLog.GetInstance().LogError(ex);
        //    }
        //}
    }


    private void ClearWeeks()
    {
        ArgumentNullException.ThrowIfNull(_backupTargetSettings.Target);

        var date = BackupDate().AddDays(-Convert.ToDouble((_backupTargetSettings.Count - 1) * 7));

        Parallel.For(0, 3 * _backupTargetSettings.Count, i =>
        {
            var woche = i + 1;
            var d = date.AddDays(-Convert.ToDouble(woche * 7));
            var kw = CalenderWeek(d);

            //Status(String.Format("KW {0} {1}", d, kw));

            var dir = Path.Combine(_backupTargetSettings.Target, $"{kw:000000}");

            //Status("ClearBackups: " + dir);

            try
            {
                RemoveDirectory(dir);

                //if (!Directory.Exists(dir)) return;
                //var cmd = GetRdCommand(dir);
                //Status($"ClearBackups: {cmd}");
                //RunInShellWait(cmd);
            }
            catch (Exception ex)
            {
                Status(ex.ToString());
            }
        });


        //for (var i = _wochen; i <= 8 * _wochen; i++)
        //{
        //    var d = BackupDate().AddDays(-Convert.ToDouble(i * 7));
        //    var kw = Kalenderwoche(d);
        //    var dir = Path.Combine(_target, String.Format("{0:000000}", kw));
        //    try
        //    {
        //        if (!Directory.Exists(dir)) continue;
        //        var cmd = GetRdCommand(dir);
        //        Status(cmd);
        //        RunInShellWait(cmd);
        //        //Directory.Delete(_Dir, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Bodoconsult.Logging.FileLog.GetInstance().LogError(ex);
        //    }
        //}
    }

    private void ClearMonths()
    {
        ArgumentNullException.ThrowIfNull(_backupTargetSettings.Target);

        var date = BackupDate().AddDays(-Convert.ToDouble((_backupTargetSettings.Count - 1) * 31));

        Parallel.For(0, 3 * _backupTargetSettings.Count, i =>
        {
            var woche = i + 1;
            var d = date.AddDays(-Convert.ToDouble(woche * 31));

            //Status(String.Format("KW {0} {1}", d, kw));

            var dir = Path.Combine(_backupTargetSettings.Target, $"{d:yyyyMM}");

            //Status("ClearBackups: " + dir);

            try
            {
                RemoveDirectory(dir);

                //if (!Directory.Exists(dir))
                //{
                //    return;
                //}
                //var cmd = GetRdCommand(dir);
                //Status($"ClearBackups: {cmd}");
                //RunInShellWait(cmd);
            }
            catch (Exception ex)
            {
                Status(ex.ToString());
            }
        });
    }

    private DateTime BackupDate()
    {
        return DateTime.Now.AddDays(_backupTargetSettings.DaysOffset);
    }


    //private static string GetRdCommand(string path)
    //{
    //    return $"""rd "{path}" /S /Q""";
    //}

    ///// <summary>
    ///// Run command in shell
    ///// </summary>
    ///// <param name="strShellCmd">Shell command to run</param>
    //private void RunInShellWait(string strShellCmd)
    //{
    //    //Der Bereich funktioniert. Nicht mehr modifizieren!!!
    //    //Hier wird das Array Zeile für Zeile abgearbeitet und an die Shell übergeben

    //    if (string.IsNullOrEmpty(strShellCmd))
    //    {
    //        return;
    //    }

    //    var psi = new ProcessStartInfo("cmd.exe ", $@"/C {strShellCmd}") { UseShellExecute = true, WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true };
    //    try
    //    {
    //        var process = Process.Start(psi);

    //        if (process is null)
    //        {
    //            // ToDo: logging
    //            return;
    //        }
    //        process.WaitForExit();
    //    }
    //    catch (Exception ex)
    //    {
    //        Errors.Add(ex);
    //    }
    //}

    /// <summary>
    /// Tage der Woche in jeweiliger Sprache des Systems liefern
    /// </summary>
    /// <returns></returns>
    private static string Weekday()
    {
        return Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
    }

    /// <summary>
    /// Get calendar week with ISO rules
    /// </summary>
    /// <param name="date">Date, the week calendar should be calculated for</param>
    /// <returns>Calendar week written as YYYYWW</returns>
    private static int CalenderWeek(DateTime date)
    {
        var a = Math.Floor((14 - date.Month) / 12D);
        var y = date.Year + 4800 - a;
        var m = date.Month + 12 * a - 3;

        var jd = date.Day + Math.Floor((153 * m + 2) / 5) +
            365 * y + Math.Floor(y / 4) - Math.Floor(y / 100) +
            Math.Floor(y / 400) - 32045;

        var d4 = (jd + 31741 - jd % 7) % 146097 % 36524 % 1461;
        var l = Math.Floor(d4 / 1460);
        var d1 = (d4 - l) % 365 + l;

        // Kalenderwoche ermitteln
        var calendarWeek = (int)Math.Floor(d1 / 7) + 1;

        // Das Jahr der Kalenderwoche ermitteln
        var year = date.Year;
        if (calendarWeek == 1 && date.Month == 12)
        {
            year++;
        }
        if (calendarWeek >= 52 && date.Month == 1)
        {
            year--;
        }

        // Die ermittelte Kalenderwoche zurückgeben
        return year * 100 + calendarWeek;
    }

    ///// <summary>
    ///// Programm starten oder eine Datei mit zugehöriger Anwendung öffnen
    ///// Warte bis Anwendung beendet wurde
    ///// </summary>
    ///// <param name="exe">Pfad zur EXE oder einer anderen Datei</param>
    ///// <param name="args">Befehlszeilenargumente</param>
    //public void ShellAndWait(string exe, string args)
    //{
    //    var psi = new ProcessStartInfo(exe, args)
    //    {
    //        UseShellExecute = false,
    //        RedirectStandardOutput = true,
    //        RedirectStandardError = true,
    //        WindowStyle = ProcessWindowStyle.Hidden,
    //        CreateNoWindow = true
    //    };
    //    try
    //    {
    //        Status($"Run {exe} {args}");
    //        RunBackupProcess(exe, args, psi);
    //    }
    //    catch (Exception ex)
    //    {
    //        Errors.Add(ex);
    //    }
    //}

    //private void RunBackupProcess(string exe, string args, ProcessStartInfo psi)
    //{
    //    var p = new Process
    //    {
    //        StartInfo = psi,
    //        PriorityClass = ProcessPriorityClass.AboveNormal
    //    };

    //    p.OutputDataReceived += (_, e) => Status(e.Data);
    //    p.ErrorDataReceived += (_, e) =>
    //    {
    //        var msg = $"CmdBatch:Error>>{exe} {args}>>{e.Data}";
    //        Errors.Add(new Exception(msg));
    //        Status($"CmdBatch:Error>>{e.Data}");
    //    };

    //    p.BeginOutputReadLine();

    //    p.Start();
    //    p.WaitForExit();

    //    if (p.ExitCode > 0)
    //    {
    //        var msg = $"CmdBatch:Error>>{exe} {args}>>ExitCode {p.ExitCode}";
    //        Errors.Add(new Exception(msg));
    //        Status(msg);
    //    }
    //    p.Close();
    //}

    /// <summary>
    /// Status-Event auslösen
    /// </summary>
    /// <param name="msg">Meldung</param>
    private void Status(string msg)
    {
        _backupTargetSettings.StatusChanged?.Invoke(msg);
    }
}