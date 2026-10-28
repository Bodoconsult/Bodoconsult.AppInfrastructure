// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using BodoCmsConsole.Business.Model;
using Bodoconsult.App.Abstractions.Delegates;

namespace BodoCmsConsole.Business.Interfaces;

/// <summary>
/// Interface for website creation class
/// </summary>
public interface IWebsiteBuilder : IDisposable
{
    /// <summary>
    /// Current database handler
    /// </summary>
    IDatabaseHandler DatabaseHandler { get; }

    /// <summary>
    /// Current state message handler
    /// </summary>
    StatusMessageDelegate StatusMessageDelegate { get; set; }

    /// <summary>
    /// Current web page settings
    /// </summary>
    WebPageSettings WebPageSettings { get; set; }

    /// <summary>
    /// Init the creation process
    /// </summary>
    void Init();

    /// <summary>
    /// Load theme from theme folder
    /// </summary>
    void LoadTheme();

    /// <summary>
    /// Create the basic required directories for the homepage
    /// </summary>
    void PrepareDirectories();

    /// <summary>
    /// Copy the required base files
    /// </summary>
    void CopyBaseFiles();

    /// <summary>
    /// Clear thumbnails
    /// </summary>
    void ClearThumbs();

    /// <summary>
    /// Find images
    /// </summary>
    /// <returns>Number of found images</returns>
    int FindImages();

    /// <summary>
    /// Check if all images in the database are still existing
    /// </summary>
    void ClearImages();

    /// <summary>
    /// Prepare the images
    /// </summary>
    void PrepareImages();

    /// <summary>
    /// Copy images to target directory if needed
    /// </summary>
    void CopyImages();

    /// <summary>
    /// Build pages from database
    /// </summary>
    void BuildPages();

    /// <summary>
    /// Get the link for a header image file
    /// </summary>
    /// <param name="headerFileName"></param>
    /// <param name="relative">relative address path</param>
    /// <returns>HTML link for header image</returns>
    string GetHeaderFileLink(string headerFileName, string relative = "");

    /// <summary>
    /// Get a filename for a page by its ID
    /// </summary>
    /// <param name="pageId">Requested page ID</param>
    /// <returns>Filename of the page</returns>
    string GetFileName(int pageId);

    /// <summary>
    /// Get the sublinks for a  page by its ID
    /// </summary>
    /// <param name="pageId">Requested page ID</param>
    /// <returns>Sublinks of the page</returns>
    string GetSubLinksForPage(int pageId);

    /// <summary>
    /// Create sitemap
    /// </summary>
    void Sitemap();

    /// <summary>
    /// Build pages for subdirectories
    /// </summary>
    int OtherDirectories();

    /// <summary>
    /// Add subfolders to the homepage
    /// </summary>
    /// <param name="subDirectories">List of subfolders</param>
    /// <param name="dir">Current directory</param>
    /// <param name="removeNumbers">Remove leading numbers</param>
    void AddSubFolders(IList<SubfolderData> subDirectories, DirectoryInfo dir, bool removeNumbers);

    /// <summary>
    /// Show website after creation
    /// </summary>
    void Show();

    /// <summary>
    /// Remove old pages from homepage directory
    /// </summary>
    void ClearOldPages();

    /// <summary>
    /// Remove images not required anymore
    /// </summary>
    /// <param name="dir">Directory to check</param>
    void CheckImagesAtTargetPath(DirectoryInfo dir);

    /// <summary>
    ///  Check the header files and add it to the images
    /// </summary>
    void CheckNewHeaderFiles();

    /// <summary>
    /// Prepare the pages for usage
    /// </summary>
    void PreparePages();

    /// <summary>
    /// Get the URLs of the sublinks for a ceratin folder
    /// </summary>
    /// <param name="dir">Current folder</param>
    /// <param name="removeNumbers">Remove leading numbers</param>
    /// <param name="level">Current level</param>
    void GetSubLinkUrls(WebPageFolder dir, bool removeNumbers, int level);

    /// <summary>
    /// Send an error mail
    /// </summary>
    void SendErrorMail();
}