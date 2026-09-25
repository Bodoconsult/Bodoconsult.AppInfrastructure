// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.Sqlite;
using BodoFtpTransfer.Business.Helpers;
using BodoFtpTransfer.Business.Interfaces;
using BodoFtpTransfer.Business.Model;
using Microsoft.Data.Sqlite;

namespace BodoFtpTransfer.Business.Databases;

public class SqliteDatabaseService : IDatabaseService
{
    private readonly IConnManager _db;

    private ConcurrentBag<DbCommand> _commands;

    private bool _isCollecting;

    public SqliteDatabaseService(string connectionString, IAppGlobals appGlobals)
    {
        ConnectionString = connectionString;
        _db = new SqliteConnManager(connectionString);

        ArgumentNullException.ThrowIfNull(appGlobals.StatusMessageDelegate);
        StatusMessageDelegate = appGlobals.StatusMessageDelegate;
    }

    /// <summary>
    /// Current status message delegate
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Connection string
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Start command collecting for saving db connections
    /// </summary>
    public void StartCommandCollecting()
    {
        _commands = new ConcurrentBag<DbCommand>();
        _isCollecting = true;
    }

    /// <summary>
    /// Run all the collected commands on one connection
    /// </summary>
    public void RunCommandsFromCollection()
    {
        var data = _commands.ToList();

        SetMessage($"Run {data.Count} commands...");

        _commands = null;

        var result = _db.ExecMultiple(data);

        _isCollecting = false;

        if (result == 0)
        {
            return;
        }

        var sql = data[result];

        throw new Exception($"Error processing sql: {sql}");
    }

    /// <summary>
    /// Status-Nachricht setzen
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    private void SetMessage(string msg)
    {
        StatusMessageDelegate?.Invoke(msg);
    }

    /// <summary>
    /// Insert a data row into table Files from entity class Files object
    /// </summary>
    public void AddNew(FtpFiles item)
    {
        const string sql = "INSERT INTO \"FtpFiles\"(\"Path\", \"HashCode\", \"PathRemote\", \"Source\", \"HashCodeNew\", \"Type\", \"Size\", \"SizeRemote\") " +
                           "VALUES (@Path, @HashCode, @PathRemote, @Source, @HashCodeNew, @Type, @Size, @SizeRemote)";

        var cmd = new SqliteCommand(sql);

        SqliteParameter p;

        // Parameter @ID
        //p = new SqliteParameter("@ID", SqliteType.Integer) { Value = item.ID };
        //cmd.Parameters.Add(p);

        // Parameter @Path
        p = new SqliteParameter("@Path", SqliteType.Text) { Value = item.Path };
        cmd.Parameters.Add(p);

        // Parameter @HashCode
        p = new SqliteParameter("@HashCode", SqliteType.Text) { Value = item.HashCode ?? "" };
        cmd.Parameters.Add(p);

        // Parameter @PathRemote
        p = new SqliteParameter("@PathRemote", SqliteType.Text) { Value = item.PathRemote ?? "" };
        cmd.Parameters.Add(p);

        // Parameter @Source
        p = new SqliteParameter("@Source", SqliteType.Text) { Value = item.Source ?? "L" };
        cmd.Parameters.Add(p);

        // Parameter @HashCodeNew
        p = new SqliteParameter("@HashCodeNew", SqliteType.Text) { Value = item.HashCodeNew ?? "" };
        cmd.Parameters.Add(p);

        // Parameter @Type
        p = new SqliteParameter("@Type", SqliteType.Text) { Value = item.Type ?? "F" };
        cmd.Parameters.Add(p);

        // Parameter @Size
        p = new SqliteParameter("@Size", SqliteType.Integer) { Value = item.Size };
        cmd.Parameters.Add(p);

        // Parameter @SizeRemote
        p = new SqliteParameter("@SizeRemote", SqliteType.Integer) { Value = item.SizeRemote };
        cmd.Parameters.Add(p);

        //_db.Exec(cmd);

        if (_isCollecting)
        {
            _commands.Add(cmd);
        }
        else
        {
            _db.Exec(cmd);
        }
    }

