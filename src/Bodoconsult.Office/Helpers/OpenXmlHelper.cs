// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Bodoconsult.Office.Helpers;

// https://learn.microsoft.com/en-us/office/open-xml/word/how-to-validate-a-word-processing-document?tabs=cs

/// <summary>
/// Helper class for OpenXml documents
/// </summary>
public static class OpenXmlHelper
{

    private static readonly List<string> ExcludedDescriptions =
        ["The 'http://schemas.openxmlformats.org/wordprocessingml/2006/main:start' attribute is not declared.",
        "The element has unexpected child element 'http://schemas.openxmlformats.org/wordprocessingml/2006/main:tblBorders'.",
        "The element has unexpected child element 'http://schemas.openxmlformats.org/wordprocessingml/2006/main:left'.",
        "The element has unexpected child element 'http://schemas.openxmlformats.org/wordprocessingml/2006/main:b'.",
        "The element has unexpected child element 'http://schemas.openxmlformats.org/wordprocessingml/2006/main:keepLines'.",
        "The element has unexpected child element 'http://schemas.openxmlformats.org/wordprocessingml/2006/main:pPr'.",
        "The element has invalid child element 'http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing:wrapTopAndBottom'. List of possible elements expected: <http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing:docPr>."];


    /// <summary>
    /// Validate an OpenXML DOCX document
    /// </summary>
    /// <param name="filepath">Full filepath</param>
    public static void ValidateWordDocument(string filepath)
    {
        if (!Debugger.IsAttached)
        {
            return;
        }

        using var wordprocessingDocument = WordprocessingDocument.Open(filepath, true);
        try
        {
            var validator = new OpenXmlValidator();
            var count = 0;
            foreach (var error in validator.Validate(wordprocessingDocument))
            {

                if (ExcludedDescriptions.Contains(error.Description))
                {
                    continue;
                }

                count++;
                Console.WriteLine($"Error {count}");
                Console.WriteLine($"Description: {error.Description}");
                Console.WriteLine($"ErrorType: {error.ErrorType}");
                Console.WriteLine($"Node: {error.Node}");
                if (error.Path is not null)
                {
                    Console.WriteLine($"Path: {error.Path.XPath}");
                }
                if (error.Part is not null)
                {
                    Console.WriteLine($"Part: {error.Part.Uri}");
                }
                Console.WriteLine("-------------------------------------------");
            }

            Console.WriteLine("count={0}", count);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    /// <summary>
    /// Validate a corrupted OpenXML DOCX document
    /// </summary>
    /// <param name="filepath"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void ValidateCorruptedWordDocument(string filepath)
    {
        if (!Debugger.IsAttached)
        {
            return;
        }

        // Insert some text into the body, this would cause Schema Error
        using var wordprocessingDocument = WordprocessingDocument.Open(filepath, true);
        if (wordprocessingDocument.MainDocumentPart is null || wordprocessingDocument.MainDocumentPart.Document.Body is null)
        {
            throw new ArgumentNullException(nameof(wordprocessingDocument.MainDocumentPart));
        }

        // Insert some text into the body, this would cause Schema Error
        var body = wordprocessingDocument.MainDocumentPart.Document.Body;
        var run = new Run(new Text("some text"));
        body.Append(run);

        try
        {
            var validator = new OpenXmlValidator();
            var count = 0;
            foreach (var error in validator.Validate(wordprocessingDocument))
            {
                if (ExcludedDescriptions.Contains(error.Description))
                {
                    continue;
                }

                count++;
                Console.WriteLine($"Error {count}");
                Console.WriteLine($"Description: {error.Description}");
                Console.WriteLine($"ErrorType: {error.ErrorType}");
                Console.WriteLine($"Node: {error.Node}");
                if (error.Path is not null)
                {
                    Console.WriteLine($"Path: {error.Path.XPath}");
                }
                if (error.Part is not null)
                {
                    Console.WriteLine($"Part: {error.Part.Uri}");
                }
                Console.WriteLine("-------------------------------------------");
            }

            Console.WriteLine("count={0}", count);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}