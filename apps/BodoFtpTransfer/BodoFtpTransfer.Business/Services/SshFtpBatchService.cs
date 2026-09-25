// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Web.Ftp.Models;
using Bodoconsult.Web.Ftp.RemoteServerHandler;
using BodoFtpTransfer.Business.Databases;
using BodoFtpTransfer.Business.Helpers;
using BodoFtpTransfer.Business.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using BodoFtpTransfer.Business.Interfaces;

namespace BodoFtpTransfer.Business.Services;

/// <summary>
/// <see cref="IFtpBatchService"/> implementation using SSH
/// </summary>
public class SshFtpBatchService : IFtpBatchService
{
    //const int CMaxSize = 131072;

    private readonly IDatabaseService _db;
    private readonly SshHandler _sftp;
    private readonly IAppLoggerProxy _logger;
    private readonly IList<FilePathItem> _allFiles = new List<FilePathItem>();
    private string _excludeDirs;
    private string _excludeRemoteDirs;


    /// <summary>
    /// Default ctor
    /// </summary>
    public SshFtpBatchService(IAppGlobals appGlobals, IDatabaseService db)
    {
        if (appGlobals is not IBodoFtpTransferAppGlobals globals)
        {
            throw new ArgumentException("appGlobals is not IBodoFtpTransferAppGlobals");
        }

        ArgumentNullException.ThrowIfNull(appGlobals.Logger);
        _logger = appGlobals.Logger;

        ArgumentNullException.ThrowIfNull(globals.Credentials);

        _sftp =  new SshHandler(globals.Credentials);
        StatusMessageDelegate = globals.StatusMessageDelegate;

        ArgumentNullException.ThrowIfNull(globals.AppStartParameter.AppPath);

        _db = db;


        //var database = Path.Combine(globals.AppStartParameter.AppPath, "BodoFtpTransfer.sqlite");

        //Debug.Print(database);

        ////_db = new SqliteDatabaseService($"Data Source={database};", statusChange);




        //#if DEBUG
        //            database = @"C:\Bodoconsult\FTP_BodoPrivate\BodoFTPTransfer.mdb";
        //#endif

        _logger.LogInformation($"Database path: {_db.ConnectionString}");

        //_hasher.Key = "ajhsd6ggf6621hdad72323%4&7eeewaawq/8)9)9_)$/6%564";

        //SqliteCommand _cmd1 = new SqliteCommand("DELETE * FROM tFiles", _conn);
        //_cmd1.ExecuteNonQuery();


        // Check if last transfer was successful
        _db.UpdateFiles();
    }

