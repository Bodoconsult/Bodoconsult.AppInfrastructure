// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Reflection;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Helper class for file handling in JobExecuter
/// </summary>
public class FileHelper
{
    private static string _appPath;

    /// <summary>
    /// Get template file as string
    /// </summary>
    /// <param name="templateName">template file name with extension</param>
    /// <returns>template content</returns>
    public static string GetTemplate(string templateName)
    {
        try
        {
            // ReSharper disable once AssignNullToNotNullAttribute

            var dir = GetAppPath();

            templateName = Path.Combine(dir, $"Templates\\{templateName}.txt");

            var fsIn = new FileStream(templateName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sr = new StreamReader(fsIn);
            var s = sr.ReadToEnd();
            sr.Dispose();
            fsIn.Close();
            return s;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Get the content of a text file
    /// </summary>
    /// <param name="fileName">Full path to a text file</param>
    /// <returns></returns>
    public static string GetTextFile(string fileName)
    {
        try
        {
            var fsIn = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sr = new StreamReader(fsIn);
            var s = sr.ReadToEnd();
            sr.Dispose();
            fsIn.Close();
            return s;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Copy a template file to a target path
    /// </summary>
    /// <param name="templateName">template name with extension</param>
    /// <param name="targetPath"></param>
    public static void CopyTemplate(string templateName, string targetPath)
    {
        try
        {
            // ReSharper disable once AssignNullToNotNullAttribute

            var dir = Assembly.GetExecutingAssembly().Location;

            if (string.IsNullOrEmpty(dir)) return;

            var directoryName = new FileInfo(dir).DirectoryName;
            if (directoryName == null) return;

            var template = Path.Combine(directoryName, $"Templates\\{templateName}");
            var target = Path.Combine(targetPath, templateName);


            //var s = File.ReadAllText(templateName, Encoding.UTF8);

            //if (File.Exists(target)) File.Delete(target);

            //File.WriteAllText(target, s, Encoding.UTF8);

            File.Copy(template, target, true);
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Save text as HTML file
    /// </summary>
    /// <param name="fileName">full filename of the HTML file</param>
    /// <param name="text">content to format as HTML</param>
    /// <param name="title">Document title</param>
    /// <returns>filename on success otherwise null</returns>
    public static string SaveAsHtml(string fileName, IStructuredText text, string title)
    {
        try
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            ArgumentNullException.ThrowIfNull(fileName);

            var dir = new FileInfo(fileName).DirectoryName;

            CopyTemplate("printer.css", dir);

            var f = new HtmlTextFormatter
            {
                Title = title,
                StructuredText = text,
                Template = GetTemplate("HtmlFile"),
                DateString = $"Date created: {DateTime.Now:G}",
                CssCode = "prettyprint linenums"
            };
            var result = f.GetFormattedText();
            File.WriteAllText(fileName, result, System.Text.Encoding.UTF8);

            return result;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get path of current application
    /// </summary>
    /// <returns>current application path</returns>
    public static string GetAppPath()
    {
        if (!string.IsNullOrEmpty(_appPath))
        {
            return _appPath;
        }

        var s = Environment.ProcessPath;

        ArgumentNullException.ThrowIfNull(s);

        s = new FileInfo(s).DirectoryName;

        _appPath = s;
        return _appPath;
    }

    /// <summary>
    /// Get a text from an embedded resource file
    /// </summary>
    /// <param name="resourceName">resource name = file name</param>
    /// <returns></returns>
    public static string GetTextResource(string resourceName)
    {
        var ass = Assembly.GetExecutingAssembly();
        var str = ass.GetManifestResourceStream(resourceName);

        if (str == null)
        {
            return null;
        }

        using var file = new StreamReader(str);
        var s = file.ReadToEnd();

        return s;
    }

    /// <summary>
    /// Save an embedded resource file to an external file
    /// </summary>
    /// <param name="resourceName">resource name = file name</param>
    /// <param name="fileName">File path to save the binary ressource</param>
    /// <returns></returns>
    public static void SaveBinaryResource(string resourceName, string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("fileName is null or empty");
        }

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        var assembly = Assembly.GetExecutingAssembly();

        using var input = assembly.GetManifestResourceStream(resourceName);
        using var output = File.Open(fileName, FileMode.CreateNew);
        if (input == null)
        {
            throw new FileNotFoundException($"{resourceName}: Embedded resoure file not found");
        }

        var buffer = new byte[32768];
        int read;
        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, read);
        }
        output.Flush();
    }

    /// <summary>
    /// Save text as PDF file
    /// </summary>
    /// <param name="fileName">full filename of the PDF file</param>
    /// <param name="text">content to format as PDF</param>
    /// <param name="title">Document title</param>
    /// <returns>filename on success otherwise null</returns>
    public static string SaveAsPdf(string fileName, IStructuredText text, string title)
    {
        try
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var f = new PdfTextFormatter
            {
                Title = title,
                StructuredText = text,
                DateString = $"Date created: {DateTime.Now:G}",
                Author = "JobExecuter"
            };
            f.GetFormattedText();
            f.SaveAsFile(fileName);

            return fileName;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Save text as Markdown file
    /// </summary>
    /// <param name="fileName">full filename of the Markdown file</param>
    /// <param name="text">content to format as Markdown</param>
    /// <param name="title">Document title</param>
    /// <returns>filename on success otherwise null</returns>
    public static string SaveAsMarkdown(string fileName, IStructuredText text, string title)
    {
        try
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            ArgumentNullException.ThrowIfNull(fileName);

            var f = new MarkDownTextFormatter
            {
                Title = title,
                StructuredText = text,
                Template = GetTemplate("MarkdownFile"),
                DateString = $"Date created: {DateTime.Now:G}"
            };
            var result = f.GetFormattedText();
            File.WriteAllText(fileName, result, System.Text.Encoding.UTF8);

            return result;
        }
        catch
        {
            return null;
        }
    }
}