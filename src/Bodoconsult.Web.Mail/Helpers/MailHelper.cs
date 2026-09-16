// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.IO;
using System.Reflection;

namespace Bodoconsult.Web.Mail.Helpers;

/// <summary>
/// Mail helper class
/// </summary>
internal static class MailHelper
{
    /// <summary>
    /// Default ctor
    /// </summary>
    static MailHelper()
    {
        var location = Assembly.GetExecutingAssembly().Location;

        if (string.IsNullOrEmpty(location ))
        {
            return;
        }

        var directoryName = new FileInfo(location).DirectoryName;
        if (directoryName == null)
        {
            return;
        }

        TemplateFolderPath = directoryName;
    }

    /// <summary>
    /// Template folder path
    /// </summary>
    public static string TemplateFolderPath { get; }

    /// <summary>
    /// Format the mail body
    /// </summary>
    /// <param name="body">Body text</param>
    /// <param name="subject">Subject</param>
    /// <param name="signatureTemplate">Signature template</param>
    /// <param name="logoUrl">URL of the logo to use or null</param>
    /// <param name="htmlMailTemplate">HTML mail template</param>
    /// <returns></returns>
    public static string FormatBody(string body, string subject, string signatureTemplate, string logoUrl, string htmlMailTemplate)
    {
        ////Do not use string.Format due to { and } in body
        //body = body.Replace("{0}", contentId);

        if (body.Contains("<html", StringComparison.OrdinalIgnoreCase) && body.Contains("</html>", StringComparison.OrdinalIgnoreCase))
        {
            return body;
        }

        var signature = string.IsNullOrEmpty(signatureTemplate) ? "" : GetTemplate(signatureTemplate).Replace("{0}", logoUrl);

        //Do not use string.Format due to { and } in body
        var html = htmlMailTemplate.Replace("{0}", body + signature).Replace("{1}", subject);

        return html;
    }

    /// <summary>
    /// Get template file as string
    /// </summary>
    /// <param name="templateName">Full path to template file</param>
    /// <returns>template content</returns>
    public static string GetTemplate(string templateName)
    {
        try
        {
            // ReSharper disable once AssignNullToNotNullAttribute

            if (!templateName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                templateName = $"{templateName}.txt";
            }

            templateName = Path.Combine(TemplateFolderPath, $"Templates\\{templateName}");

            var fsIn = new FileStream(templateName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sr = new StreamReader(fsIn);
            var s = sr.ReadToEnd();
            sr.Dispose();
            fsIn.Close();
            return s;
        }
        catch
        {
            return "";
        }
    }
}