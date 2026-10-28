// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Sitemap element
/// </summary>
public struct SiteMapElement
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id;

    /// <summary>
    /// URL
    /// </summary>
    public string? Url;

    /// <summary>
    /// Name
    /// </summary>
    public string? Name;

    /// <summary>
    /// Titel for the URL
    /// </summary>
    public string? TitleUrl;

    /// <summary>
    /// Priority
    /// </summary>
    public double? Prio;

    /// <summary>
    /// Frequency
    /// </summary>
    public string? Freq;
}