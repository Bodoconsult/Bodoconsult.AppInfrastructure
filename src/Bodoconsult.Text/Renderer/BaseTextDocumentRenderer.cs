// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Text;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer;

/// <summary>
/// Base implementation of a <see cref="IDocumentRenderer"/> for text based output like TXT, MD or HTML
/// </summary>
public class BaseTextDocumentRenderer : BaseDocumentRenderer, ITextDocumentRenderer
{

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="document">Document to render</param>
    public BaseTextDocumentRenderer(Document document) : base(document)
    { }

    /// <summary>
    /// Is the rendering of the styles required
    /// </summary>
    public bool IsRenderingStylesRequired { get; set; } = true;

    /// <summary>
    /// Images to store to target path
    /// </summary>
    public Dictionary<string, string> Images { get; } = new();

    /// <summary>
    /// Template to place the structered text. Must contain placeholder {0} for the structured text
    /// </summary>
    public string Template { get; set; } = "{0}";

    /// <summary>
    /// The current content
    /// </summary>

    public StringBuilder Content { get; set; } = new();

    /// <summary>
    /// Current text renderer element factory
    /// </summary>
    public ITextRendererElementFactory TextRendererElementFactory { get; protected set; }

    /// <summary>
    /// Render the document
    /// </summary>
    public override void RenderIt()
    {
        var rendererElement = TextRendererElementFactory.CreateInstance(Document);
        rendererElement.RenderIt(this);
    }

    /// <summary>
    /// Get the fully rendered text
    /// </summary>
    /// <returns>Rendered text</returns>
    public virtual string GetRenderedText()
    {
        var content = Template.Replace("{0}", Content.ToString());
        return content;
    }

    /// <summary>
    /// Register an image file for later copying
    /// </summary>
    /// <param name="imagePath">Image path</param>
    /// <returns>Image filename without path</returns>
    public string RegisterImage(string imagePath)
    {
        var fi = new FileInfo(imagePath);

        if (!fi.Exists)
        {
            return null;
        }
        Images.TryAdd(fi.Name, imagePath);
        return fi.Name;

    }

    /// <summary>
    /// Save the rendered document as file
    /// </summary>
    /// <param name="fileName">Full file path. Existing file will be overwritten</param>
    public override void SaveAsFile(string fileName)
    {

        var fi = new FileInfo(fileName);

        // Copy images to target dir
        foreach (var image in Images)
        {
            var target = Path.Combine(fi.DirectoryName ?? "", image.Key);

            if (File.Exists(target))
            {
                File.Delete(target);
            }

            File.Copy(image.Value, target, true);
        }

        // Now save the file
        File.WriteAllText(fileName, GetRenderedText());
    }
}