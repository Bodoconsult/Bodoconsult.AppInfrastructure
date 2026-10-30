// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Text;
using BodoCmsConsole.Business.Interfaces;
using BodoCmsConsole.Business.Model;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Drawing.Model;
using Microsoft.Data.SqlClient;

namespace BodoCmsConsole.Business.Handlers;

/// <summary>
/// Encapsulates database access
/// </summary>
public class DatabaseHandler : IDatabaseHandler
{
    private readonly IAppLoggerProxy _logger;
    private readonly IConnManager _db;
    private readonly ConcurrentBag<DbCommand> _commands = new();
    private bool _isCollecting;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="db">Current DB connection manager</param>
    /// <param name="logger">Current app logger</param>
    public DatabaseHandler(IConnManager db, IAppLoggerProxy logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Start command collecting for saving db connections
    /// </summary>
    public void StartCommandCollecting()
    {
        _commands.Clear();
        _isCollecting = true;
    }

    /// <summary>
    /// Run the commands from collection
    /// </summary>
    public void RunCommandsFromCollection()
    {

        var data = _commands.ToList();
        _commands.Clear();

        var result = _db.ExecMultiple(data);

        _isCollecting = false;

        if (result == 0)
        {
            return;
        }

        var sql = data[result];

        _logger.LogError($"Error: SQL: {sql}");

        throw new Exception($"Error processing sql: {sql}");
    }


    /// <summary>
    /// Get new image header files
    /// </summary>
    /// <returns>List with images</returns>
    public IList<ImageData> GetNewHeaderFiles()
    {

        var result = new List<ImageData>();

        //try
        //{

        var dt = _db.GetDataTable("exec dbo.spImages_GetNewHeaderImages");
        foreach (DataRow row in dt.Rows)
        {
            var s = row["I_Path"].ToString();

            if (string.IsNullOrEmpty(s))
            {
                continue;
            }

            var i = new ImageData
            {
                Path = s
            };

            result.Add(i);

        }
        return result;

        //}
        //catch (Exception e)
        //{
        //    Logger.Error("GetNewHeaderFiles:exec dbo.spImages_GetNewHeaderImages", e);
        //    Status(e.Message);
        //    return result;
        //}

    }

    /// <summary>
    ///  Run a SQL statement against the database
    /// </summary>
    /// <param name="sql">SQL statement</param>
    public void Exec(string sql)
    {
        _db.Exec(sql);
    }

    /// <summary>
    /// Exec a SQL statement and get a single string value as the result back
    /// </summary>
    /// <param name="sql">SQL statement</param>
    /// <returns></returns>
    public string ExecWithResult(string sql)
    {
        //try
        //{
        return _db.ExecWithResult(sql);
        //}
        //catch (Exception e)
        //{
        //    Logger.Error("ExecWithResult:" + sql, e);
        //    Status(e.Message);
        //    return null;
        //}
    }

    ///// <summary>
    ///// Exec a command against the database
    ///// </summary>
    ///// <param name="cmd"></param>
    //internal void Exec(SqlCommand cmd)
    //{
    //    try
    //    {
    //        _db.Exec(cmd);
    //    }
    //    catch (Exception e)
    //    {
    //        Logger.Error("ExecWithResult:" + cmd.CommandText, e);
    //        Status(e.Message);
    //        // ToDo: Logging
    //    }
    //}


    /// <summary>
    /// Check if a file exits already in the database
    /// </summary>
    /// <param name="fri">File info</param>
    /// <returns></returns>
    public string CheckFile(FileSystemInfo fri)
    {
        //try
        //{


        var cmd = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFilesUpdate]"
        };

        var p = cmd.Parameters.Add("@F", SqlDbType.VarChar);
        p.Value = fri.FullName;

        p = cmd.Parameters.Add("@D", SqlDbType.DateTime);
        p.Value = fri.LastWriteTime;

        p = cmd.Parameters.Add("@N", SqlDbType.VarChar);
        p.Value = fri.Name.Replace(fri.Extension, "").Replace("_", " ");

        //_db.Exec(cmd);

