// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Data of an advertising set
/// </summary>
public class AdSetData
{
    /// <summary>
    /// Adset ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Image 1 target url
    /// </summary>
    public string? W1Url { get; set; }

    /// <summary>
    /// Image 1 image path
    /// </summary>
    public string? W1Image { get; set; }

    /// <summary>
    /// Image 2 target url
    /// </summary>
    public string? W2Url { get; set; }

    /// <summary>
    /// Image 2 image path
    /// </summary>
    public string? W2Image { get; set; }
}