    /// <summary>
    /// Update a data row in table Files from an entity class Files object
    /// </summary>
    public void Update(FtpFiles item)
    {
        const string sql = "UPDATE \"FtpFiles\" SET \"Path\"=@Path, \"HashCode\"=@HashCode, \"PathRemote\"=@PathRemote, \"Source\"=@Source, \"HashCodeNew\"=@HashCodeNew, \"Type\"=@Type, \"Size\"=@Size, \"SizeRemote\"=@SizeRemote WHERE \"ID\"=@ID; ";

        var cmd = new SqliteCommand(sql);

        SqliteParameter p;

        // Parameter @ID
        p = new SqliteParameter("@ID", SqliteType.Integer) { Value = item.Id };
        cmd.Parameters.Add(p);

        // Parameter @Path
        p = new SqliteParameter("@Path", SqliteType.Text) { Value = item.Path };
        cmd.Parameters.Add(p);

        // Parameter @HashCode
        p = new SqliteParameter("@HashCode", SqliteType.Text) { Value = item.HashCode ?? "" };
        cmd.Parameters.Add(p);

        // Parameter @PathRemote
        p = new SqliteParameter("@PathRemote", SqliteType.Text) { Value = item.PathRemote ?? "" };
        cmd.Parameters.Add(p);

        // Parameter @Source
        p = new SqliteParameter("@Source", SqliteType.Text) { Value = item.Source ?? "L" };
        cmd.Parameters.Add(p);

        // Parameter @HashCodeNew
        p = new SqliteParameter("@HashCodeNew", SqliteType.Text) { Value = item.HashCodeNew ?? "" };
        cmd.Parameters.Add(p);

        // Parameter @Type
        p = new SqliteParameter("@Type", SqliteType.Text) { Value = item.Type ?? "F" };
        cmd.Parameters.Add(p);

        // Parameter @Size
        p = new SqliteParameter("@Size", SqliteType.Integer) { Value = item.Size };
        cmd.Parameters.Add(p);

        // Parameter @SizeRemote
        p = new SqliteParameter("@SizeRemote", SqliteType.Integer) { Value = item.SizeRemote };
        cmd.Parameters.Add(p);

        _db.Exec(cmd);
    }

    /// <summary>
    /// Delete a row from table Files 
    /// </summary>
    public void Delete(long id)
    {
        var cmd = new SqliteCommand("DELETE FROM \"FtpFiles\" WHERE \"ID\" = @PK");

        var p = new SqliteParameter("@PK", SqliteType.Integer) { Value = id };
        cmd.Parameters.Add(p);

        _db.Exec(cmd);
    }

    /// <summary>
    /// Get all rows in table Files
    /// </summary>
    public IList<FtpFiles> GetAll()
    {
        var result = new List<FtpFiles>();

        var reader = _db.GetDataReader("SELECT * FROM \"FtpFiles\"");

        while (reader.Read())
        {
            var dto = DataHelper.MapFromDbToFiles(reader);
            result.Add(dto);

        }

        reader.Dispose();

        return result;
    }

    /// <summary>
    /// Get all rows in table Files
    /// </summary>
    public FtpFiles GetById(long pkId)
    {
        FtpFiles dto = null;

        var reader = _db.GetDataReader($"SELECT * FROM \"FtpFiles\" WHERE \"ID\"={pkId};");

        while (reader.Read())
        {
            dto = DataHelper.MapFromDbToFiles(reader);
            break;

        }

        reader.Dispose();

        return dto;
    }

    /// <summary>
    /// Count all rows in table Files 
    /// </summary>
    public int Count()
    {
        var result = _db.ExecWithResult("SELECT COUNT(*) FROM \"FtpFiles\"");

        return Convert.ToInt32(result);
    }

