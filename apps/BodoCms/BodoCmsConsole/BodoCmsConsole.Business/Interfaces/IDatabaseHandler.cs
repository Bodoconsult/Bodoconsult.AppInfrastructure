// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Data;
using System.Data.Common;
using BodoCmsConsole.Business.Model;
using Bodoconsult.Drawing.Model;

namespace BodoCmsConsole.Business.Interfaces;

/// <summary>
/// Interface for data handler implementations providing data from BodoCMS database
/// </summary>
public interface IDatabaseHandler
{
    /// <summary>
    /// Start command collecting for saving db connections
    /// </summary>
    void StartCommandCollecting();

    /// <summary>
    /// Run the commands from collection
    /// </summary>
    void RunCommandsFromCollection();

    /// <summary>
    /// Get new image header files
    /// </summary>
    /// <returns>List with images</returns>
    IList<ImageData> GetNewHeaderFiles();

    /// <summary>
    ///  Run a SQL statement against the database
    /// </summary>
    /// <param name="sql">SQL statement</param>
    void Exec(string sql);

    /// <summary>
    /// Exec a SQL statement and get a single string value as the result back
    /// </summary>
    /// <param name="sql">SQL statement</param>
    /// <returns>Result as string</returns>
    string ExecWithResult(string sql);

    /// <summary>
    /// Check if a file exits already in the database
    /// </summary>
    /// <param name="fri">File info</param>
    /// <returns></returns>
    string CheckFile(FileSystemInfo fri);

    /// <summary>
    /// Update last change date of the files
    /// </summary>
    /// <param name="date">Date to be set as last update date</param>
    void UpdateLastChanges(DateTime date);

    /// <summary>
    /// Replace page codes in the content of the pages
    /// </summary>
    /// <param name="tag">Tag to search, i.e. ??Page1??</param>
    /// <param name="content">Content to place instead of the tag</param>
    void PagesReplaceContentAll(string tag, string content);

    /// <summary>
    /// Get a settings from the settings table
    /// </summary>
    /// <param name="key">Setting key</param>
    /// <returns>Setting value or null</returns>
    string GetSetting(string key);

    /// <summary>
    /// Check if an image already exists
    /// </summary>
    /// <param name="fileName">Image full filename</param>
    /// <returns>True if the image already exists else false</returns>
    bool CheckImageExists(string fileName);

    /// <summary>
    /// Create an image
    /// </summary>
    /// <param name="f">File info</param>
    /// <param name="targetPath">Target file path</param>
    /// <param name="size">Requested size in pixels</param>
    /// <param name="thumbnail">Create a thumbnail?</param>
    /// <param name="slideShow">Add the image to the slideshow?</param>
    void CreateImage(FileSystemInfo f, string targetPath, ImageSize size, bool thumbnail, bool slideShow = true);

    /// <summary>
    /// Delete an image by its ID
    /// </summary>
    /// <param name="id">Image ID</param>
    void DeleteImage(int id);

    /// <summary>
    /// Prepare images
    /// </summary>
    void PrepareImages();

    /// <summary>
    /// Get <see cref="DataTable"/> from the database
    /// </summary>
    /// <param name="sql">SQL which returns <see cref="DataTable"/></param>
    /// <returns><see cref="DataTable"/></returns>
    DataTable GetDataTable(string sql);

    /// <summary>
    /// Get images without thumbnail
    /// </summary>
    /// <returns>List with images</returns>
    IList<ImageData> ImagesWithNoThumbs();

    /// <summary>
    /// Get a <see cref="DbDataReader"/> from the database
    /// </summary>
    /// <param name="sql">SQL statement to run</param>
    /// <returns><see cref="DbDataReader"/> </returns>
    DbDataReader GetDataReader(string sql);

    /// <summary>
    /// Get data for an image
    /// </summary>
    /// <param name="targetPath">Image path</param>
    /// <returns>Image data or null</returns>
    ImageData? GetImageData(string targetPath);

    /// <summary>
    /// Update the title of the root directories
    /// </summary>
    /// <param name="homePath">Local homepage path</param>
    void UpdateRootFolderNames(string homePath);

    /// <summary>
    /// Get image data by a source path
    /// </summary>
    /// <param name="sourcePath">Source path</param>
    /// <returns>Image data</returns>
    ImageData? GetImageDataBySourcePath(string sourcePath);

    /// <summary>
    /// Get all images for a path
    /// </summary>
    /// <param name="dirPath">directory path of the folder</param>
    /// <returns></returns>
    IList<ImageData> GetImagesForPath(string dirPath);

