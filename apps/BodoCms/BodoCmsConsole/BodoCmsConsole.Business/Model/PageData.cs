// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Page data
/// </summary>
public class PageData
{
    /// <summary>
    /// URL
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Page ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Path to the header image
    /// </summary>
    public string HeaderImagePath { get; set; } = string.Empty;

    /// <summary>
    /// Author
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Title URL
    /// </summary>
    public string TitleUrl { get; set; } = string.Empty;

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Page content
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Keywords
    /// </summary>
    public string Keywords { get; set; } = string.Empty;

    /// <summary>
    /// Date of the last change
    /// </summary>
    public DateTime DateLastChange { get; set; }

    /// <summary>
    /// Ad set ID
    /// </summary>
    public int AdSetId { get; set; }

    /// <summary>
    /// CSS style
    /// </summary>
    public string Css { get; set; } = string.Empty;

    /// <summary>
    /// Page type
    /// </summary>
    public string PageType { get; set; } = string.Empty;

    /// <summary>
    /// Page name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Page link
    /// </summary>
    public string Link { get; set; } = string.Empty;    
}