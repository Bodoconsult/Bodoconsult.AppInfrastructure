// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.Text;

namespace Bodoconsult.Text.Interfaces;

/// <summary>
/// Interface for text based <see cref="IDocumentRenderer"/> implementations
/// </summary>
public interface ITextDocumentRenderer: IDocumentRenderer
{

    /// <summary>
    /// Images to store to target path
    /// </summary>
    public Dictionary<string, string> Images { get;  }


    /// <summary>
    /// Template to place the structered text. Must contain placeholder {0} for the structured text
    /// </summary>
    string Template { get; set; }

    /// <summary>
    /// The current content
    /// </summary>

    StringBuilder Content { get; set; }

    /// <summary>
    /// Current text renderer element factory
    /// </summary>
    ITextRendererElementFactory TextRendererElementFactory { get; }

    /// <summary>
    /// Get the fully rendered text
    /// </summary>
    /// <returns>Rendered text</returns>
    string GetRenderedText();

    /// <summary>
    /// Check the content for tags to replace
    /// </summary>
    /// <param name="content">Content</param>
    /// <returns>Checked content string</returns>
    string CheckContent(string content);

    /// <summary>
    /// Register an image file for later copying
    /// </summary>
    /// <param name="imagePath">Image path</param>
    /// <returns>Image filename without path</returns>
    string RegisterImage(string imagePath);

}