    /// <summary>
    /// Get the number of adsets
    /// </summary>
    /// <returns>Current number of adsets</returns>
    int GetNumberOfAdsets();

    /// <summary>
    /// Get all adset IDs
    /// </summary>
    /// <returns></returns>
    IList<int> GetAdsetIds();

    /// <summary>
    /// Get all approved folders
    /// </summary>
    /// <returns>List with all approved folder paths</returns>
    IList<string> GetFoldersWithApproval();

    /// <summary>
    /// Get all images to copy
    /// </summary>
    /// <returns></returns>
    IList<ImageData> GetImagesToCopy();

    /// <summary>
    /// Get data of all pages to be built
    /// </summary>
    /// <returns></returns>
    IList<PageData> GetPagesToBuild();

    /// <summary>
    /// Get the main links for all webpages
    /// </summary>
    /// <returns></returns>
    IList<PageData> GetMainLinksForPage();

    /// <summary>
    /// Get all links at the bottom of a page
    /// </summary>
    /// <returns></returns>
    IList<PageData> GetBottomLinksForPages();

    /// <summary>
    /// Get page individual links
    /// </summary>
    /// <param name="pageId">ID of the page</param>
    /// <returns></returns>
    IList<PageData> GetSubLinksForPage(int pageId);

    /// <summary>
    /// Get all sitemap elements
    /// </summary>
    /// <param name="sitemapItems">List to fill with all sitemap items</param>
    void GetSitemapPages(List<SiteMapElement> sitemapItems);

    /// <summary>
    /// Get other directories
    /// </summary>
    /// <returns>List with data of other directories</returns>
    IList<FolderData> GetOtherDirectories();

    /// <summary>
    /// Get all adsets
    /// </summary>
    /// <returns>List with adsets</returns>
    IList<AdSetData> GetAdsets();

    /// <summary>
    /// Get a list of pages
    /// </summary>
    /// <returns>List with page data</returns>
    IList<PageData> GetPagesList();

    /// <summary>
    /// Do an init
    /// </summary>
    void DoInit();

    /// <summary>
    /// Get all images to check its existence
    /// </summary>
    /// <returns>List with image data</returns>
    IList<ImageData> GetImagesToCheck();

    /// <summary>
    /// Check a folder in the database
    /// </summary>
    void CheckFolder(FileSystemInfo dir);

    /// <summary>
    /// Get folder data
    /// </summary>
    /// <param name="path">Full path of the folder</param>
    /// <returns>Folder data</returns>
    FolderData? GetFolder(string path);

    /// <summary>
    /// Get the display name for a folder
    /// </summary>
    /// <param name="path">full folder path</param>
    /// <returns>Dislay name</returns>
    string GetDisplayName(string path);

    /// <summary>
    /// Check if a directory is not required anymore
    /// </summary>
    /// <param name="path">Directory path</param>
    /// <returns>True if the directory is not required anymore</returns>
    bool IsCheckDirectoryNotRequired(string path);

    /// <summary>
    /// Check if an image is not required anymore
    /// </summary>
    /// <param name="path">Image path</param>
    /// <returns>True if the image is not required anymore</returns>
    bool IsImageCheckNotRequired(string path);

    /// <summary>
    /// Get the maximum page ID
    /// </summary>
    /// <returns>Maximum page ID</returns>
    int GetPagesMaxId();

    /// <summary>
    /// The page exists in the database?
    /// </summary>
    /// <param name="pageId">Page ID</param>
    /// <returns>True if the page exists else false</returns>
    bool IsPage(int pageId);

    /// <summary>
    /// Get the display name for a folder
    /// </summary>
    /// <param name="path">Folder path</param>
    /// <returns>Display name of the folder</returns>
    string GetFolderDisplayName(string path);

    /// <summary>
    /// Get the link name of a folder
    /// </summary>
    /// <param name="path">Folder path</param>
    /// <returns>Link name to use for the folder</returns>
    string GetFolderLinkName(string path);

    /// <summary>
    /// Update all change images in the database
    /// </summary>
    /// <param name="images">List with images to update</param>
    void UpdateChangedImages(IEnumerable<ImageData> images);

    /// <summary>
    /// Send an error mail to the admin
    /// </summary>
    /// <param name="to">Receiver email address</param>
    /// <param name="subject">subject of the mail</param>
    /// <param name="message">message body of the mail</param>
    void SendMail(string to, string subject, string message);
}