        return _db.ExecWithResult(cmd);
        //}
        //catch (Exception e)
        //{
        //    Logger.Error("GetDataTable: EXEC.dbo.spFilesUpdate:" + fri.FullName, e);
        //    Status(e.Message);
        //    return fri.Name;
        //}

    }


    //private static void Status(string msg)
    //{

    //    Debug.Print(msg);
    //}


    /// <summary>
    /// Update last change date of the files
    /// </summary>
    /// <param name="date">Date to be set as last update date</param>
    public void UpdateLastChanges(DateTime date)
    {
        var sql = $"update dbo.tLastChanges set o_date='{date:dd.MM.yyyy}' where o_date<'{date:dd.MM.yyyy}'";
        _db.Exec(sql);
    }

    /// <summary>
    /// Replace page codes in the content of the pages
    /// </summary>
    /// <param name="tag">Tag to search, i.e. ??Page1??</param>
    /// <param name="content">Content to place instead of the tag</param>
    public void PagesReplaceContentAll(string tag, string content)
    {
        var cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.spPagesReplaceContentAll" };
        cmd.Parameters.Add("@Was", SqlDbType.NVarChar);
        cmd.Parameters[0].Value = tag;
        cmd.Parameters.Add("@Text", SqlDbType.NVarChar);
        cmd.Parameters[1].Value = content;

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
    /// Get a settings from the settings table
    /// </summary>
    /// <param name="key">Setting key</param>
    /// <returns>Setting value or null</returns>
    public string GetSetting(string key)
    {
        try
        {
            return _db.ExecWithResult($"SELECT Value FROM dbo.Settings WITH (noLock) WHERE (sKey = '{key}')");
        }
        catch
        {
            _logger.LogError($"Error: Key: {key}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Check if an image already exists
    /// </summary>
    /// <param name="fileName">Image full filename</param>
    /// <returns>True if the image already exists else false</returns>
    public bool CheckImageExists(string fileName)
    {
        var cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.spImages_Exists" };

        cmd.Parameters.Add("@TargetPath", SqlDbType.NVarChar);
        cmd.Parameters[0].Value = fileName;

        var erg = _db.ExecWithResult(cmd);

        return erg != "0";
    }

    /// <summary>
    /// Create an image
    /// </summary>
    /// <param name="f">File info</param>
    /// <param name="targetPath">Target file path</param>
    /// <param name="size">Requested size in pixels</param>
    /// <param name="thumbnail">Create a thumbnail?</param>
    /// <param name="slideShow">Add the image to the slideshow?</param>
    public void CreateImage(FileSystemInfo f, string targetPath, ImageSize size, bool thumbnail, bool slideShow = true)
    {
        var cmd1 = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.spImages_New" };

        cmd1.Parameters.Add("@I_GUID", SqlDbType.UniqueIdentifier);
        cmd1.Parameters[0].Value = Guid.NewGuid();

        cmd1.Parameters.Add("@I_Name", SqlDbType.NVarChar);
        cmd1.Parameters[1].Value = f.Name.Replace(f.Extension, "").Replace("_", " ");

        cmd1.Parameters.Add("@I_Path", SqlDbType.NVarChar);
        cmd1.Parameters[2].Value = f.FullName;

        cmd1.Parameters.Add("@I_Width", SqlDbType.Int);
        cmd1.Parameters[3].Value = size.Width;

        cmd1.Parameters.Add("@I_Height", SqlDbType.Int);
        cmd1.Parameters[4].Value = size.Height;

        cmd1.Parameters.Add("@I_TargetPath", SqlDbType.NVarChar);
        cmd1.Parameters[5].Value = targetPath;


        cmd1.Parameters.Add("@I_SlideShow", SqlDbType.Bit);
        cmd1.Parameters[6].Value = slideShow ? 1 : 0;


        cmd1.Parameters.Add("@I_Thumbnail", SqlDbType.Bit);
        cmd1.Parameters[7].Value = thumbnail ? 1 : 0;

        cmd1.Parameters.Add("@I_SubTitle", SqlDbType.NVarChar);
        cmd1.Parameters[8].Value = f.Name.Replace("_", "").Replace(".png", "").Replace(".jpg", "").Replace(".gif", "");

        _db.Exec(cmd1);
    }

    /// <summary>
    /// Delete an image by its ID
    /// </summary>
    /// <param name="id">Image ID</param>
    public void DeleteImage(int id)
    {

        var cmd = new SqlCommand($"delete from dbo.tImages where I_ID={id}");

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
    /// Prepare images
    /// </summary>
    public void PrepareImages()
    {
        _db.Exec("EXEC dbo.spImages_Prepare");
    }

    /// <summary>
    /// Get <see cref="DataTable"/> from the database
    /// </summary>
    /// <param name="sql">SQL which returns <see cref="DataTable"/></param>
    /// <returns><see cref="DataTable"/></returns>
    public DataTable GetDataTable(string sql)
    {
        //try
        //{
        return _db.GetDataTable(sql);
        //}
        //catch (Exception e)
        //{
        //    Logger.Error("GetDataTable:" + sql, e);
        //    Status(e.Message);
        //    return null;
        //}
    }


    /// <summary>
    /// Get images without thumbnail
    /// </summary>
    /// <returns>List with images</returns>
    public IList<ImageData> ImagesWithNoThumbs()
    {
        var result = new List<ImageData>();

        var cmd = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "dbo.spImages_NoThumbs",
        };

        var dt = _db.GetDataTable(cmd);
        foreach (DataRow row in dt.Rows)
        {
            var s = row["I_TargetPath"].ToString();
            
            if (string.IsNullOrEmpty(s))
            {
                continue;
            }

            var i = new ImageData { Path = s };
            result.Add(i);
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Get a <see cref="DbDataReader"/> from the database
    /// </summary>
    /// <param name="sql">SQL statement to run</param>
    /// <returns><see cref="DbDataReader"/> </returns>
    public DbDataReader GetDataReader(string sql)
    {
        return _db.GetDataReader(sql);
    }

    /// <summary>
    /// Get data for an image
    /// </summary>
    /// <param name="targetPath">Image path</param>
    /// <returns>Image data or null</returns>
    public ImageData? GetImageData(string targetPath)
    {
        var cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.spImages_GetData" };

        cmd.Parameters.Add("@TargetPath", SqlDbType.NVarChar);
        cmd.Parameters[0].Value = targetPath;

        var dt = _db.GetDataTable(cmd);

        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            return null;
        }

        var row = dt.Rows[0];

        var i = new ImageData
        {
            Name = row["I_name"].ToString() ?? string.Empty,
            TargetPath = targetPath
        };

        dt.Dispose();
        return i;
    }

    /// <summary>
    /// Update the title of the root directories
    /// </summary>
    /// <param name="homePath">Local homepage path</param>
    public void UpdateRootFolderNames(string homePath)
    {
        var sql = $"UPDATE tFolder SET FO_Name1 = d.D_NameHeader, FO_MainLinkName=d.D_Name FROM tFolder f INNER JOIN tDirectories d  ON f.FO_Name = '{homePath}'+d.D_Name WHERE isnull(d.D_NameHeader,'')<> '' AND FO_Name1 = d.D_Name;";
        _db.Exec(sql);
    }

    /// <summary>
    /// Get image data by a source path
    /// </summary>
    /// <param name="sourcePath">Source path</param>
    /// <returns>Image data</returns>
    public ImageData? GetImageDataBySourcePath(string sourcePath)
    {
        var cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.spImages_GetDataBySourcePath" };

        cmd.Parameters.Add("@SourcePath", SqlDbType.NVarChar);
        cmd.Parameters[0].Value = sourcePath;

        var dt = _db.GetDataTable(cmd);

        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            return null;
        }

        var dr = dt.Rows[0];

        var result = new ImageData
        {
            Path = dr["I_TargetPath"].ToString() ?? string.Empty,
            Name = dr["I_Name"].ToString() ?? string.Empty
        };

        dt.Dispose();
        return result;
    }

    /// <summary>
    /// Get all images for a path
    /// </summary>
    /// <param name="dirPath">directory path of the folder</param>
    /// <returns></returns>
    public IList<ImageData> GetImagesForPath(string dirPath)
    {
        var result = new List<ImageData>();

        var cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.spImages_GetDataByFolder" };
        var p = cmd.Parameters.Add("@Path", SqlDbType.NVarChar);
        p.Value = dirPath;

        var dt = _db.GetDataTable(cmd);

        foreach (DataRow row in dt.Rows)
        {
            var s = row["I_TargetPath"].ToString();

            if (string.IsNullOrEmpty(s))
            {
                continue;
            }

            var i = new ImageData
            {
                Width = Convert.ToInt32(row["I_Width"].ToString()),
                Height = Convert.ToInt32(row["I_Height"].ToString()),
                TargetPath = s,
                Thumbnail = Convert.ToBoolean(row["I_Thumbnail"].ToString()),
                SlideShow = Convert.ToBoolean(row["I_SlideShow"].ToString())
            };

            result.Add(i);
        }

        dt.Dispose();

        return result;
    }


    ///// <summary>
    ///// Prepare page content before start
    ///// </summary>
    //public void PrepareContent()
    //{
    //    const string query3 = "exec dbo.spPagesPrepareContent";
    //    Exec(query3);
    //    const string query2 = "exec dbo.spPagesMasterPageFormat";
    //    Exec(query2);
    //}

    /// <summary>
    /// Get the number of adsets
    /// </summary>
    /// <returns>Current number of adsets</returns>
    public int GetNumberOfAdsets()
    {
        const string query4 = "select count(*) from dbo.tAdSets where as_active=1";
        return Convert.ToInt16(ExecWithResult(query4));
    }

    /// <summary>
    /// Get all adset IDs
    /// </summary>
    /// <returns></returns>
    public IList<int> GetAdsetIds()
    {
        var adSets = new List<int>();
        var dt = GetDataTable("exec dbo.[spAdSets_FetchIds]");

        foreach (DataRow r in dt.Rows)
        {
            adSets.Add(Convert.ToInt16(r[0].ToString()));
        }

        dt.Dispose();

        return adSets;
    }

    /// <summary>
    /// Get all approved folders
    /// </summary>
    /// <returns>List with all approved folder paths</returns>
    public IList<string> GetFoldersWithApproval()
    {
        var result = new List<string>();

        const string query = "EXEC dbo.spDirectories_MitFreigabe";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {
            var s = r["d_name"].ToString();

            if (!string.IsNullOrEmpty(s))
            {
                result.Add(s);
            }
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Get all images to copy
    /// </summary>
    /// <returns></returns>
    public IList<ImageData> GetImagesToCopy()
    {
        var result = new List<ImageData>();

        const string query = "EXEC dbo.spImages_ToCopy";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {
            var s = r["i_path"].ToString();

            if (string.IsNullOrEmpty(s))
            {
                continue;
            }

            var imagefile = new ImageData
            {
                Id = Convert.ToInt32(r["I_ID"].ToString()),
                Path = s
            };

            result.Add(imagefile);
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Get data of all pages to be built
    /// </summary>
    /// <returns></returns>
    public IList<PageData> GetPagesToBuild()
    {
        var result = new List<PageData>();

        const string query = "exec spPagesList";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {
            var pd = new PageData
            {
                Id = Convert.ToInt32(r["P_ID"].ToString()),
                Url = r["P_Url"].ToString()?.Trim() ?? string.Empty,
                Author = r["P_Author"].ToString() ?? string.Empty,
                HeaderImagePath = r["P_Header_Image"].ToString() ?? string.Empty,
                Title = r["P_Name"].ToString() ?? string.Empty,
                Description = r["P_Description"].ToString() ?? string.Empty,
                Content = r["P_ContentFinale"].ToString() ?? string.Empty,
                Keywords = r["P_Keywords"].ToString() ?? string.Empty,
                DateLastChange = (DateTime)r["O_Date"],
                AdSetId = Convert.ToInt32(r["P_AdSetID"].ToString())
            };
            result.Add(pd);
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Get the main links for all webpages
    /// </summary>
    /// <returns></returns>
    public IList<PageData> GetMainLinksForPage()
    {

        var result = new List<PageData>();

        //spPagesMainLinks
        const string query = "EXEC dbo.spPagesMainLinks";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {


            var p = new PageData
            {
                Id = Convert.ToInt32(r["P_Id"].ToString()),
                PageType = r["P_Typ"].ToString() ?? string.Empty,
                Url = r["P_URL"].ToString() ?? string.Empty,
                Name = r["P_Name"].ToString() ?? string.Empty,
                TitleUrl = r["P_TitleURL"].ToString() ?? string.Empty
            };

            result.Add(p);
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Get all links at the bottom of a page
    /// </summary>
    /// <returns></returns>
    public IList<PageData> GetBottomLinksForPages()
    {
        var result = new List<PageData>();

        const string query = "EXEC dbo.spWebsiteMenu 3";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {

            var p = new PageData
            {
                Id = Convert.ToInt32(r["P_Id"].ToString()),
                //                    PageType = dt["P_Typ"].ToString(),
                Url = r["P_URL"].ToString() ?? string.Empty,
                Name = r["P_Name"].ToString() ?? string.Empty,
                TitleUrl = r["P_TitleURL"].ToString() ?? string.Empty
            };

            result.Add(p);
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Get page individual links
    /// </summary>
    /// <param name="pageId">ID of the page</param>
    /// <returns></returns>
    public IList<PageData> GetSubLinksForPage(int pageId)
    {
        var result = new List<PageData>();

        var query = $"exec dbo.spPagesSubLinks {pageId}";// ReadResourceString("BodoCMSCore.Queries.spPagesSubLinks.sql");
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {
            var p = new PageData
            {
                Id = Convert.ToInt32(r["P_Id"].ToString()),
                //                    PageType = dt["P_Typ"].ToString(),
                Url = r["P_URL"].ToString() ?? string.Empty,
                Name = r["P_Name"].ToString() ?? string.Empty,
                TitleUrl = r["P_TitleURL"].ToString() ?? string.Empty,
                Css = r["Css"].ToString() ?? string.Empty

            };

            result.Add(p);
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Get all sitemap elements
    /// </summary>
    /// <param name="sitemapItems">List to fill with all sitemap items</param>
    public void GetSitemapPages(List<SiteMapElement> sitemapItems)
    {
        const string query = "exec dbo.spSitemap";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {
            var s = new SiteMapElement
            {
                Id = (int)r["P_ID"],
                Name = r["P_Name"].ToString()?.Trim(),
                TitleUrl = r["P_TitleUrl"].ToString(),
                Url = r["P_Url"].ToString()?.Trim(),
                Prio = (double)r["P_Sitemap_PRIO"],
                Freq = r["P_Sitemap_FREQ"].ToString()
            };
            sitemapItems.Add(s);
        }

        dt.Dispose();
    }

    /// <summary>
    /// Get other directories
    /// </summary>
    /// <returns>List with data of other directories</returns>
    public IList<FolderData> GetOtherDirectories()
    {

        var result = new List<FolderData>();

        const string query = "EXEC dbo.spDirectories_MitFreigabe";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {
            var folder = new FolderData
            {
                Name = r["d_name"].ToString() ?? string.Empty,
                Subdomain = Convert.ToBoolean(r["D_SubDomain"].ToString()),
                Sitemap = Convert.ToBoolean(r["D_Sitemap"].ToString()),
                HeaderRemoveNumbers = Convert.ToBoolean(r["D_HeaderRemoveNumbers"].ToString())
            };

            result.Add(folder);
        }

        return result;
    }

    /// <summary>
    /// Get all adsets
    /// </summary>
    /// <returns>List with adsets</returns>
    public IList<AdSetData> GetAdsets()
    {

        var result = new List<AdSetData>();

        const string query = "EXEC dbo.spAdsets_FetchAll";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {
            var ads = new AdSetData
            {
                Id = Convert.ToInt32(r["AS_ID"].ToString()),

                W1Url = r["AS_W1_Url"].ToString(),
                W1Image = r["AS_W1_Image"].ToString(),

                W2Url = r["AS_W2_Url"].ToString(),
                W2Image = r["AS_W2_Image"].ToString(),
            };

            result.Add(ads);
        }
        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Get a list of pages
    /// </summary>
    /// <returns>List with page data</returns>
    public IList<PageData> GetPagesList()
    {

        var result = new List<PageData>();


        const string query = "exec dbo.spPagesList"; //"select t.*, c.o_date , pp.PP_ContentFinale as P_ContentFinale, isnull(p_nameurl, p_name) as P_link from tpages t inner join tPagesPrepare pp on t.p_id=pp.pp_p_id left join tLastChanges c on t.p_guid=c.o_o_id where p_type='P' order by p_id";
        var dt = GetDataTable(query);
        foreach (DataRow r in dt.Rows)
        {
            var p = new PageData
            {
                Id = Convert.ToInt32(r["P_ID"].ToString()),
                Url = r["P_URL"].ToString() ?? string.Empty,
                Link = r["P_Link"].ToString() ?? string.Empty
            };

            result.Add(p);
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Do an init
    /// </summary>
    public void DoInit()
    {
        const string query3 = "exec dbo.spPagesPrepareContent; ";
        //Exec(query3);

        const string query2 = "exec dbo.spPagesMasterPageFormat; ";
        //Exec(query2);

        const string query4 = "EXEC [dbo].[spFilesUpdateStart];";
        Exec(query3 + query2 + query4);
    }

    /// <summary>
    /// Get all images to check its existence
    /// </summary>
    /// <returns>List with image data</returns>
    public IList<ImageData> GetImagesToCheck()
    {
        var result = new List<ImageData>();

        var dt = GetDataTable("EXEC dbo.spImages_ToCheck");

        foreach (DataRow row in dt.Rows)
        {
            var i = new ImageData
            {
                Id = Convert.ToInt32(row["i_id"].ToString()),
                Path = row["i_path"].ToString() ?? string.Empty,
                TargetPath = row["i_TargetPath"].ToString() ?? string.Empty,
                Thumbnail = Convert.ToBoolean(row["i_Thumbnail"].ToString()),
                Width = Convert.ToInt32(row["I_WIDTH"].ToString()),
                Height = Convert.ToInt32(row["I_Height"].ToString()),
                Name = row["I_Name"].ToString() ?? string.Empty
            };

            result.Add(i);
        }

        dt.Dispose();

        return result;
    }

    /// <summary>
    /// Check a folder in the database
    /// </summary>
    public void CheckFolder(FileSystemInfo dir)
    {
        //try
        //{
        var cmd = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFolder_New]"
        };

        var p = cmd.Parameters.Add("@F", SqlDbType.VarChar);
        p.Value = dir.FullName;

        p = cmd.Parameters.Add("@N", SqlDbType.VarChar);
        p.Value = dir.Name;

        if (_isCollecting)
        {
            _commands.Add(cmd);
        }
        else
        {
            _db.Exec(cmd);
        }

        //}
        //catch (Exception ex)
        //{
        //    Logger.Error("CheckFolder", ex);
        //}
    }

    /// <summary>
    /// Get folder data
    /// </summary>
    /// <param name="path">Full path of the folder</param>
    /// <returns>Folder data</returns>
    public FolderData? GetFolder(string path)
    {
        using var dt = _db.GetDataTable($"EXEC dbo.spFolder_Fetch '{path}'");
        if (dt.Rows.Count == 0)
        {
            return null;
        }

        var r = dt.Rows[0];
        var fo = new FolderData
        {
            Name = r["FO_name1"].ToString() ?? string.Empty,
            DisplayName = r["FO_MainLinkName"].ToString() ?? string.Empty,
            Summary = Convert.ToBoolean(r["FO_Summary"].ToString()),
            Keywords = r["FO_Keywords"].ToString() ?? string.Empty,
            Description = r["FO_Description"].ToString() ?? string.Empty,
            HeaderImage = r["HeaderImage"].ToString() ?? string.Empty,
            Path = path,
        };


        if (string.IsNullOrEmpty(fo.DisplayName))
        {
            fo.DisplayName = fo.Name;
        }

        if (!string.IsNullOrEmpty(r["FO_AdSetId"].ToString()))
        {
            fo.AdSetId = Convert.ToInt32(r["FO_AdSetId"].ToString());
        }

        return fo;
    }

    /// <summary>
    /// Get the display name for a folder
    /// </summary>
    /// <param name="path">full folder path</param>
    /// <returns>Dislay name</returns>
    public string GetDisplayName(string path)
    {
        return _db.ExecWithResult($"EXEC dbo.spFiles_GetName1 '{path}'");
    }

    /// <summary>
    /// Check if a directory is not required anymore
    /// </summary>
    /// <param name="path">Directory path</param>
    /// <returns>True if the directory is not required anymore</returns>
    public bool IsCheckDirectoryNotRequired(string path)
    {
        return ExecWithResult($"exec dbo.spDirectories_CheckDirectories '{path}'") == "1";
    }

    /// <summary>
    /// Check if an image is not required anymore
    /// </summary>
    /// <param name="path">Image path</param>
    /// <returns>True if the image is not required anymore</returns>
    public bool IsImageCheckNotRequired(string path)
    {
        return ExecWithResult($"exec dbo.spImages_CheckNeeded '{path}'") == "1";
    }

    /// <summary>
    /// Get the maximum page ID
    /// </summary>
    /// <returns>Maximum page ID</returns>
    public int GetPagesMaxId()
    {
        return Convert.ToInt32(ExecWithResult("dbo.spPages_MaxID"));
    }

    /// <summary>
    /// The page exists in the database?
    /// </summary>
    /// <param name="pageId">Page ID</param>
    /// <returns></returns>
    public bool IsPage(int pageId)
    {
        return ExecWithResult($"exec dbo.spPages_Exist {pageId}") != "0";
    }

    /// <summary>
    /// Get the display name for a folder
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetFolderDisplayName(string path)
    {
        return ExecWithResult($"EXEC dbo.spFolder_GetName1 '{path}'");
    }

    /// <summary>
    /// Get the link name of a folder
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetFolderLinkName(string path)
    {
        return _db.ExecWithResult($"EXEC dbo.spFolder_GetName2 '{path}'");
    }

    /// <summary>
    /// Update all change images in the database
    /// </summary>
    /// <param name="images">List with images to update</param>
    public void UpdateChangedImages(IEnumerable<ImageData> images)
    {
        var data = images.Where(x => x.IsChanged).ToList();

        if (!data.Any()) return;

        var sql = new StringBuilder();

        foreach (var image in data)
        {
            sql.Append($"update tImages set I_SlideShow=0, I_Thumbnail=0 where I_TargetPath='{image.TargetPath}';");
        }

        Exec(sql.ToString());
    }

    /// <summary>
    /// Send an error mail to the admin
    /// </summary>
    /// <param name="to">Receiver email address</param>
    /// <param name="subject">subject of the mail</param>
    /// <param name="message">message body of the mail</param>
    public void SendMail(string to, string subject, string message)
    {

#if DEBUG
        to = "robert.leisner@bodoconsult.de";
        //Debug.WriteLine("MailTo: "+to);
        //Debug.WriteLine("Subject: "+subject);
        //Debug.WriteLine("Body: " + message);
        //return;
#endif

        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "dbo.[spMAIL_New]"
        };
        lo.Parameters.Add("@To", SqlDbType.VarChar);
        lo.Parameters[0].Value = to;
        lo.Parameters.Add("@Subject", SqlDbType.VarChar);
        lo.Parameters[1].Value = subject;
        lo.Parameters.Add("@Msg", SqlDbType.VarChar);
        lo.Parameters[2].Value = message;
        _db.Exec(lo);
    }
}