    public IList<FilePathItem> GetFilePaths()
    {
        var result = new List<FilePathItem>();

        var dr = _db.GetDataReader(
            "SELECT \"Path\" FROM \"FtpFiles\" WHERE \"Type\"='F' AND \"Source\" In ('L', 'B') ORDER BY \"Path\"");

        while (dr.Read())
        {
            var item = new FilePathItem
            {
                Path = dr.GetString(0)
            };

            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Reset existing file data to start point
    /// </summary>
    public void UpdateFiles()
    {
        //var cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"Source\"='U', \"Size\"=0, \"SizeRemote\"=0");
        var cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"Source\"='U'");
        _db.Exec(cmd);
    }

    ///// <summary>
    ///// Check if a local file exists in the database
    ///// </summary>
    ///// <param name="localPath"></param>
    ///// <returns></returns>
    //public int CheckIfFileExists(string localPath)
    //{

    //    var cmd = new SqliteCommand("SELECT count(*) FROM \"FtpFiles\" WHERE \"Path\"=@Path");
    //    var p = new SqliteParameter("@Path", SqliteType.Text) { Value = localPath };
    //    cmd.Parameters.Add(p);

    //    var erg = _db.ExecWithResult(cmd);

    //    return Convert.ToInt32(erg);
    //}

    /// <summary>
    /// Update an existing remote file with its size
    /// </summary>
    /// <param name="file"></param>
    /// <param name="size"></param>
    public void UpdateExistingRemoteFile(FilePathItem file, in long size)
    {
        SqliteCommand cmd;
        SqliteParameter p;

        // Command 1
        if (file.Source == "U")
        {
            cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"Source\"='R', \"SizeRemote\"=@Size  WHERE \"Path\"=@Path AND \"Source\"='U'");
            p = new SqliteParameter("@Path", SqliteType.Text) { Value = file.Path };
            cmd.Parameters.Add(p);

            p = new SqliteParameter("@Size", SqliteType.Integer) { Value = size };
            cmd.Parameters.Add(p);

            if (_isCollecting)
            {
                _commands.Add(cmd);
            }
            else
            {
                _db.Exec(cmd);
            }

            file.SizeRemote = size;
        }


        // Command 2
        if (size != file.SizeRemote)
        {
            cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"SizeRemote\"=@Size WHERE \"Path\"=@Path");
            p = new SqliteParameter("@Path", SqliteType.Text) { Value = file.Path };
            cmd.Parameters.Add(p);

            p = new SqliteParameter("@Size", SqliteType.Integer) { Value = size };
            cmd.Parameters.Add(p);

            if (_isCollecting)
            {
                _commands.Add(cmd);
            }
            else
            {
                _db.Exec(cmd);
            }

        }

    }

    /// <summary>
    /// Update an existing local file with its size
    /// </summary>
    /// <param name="file">File data in the database</param>
    /// <param name="size">New size of the file</param>
    public void UpdateExistingLocalFile(FilePathItem file, in long size)
    {
        SqliteCommand cmd;
        SqliteParameter p;

        // Command 1
        if (file.Source == "U")
        {
            cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"Source\"='L', \"Size\"=@Size WHERE \"Path\"=@Path AND \"Source\"='U'");
            p = new SqliteParameter("@Path", SqliteType.Text) { Value = file.Path };
            cmd.Parameters.Add(p);

            p = new SqliteParameter("@Size", SqliteType.Integer) { Value = size };
            cmd.Parameters.Add(p);

            if (_isCollecting)
            {
                _commands.Add(cmd);
            }
            else
            {
                _db.Exec(cmd);
            }

            file.Size = size;
        }



        // Command 2
        if (file.Source == "R")
        {
            cmd = new SqliteCommand(
                "UPDATE \"FtpFiles\" SET \"Source\"='B', \"Size\"=@Size WHERE \"Path\"=@Path AND \"Source\"='R'");
            p = new SqliteParameter("@Path", SqliteType.Text) { Value = file.Path };
            cmd.Parameters.Add(p);

            p = new SqliteParameter("@Size", SqliteType.Integer) { Value = size };
            cmd.Parameters.Add(p);

            if (_isCollecting)
            {
                _commands.Add(cmd);
            }
            else
            {
                _db.Exec(cmd);
            }

            file.Size = size;
        }

        // Command 3

        if (size != file.Size)
        {
            cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"Size\"=@Size WHERE \"Path\"=@Path");
            p = new SqliteParameter("@Path", SqliteType.Text) { Value = file.Path };
            cmd.Parameters.Add(p);

            p = new SqliteParameter("@Size", SqliteType.Integer) { Value = size };
            cmd.Parameters.Add(p);

            if (_isCollecting)
            {
                _commands.Add(cmd);
            }
            else
            {
                _db.Exec(cmd);
            }
        }
    }

    //public void SetHashForFile(string path, string hash)
    //{
    //    var cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"HashCodeNew\"=@Hash WHERE \"Path\"=@Path");
    //    var p = new SqliteParameter("@Path", SqliteType.Text) { Value = path };
    //    cmd.Parameters.Add(p);

    //    p = new SqliteParameter("@Hash", SqliteType.Text) { Value = hash };
    //    cmd.Parameters.Add(p);

    //    _db.Exec(cmd);
    //}


    public DbCommand SetHashForFileCmd(string path, string hash)
    {
        var cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"HashCodeNew\"=@Hash WHERE \"Path\"=@Path");
        var p = new SqliteParameter("@Path", SqliteType.Text) { Value = path };
        cmd.Parameters.Add(p);

        p = new SqliteParameter("@Hash", SqliteType.Text) { Value = hash };
        cmd.Parameters.Add(p);

        return cmd;
    }



    public void SetHashForRemoteFiles()
    {
        var cmd = new SqliteCommand("UPDATE \"FtpFiles\" SET \"HashCode\"=\"HashCodeNew\" WHERE \"Type=\"='F' AND \"Source\"='B' AND \"HashCode\"=''");
        _db.Exec(cmd);
    }

    public IList<FilePathItem> GetRemoteFilePathToCreate()
    {

        var result = new List<FilePathItem>();

        var dr = _db.GetDataReader(
            "SELECT \"PathRemote\" FROM \"FtpFiles\" WHERE \"Type\"='D' AND \"Source\"='L' ORDER BY \"PathRemote\"");

        while (dr.Read())
        {
            var item = new FilePathItem
            {
                PathRemote = dr.GetString(0)
            };

            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ClearFiles()
    {
        var cmd = new SqliteCommand("DELETE FROM \"FtpFiles\" WHERE \"Source\"='U'");
        _db.Exec(cmd);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IList<FilePathItem> GetRemoteFilePathRemoveFiles()
    {
        var result = new List<FilePathItem>();

        var dr = _db.GetDataReader(
            "SELECT \"PathRemote\" FROM \"FtpFiles\" WHERE \"Type\"='F' AND \"Source\"='R' ORDER BY \"PathRemote\"");

        while (dr.Read())
        {
            var item = new FilePathItem
            {
                PathRemote = dr.GetString(0)
            };

            result.Add(item);
        }

        return result;

    }


    public IList<FilePathItem> GetRemoteFilePathToRemove()
    {

        var result = new List<FilePathItem>();

        var dr = _db.GetDataReader(
            "SELECT \"PathRemote\" FROM \"FtpFiles\" WHERE \"Type\"='D' AND \"Source\"='R' ORDER BY \"PathRemote\" DESC");

        while (dr.Read())
        {
            var item = new FilePathItem
            {
                PathRemote = dr.GetString(0)
            };

            result.Add(item);
        }

        return result;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IList<FilePathItem> GetLocalFilesToCopy()
    {
        var result = new List<FilePathItem>();

        const string sql = "SELECT \"Path\", \"PathRemote\" FROM \"FtpFiles\" WHERE \"Type\"='F'  AND (\"Source\"='L' OR (\"Source\"='B' AND \"Size\"<>\"SizeRemote\") OR \"Hashcode\"<>\"HashCodeNew\") ORDER BY \"Path\"";

        var dr = _db.GetDataReader(sql);

        while (dr.Read())
        {
            var item = new FilePathItem
            {
                Path = dr.GetString(0),
                PathRemote = dr.GetString(1),
                //Path = dr["Path"].ToString(),
                //PathRemote = dr["PathRemote"].ToString()
            };

            result.Add(item);
        }

        return result;

    }

    public void SetHashCodeForRemoteFile(string path)
    {
        var update = new SqliteCommand("UPDATE \"FtpFiles\" SET \"HashCode\"=\"HashCodeNew\", \"HashCodeNew\"='', \"SizeRemote\"=\"Size\" WHERE \"Path\"=@Path");
        var p = new SqliteParameter("@Path", SqliteType.Text) { Value = path };
        update.Parameters.Add(p);

        _db.Exec(update);
    }

    public void Exec(List<DbCommand> cmds)
    {
        _db.ExecMultiple(cmds);
    }

    public void LoadAllFilePaths(IList<FilePathItem> allFiles)
    {
        const string sql = "SELECT \"Path\", \"PathRemote\", \"Source\", \"Size\", \"SizeRemote\" FROM \"FtpFiles\" ORDER BY \"Path\"";

        var dr = _db.GetDataReader(sql);

        while (dr.Read())
        {
            var item = new FilePathItem
            {
                Path = dr.GetString(0),
                PathRemote = dr.GetString(1),
                Source = dr.GetString(2),
                Size = dr.GetInt64(3),
                SizeRemote = dr.GetInt64(4)
                //Path = dr["Path"].ToString(),
                //PathRemote = dr["PathRemote"].ToString()
            };

            allFiles.Add(item);
        }
    }
}