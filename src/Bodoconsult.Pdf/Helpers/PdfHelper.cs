// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bodoconsult.Pdf.Helpers;

/// <summary>
/// Basic tools for basic PDF handling
/// </summary>
public static class PdfHelper
{
    /// <summary>
    /// Default ctor
    /// </summary>
    static PdfHelper()
    {
        PdfSharpLicense.SetAgplLicense();
    }

    /// <summary>
    /// Merge some PDF files into a target PDF file
    /// </summary>
    /// <param name="targetFileName">Full path of the target PDF file. Existing file will be overridden</param>
    /// <param name="pdfFilesToMerge">List of paths of all PDF files to merge</param>
    /// <param name="deleteSourceFiles">Delete the source files after successful creation of the merged PDF</param>
    public static void MergePdfs(string targetFileName, List<string> pdfFilesToMerge, bool deleteSourceFiles = true)
    {
        var outputPdfDocument = new PdfDocument();
        foreach (var pdfFile in pdfFilesToMerge)
        {
            var inputPdfDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
            outputPdfDocument.Version = inputPdfDocument.Version;
            foreach (var page in inputPdfDocument.Pages)
            {
                outputPdfDocument.AddPage(page);
            }
            inputPdfDocument.Close();
        }
        outputPdfDocument.Save(targetFileName);
        outputPdfDocument.Close();

        if (!deleteSourceFiles)
        {
            return;
        }

        foreach (var fileName in pdfFilesToMerge)
        {
            if (!File.Exists(fileName))
            {
                continue;
            }

            try
            {
                File.Delete(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }


}