    /// <summary>
    /// Current status message delegate
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; }


    public string ExcludeDirs
    {
        get => _excludeDirs;
        set
        {
            _excludeDirs = value.Replace("{", $"{{{_base}\\").Replace("/", @"\");
            _excludeRemoteDirs = value.Replace("{", $"{{{_remoteDir}/").Replace(@"\", "/").Replace("}", "/}");
        }
    }


    public string ExcludedFiles { get; set; }

    private string _remoteDir;
    private int _remoteLen;

    public string RemoteDirectory
    {
        get => _remoteDir;
        set
        {
            _remoteDir = value;
            _remoteLen = _remoteDir.Length;
        }
    }

    private string _file;
    /// <summary>
    /// Pfad zur Batch-Datei
    /// </summary>
    public string FileName
    {
        get => _file;
        set
        {
            _file = value;
            if (File.Exists(value))
            {
                File.Delete(value);
            }
        }
    }

    private string _base;
    private int _baseLen;
    /// <summary>
    /// Zu übertragendes Verzeichnis
    /// </summary>
    public string BaseDir
    {
        get => _base;
        set
        {
            _base = value;
            _baseLen = value.Length;
        }
    }

    /// <summary>
    /// Start command collecting for saving db connections
    /// </summary>
    public void StartCommandCollecting()
    {
        _db.StartCommandCollecting();
    }

    /// <summary>
    /// Run all the collected commands on one connection
    /// </summary>
    public void RunCommandsFromCollection()
    {
        _db.RunCommandsFromCollection();
    }


    /// <summary>
    /// Prüfe, ob Datei bereits übertragen wurde oder nicht
    /// </summary>
    /// <param name="fo"> </param>
    public void CheckFile(FileInfo fo)
    {
        try
        {
            // Datei schreibgeschützt?
            if (ExcludedFiles.Contains($"{{{fo.Name.ToLower()}{'}'}", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (ExcludedFiles.Contains($"{{*{fo.Extension.ToLower()}{'}'}", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (fo.IsReadOnly)
            {
                fo.IsReadOnly = false;
            }

            var remotePath = _remoteDir + fo.FullName.Substring(_baseLen, fo.FullName.Length - _baseLen).Replace(@"\", "/");

            CheckObject(fo.FullName, remotePath, 'L', 'F', fo.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError($"CheckFile: {fo.FullName}", ex);
        }
    }

    /// <summary>
    /// Verzeichnis anlegen
    /// </summary>

    public void CheckDirectory(string localPath)
    {
        var remotePath = _remoteDir + localPath.Substring(_baseLen, localPath.Length - _baseLen);
        CheckObject(localPath, remotePath.Replace(@"\", "/", StringComparison.OrdinalIgnoreCase), 'L', 'D', 0);
    }




    /// <summary>
    /// Status-Nachricht setzen
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    private void SetMessage(string msg)
    {
        StatusMessageDelegate.Invoke(msg);
    }

    public void Quit()
    {
        // Wird jetzt pro übertragenem File erledigt!!
        //SqliteCommand _Update = new SqliteCommand("UPDATE tFiles SET F_HashCode=F_HashCodeNew, F_HashCodeNew=Null WHERE F_Type='F'", _conn);
        //_Update.ExecuteNonQuery();

        _sftp.Disconnect();

    }

    public void CheckRemoteFile(string remotePath, long size)
    {
        var localPath = _base + remotePath[_remoteLen..].Replace("/", @"\", StringComparison.OrdinalIgnoreCase);
        CheckObject(HelperCheckLocalPath(localPath), remotePath, 'R', 'F', size);
    }


    public void CheckRemoteDir(string remotePath)
    {
        var localPath = _base + remotePath[_remoteLen..].Replace("/", @"\", StringComparison.OrdinalIgnoreCase);
        CheckObject(HelperCheckLocalPath(localPath[..^1]), remotePath, 'R', 'D', 0);

    }


    private static string HelperCheckLocalPath(string localPath)
    {
        localPath = localPath.Replace("\\\\", "\\");
        if (!(localPath.StartsWith("\\", StringComparison.OrdinalIgnoreCase) & localPath.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase) == false)) return localPath;
        localPath = $"\\{localPath}";

        return localPath;
    }

    private void CheckObject(string localPath, string remotePath, char source, char objectType, long size)
    {

        var erg = _allFiles.FirstOrDefault(x => x.Path == localPath);

        //var erg = _db.CheckIfFileExists(localPath);

        // Datei nicht erfasst
        //if (erg == 0)
        if (erg==null)
        {

            var file = new FtpFiles
            {
                Path = localPath,
                PathRemote = remotePath,
                Source = source.ToString(),
                Type = objectType.ToString()
            };

            if (source == 'R')
            {
                file.Size = 0;
                file.SizeRemote = size;
            }
            else
            {
                file.Size = size;
                file.SizeRemote = 0;
            }
                
            _db.AddNew(file);
        }
        else
        {
            // Falls Datei lokal auch schon vorhanden: setze Attribut 'B'

            if (source == 'R')
            {
                _db.UpdateExistingRemoteFile(erg, size);
            }
            else
            {
                _db.UpdateExistingLocalFile(erg, size);
            }
        }
    }

    public void GetHashCodes()
    {
        try
        {
            SetMessage("Hashcode berechnen");

            var files = _db.GetFilePaths();

            var cmds = new List<DbCommand>();

            foreach (var r in files)
            {
#if DEBUG
                SetMessage($"Hashcode für {r.Path}");
#endif

                var hash = FileHelper.GetHashCode(r.Path);


                cmds.Add(_db.SetHashForFileCmd(r.Path, hash));
            }

            _db.Exec(cmds);

            //foreach (var r in files)
            //{
            //    SetMessage("Hashcode für " + r.Path);

            //    var hash = FileHelper.GetHashCode(r.Path);


            //    _db.SetHashForFile(r.Path, hash);
            //}


            //var cmd = new SqliteCommand("SELECT F_Path FROM tFiles WHERE F_Type='F' AND F_Source In ('L', 'B') ORDER BY F_Path", _conn);
            //var da = new SqliteDataAdapter(cmd);
            //var dt = new DataTable();
            //da.Fill(dt);

            //foreach (DataRow r in dt.Rows)
            //{
            //    var f = r["F_Path"].ToString();

            //    SetMessage("Hashcode für " + f);




            //    var fi = new FileStream(f, FileMode.Open);
            //    var hashcode = _hasher.ComputeHashHex(fi);
            //    fi.Close();

            //    var update = new SqliteCommand("UPDATE tFiles SET F_HashCodeNew='" + hashcode + "' WHERE F_Path=@Path", _conn);
            //    var p = new SqliteParameter("@Path", SqliteType.VarChar) { Value = f };
            //    update.Parameters.Add(p);

            //    ////// Geht nicht wegen???
            //    //_p = new SqliteParameter("@Wert", SqliteType.VarChar);
            //    //_p.Value = _hashcode;
            //    //_Update.Parameters.Add(_p);

            //    update.ExecuteNonQuery();

            //}


            //var cmd = new SqliteCommand("SELECT F_Path FROM tFiles WHERE F_Type='F' AND F_Source In ('L', 'B') ORDER BY F_Path", _conn);
            //var da = new SqliteDataAdapter(cmd);
            //var dt = new DataTable();
            //da.Fill(dt);

            //foreach (DataRow r in dt.Rows)
            //{
            //    var f = r["F_Path"].ToString();

            //    SetMessage("Hashcode für " + f);




            //    var fi = new FileStream(f, FileMode.Open);
            //    var hashcode = _hasher.ComputeHashHex(fi);
            //    fi.Close();

            //    var update = new SqliteCommand("UPDATE tFiles SET F_HashCodeNew='" + hashcode + "' WHERE F_Path=@Path", _conn);
            //    var p = new SqliteParameter("@Path", SqliteType.VarChar) { Value = f };
            //    update.Parameters.Add(p);

            //    ////// Geht nicht wegen???
            //    //_p = new SqliteParameter("@Wert", SqliteType.VarChar);
            //    //_p.Value = _hashcode;
            //    //_Update.Parameters.Add(_p);

            //    update.ExecuteNonQuery();

            //}
        }
        catch (Exception ex)
        {
            SetMessage($"GetHashCodes:Error:{ex.Message}");
        }
    }

    public void SetzeHashcodesFürRemoteFiles()
    {
        _db.SetHashForRemoteFiles();
    }


    public void CreateRemoteDirectory()
    {
        var files = _db.GetRemoteFilePathToCreate();

        foreach (var file in files)
        {
            //_S.Append(@"mkdir """ + _R["F_RemotePath"].ToString() + @"""" + "\r\n");
            MkDir(file.PathRemote);
        }
    }

    public void RemoveRemoteDirectory()
    {
        var files = _db.GetRemoteFilePathToRemove();

        foreach (var file in files)
        {
            //_S.Append(@"mkdir """ + _R["F_RemotePath"].ToString() + @"""" + "\r\n");
            MkDir(file.PathRemote);
        }
    }

    public void RemoveRemoteFiles()
    {

        var files = _db.GetRemoteFilePathRemoveFiles();

        foreach (var file in files)
        {
            Del(file.PathRemote);
        }

        // Datenbank bereinigen
        _db.ClearFiles();

    
    }
    /// <summary>
    /// Lokale Dateien auf FTP-Server kopieren
    /// </summary>
    public void CopyLocalFiles()
    {
        //var cmd = new SqliteCommand("SELECT F_Path, F_RemotePath FROM tFiles WHERE F_Type='F' AND (F_Source='L' OR (F_Source='B' AND F_Size<>F_SizeRemote) OR F_Hashcode<>F_HashCodeNew) ORDER BY F_Path ASC", _conn);
        //var da = new SqliteDataAdapter(cmd);
        //var dt = new DataTable();
        //da.Fill(dt);

        var files = _db.GetLocalFilesToCopy();
        foreach (var file in files)
        {
            if (Put(file.Path, file.PathRemote) != true) continue;
            // Bei Erfolg: Hashcode alt setzen auf neu

            _db.SetHashCodeForRemoteFile(file.Path);
        }
    }


    public void Open()
    {
        //if (!_sftp.IsConnected)
        _sftp.Connect();

        //if (!_sftp.IsConnected) return;
    }


    public void GetRemoteData(string remotePath)
    {

        if (_excludeRemoteDirs.Contains($"{{{remotePath}}}", StringComparison.OrdinalIgnoreCase))
        {
            SetMessage($"RemoteDir: {remotePath} skipped");
            return;
        }
        CheckRemoteDir(remotePath);
        SetMessage($"RemoteDir: {remotePath}");


        try
        {
            var data = _sftp.GetDirectoryItems(remotePath);
            if (data == null)
            {
                return;
            }

            foreach (var obj in data)
            {
                //if (!(obj is ChannelSftp.LsEntry)) continue;

                if (obj.Name.EndsWith(".", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (obj.IsDirectory)
                {
                    GetRemoteData($"{remotePath}{obj.Name}/");
                }
                else
                {
                    if (ExcludedFiles.Contains($"{{{obj.Name}}}", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    CheckRemoteFile(remotePath + obj.Name, obj.Length);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Getting remote data failed", ex);
        }
    }

    public void RmDir(string remotePath)
    {
        try
        {
            SetMessage($"RmDir: {remotePath}");
            _sftp.RemoveDirectory(remotePath);
        }
        catch 
        {
            SetMessage($"RmDir:Error: {remotePath} not deleted");
        }
    }

    public bool Put(string localPath, string remotePath)
    {
        try
        {
            SetMessage($"Put: {localPath[_baseLen..]}");

            //+ " to " + RemotePath
            try
            {
                // Zuerst Remotedatei löschen
                _sftp.RemoveFile(remotePath);
            }
            catch
            {
                //SetMessage("Put:Error: " + remotePath + " not transferred: "+ex.Message);
            }

            // Kopieren
            _sftp.Put(localPath, remotePath);
            return true;
        }
        catch (Exception ex) //(Tamir.SharpSsh.jsch.SftpException Ex)
        {


            SetMessage($"Put:Error: {localPath} to {remotePath}:{ex.Message}");
            return false;
        }

    }

    public void MkDir(string remotePath)
    {
        try
        {
            if (_sftp.Exists(remotePath))
            {
                return;
            }

            SetMessage($"MkDir: {remotePath}");
            _sftp.CreateDirectory(remotePath);
        }
        catch 
        {
            SetMessage($"MkDir:Error: {remotePath} not created");
        }
    }

    public void Del(string remotePath)
    {
        try
        {
            if (!_sftp.Exists(remotePath))
            {
                return;
            }

            SetMessage($"Del: {remotePath}");
            _sftp.RemoveFile(remotePath);
        }
        catch
        {
            SetMessage($"Del:Error: {remotePath} not deleted");
        }
    }

    /// <summary>
    /// Verzeichnis auf Dateien und Untervezeichnisse prüfen
    /// </summary>
    /// <param name="d">Verzeichnis</param>
    public void GetLocalData(DirectoryInfo d)
    {
        if (_excludeDirs.Contains($"{{{d.FullName}}}", StringComparison.OrdinalIgnoreCase))
        {
            SetMessage($"LocalDir: {d.FullName}\\ skipped");
            return;
        }
        SetMessage($"LocalDir: {d.FullName}");
        CheckDirectory(d.FullName);

        foreach (var dir in d.GetDirectories())
        {
            GetLocalData(dir);
        }

        foreach (var f in d.GetFiles())
        {
            CheckFile(f);
        }
    }

    /// <summary>
    /// Load all file paths from database
    /// </summary>
    public void LoadAllFilePaths()
    {
        _allFiles.Clear();
        _db.LoadAllFilePaths(_allFiles);

    }
}