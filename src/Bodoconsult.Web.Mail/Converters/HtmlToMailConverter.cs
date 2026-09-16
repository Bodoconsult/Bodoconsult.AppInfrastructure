// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Web.Mail.Models;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using MimeKit;

namespace Bodoconsult.Web.Mail.Converters;

/// <summary>
/// Converts an HTML page to a body of an email
/// </summary>
[SuppressMessage("ReSharper", "NotResolvedInText")]
public sealed class HtmlToMailConverter
{
    private static readonly HttpClient HttpClient = new();
    private string _docUrl;

    /// <summary>
    /// Document URL
    /// </summary>
    public string DocUrl
    {
        get => _docUrl;
        set
        {
            _docUrl = value;

            if (_docUrl.Contains(@":\"))
            {
                LocalFile = true;

                var fi = new FileInfo(_docUrl);

                BaseUrl = fi.DirectoryName;
            }
            else
            {
                //ToDo: Get base url for document
                throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// Base url for the DocUrl
    /// </summary>
    public string BaseUrl { get; private set; }

    /// <summary>
    /// File ia local filesystem file (true/false) or otherwise file in the Internet
    /// </summary>
    public bool LocalFile { get; private set; }

    /// <summary>
    /// HTML content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Contains all found images in the document
    /// </summary>
    public List<ImageMetaData> Images { get; set; } = new();

    /// <summary>
    /// Load the file from its location
    /// </summary>
    public void LoadDocument()
    {
        if (LocalFile)
        {
            LoadLocalFile();
        }
        else
        {
            LoadWebFile();
        }
    }

    /// <summary>
    /// Load file form web location
    /// </summary>
    private void LoadWebFile()
    {
        try
        {
            string pageHtml;

            var cts = new CancellationTokenSource(100000);

            var request = HttpClient.GetAsync(DocUrl, cts.Token).GetAwaiter().GetResult();

            using (var stream = request.Content.ReadAsStream())
            {
                if (stream == null)
                {
                    throw new ArgumentNullException("No response from website!");
                }

                using (var reader = new StreamReader(stream))
                {
                    pageHtml = reader.ReadToEnd();
                }
            }
            Content = pageHtml;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving web file {DocUrl}", ex);
        }
    }

    /// <summary>
    /// Load the file form local filesystem
    /// </summary>
    private void LoadLocalFile()
    {
        try
        {
            var fsIn = new FileStream(DocUrl, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sr = new StreamReader(fsIn);
            var s = sr.ReadToEnd();
            sr.Dispose();
            fsIn.Close();

            Content = s;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving local file {DocUrl}", ex);
        }
    }

    /// <summary>
    /// Find all images in the HTML document
    /// </summary>
    public void FindImages()
    {
        const string anchorPattern = @"(?<=img\s*\S*src\=[\x27\x22])(?<Url>[^\x27\x22]*)(?=[\x27\x22])";
        var matches = Regex.Matches(Content,
            anchorPattern,
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        foreach (Match m in matches)
        {
            var url = m.Groups["Url"].Value;

            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var testUri))
            {
                continue;
            }

            if (Images.Any(s => s.OriginalUrl == testUri.ToString()))
            {
                continue;
            }

            var i = new ImageMetaData
            {
                OriginalUrl = testUri.ToString()
            };

            if (i.OriginalUrl.Contains(@":\"))
            {
                i.LocalFile = true;
                i.Url = i.OriginalUrl;
            }
            else if (i.OriginalUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                i.LocalFile = false;
                i.Url = i.OriginalUrl;
            }
            else
            {
                i.LocalFile = LocalFile;

                if (i.LocalFile)
                {
                    i.Url = Path.Combine(BaseUrl, i.OriginalUrl);
                }
                else
                {
                    //ToDo: Create web url
                    throw new NotImplementedException();
                }
            }


            Images.Add(i);
        }
    }

    ///// <summary>
    ///// Load the images as linked ressources (for inclusion in an SMTP mail)
    ///// </summary>
    //public void GetLinkedRessources()
    //{
    //    foreach (var image in Images)
    //    {
    //        if (image.LocalFile)
    //        {

    //            var imagelink = new LinkedResource(image.Url)
    //            {
    //                ContentId = image.ContentId,
    //                //ContentLink = new Uri("cid:" + image.ContentId),
    //                //TransferEncoding = System.Net.Mime.TransferEncoding.Base64
    //            };

    //            LinkedResources.Add(imagelink);
    //        }
    //        else
    //        {
    //            //ToDo: load from web and add to linked ressources
    //        }

    //    }
    //}

    /// <summary>
    /// Replace image paths with cid:-Tags to include linked ressources
    /// </summary>
    public void ProcessContent()
    {
        foreach (var image in Images.OrderByDescending(x => x.Length))
        {             
            Content = Content.Replace(image.OriginalUrl, $"cid:{image.ContentId}");            
        }
    }

    /// <summary>
    /// Store all data from the converter to the mail message
    /// </summary>
    /// <param name="msg">Mail message object</param>
    public void SaveToMail(ref MimeMessage msg)
    {
        var builder = new BodyBuilder
        {
            HtmlBody = Content
        };

        foreach (var image in Images)
        {
            var imagelink = builder.LinkedResources.Add(image.Url);
            imagelink.ContentId = image.ContentId;
        }

        msg.Body = builder.ToMessageBody();
    }

    /// <summary>
    /// Store all data from the converter to the mail message
    /// </summary>
    /// <param name="msg">Mail message object</param>
    public void SaveToMail(ref Message msg)
    {
        msg.Body = new ItemBody
        {
            Content = Content,
            ContentType = BodyType.Html
        };

        msg.HasAttachments = true;
        msg.Attachments = [];

        foreach (var image in Images)
        {
            var att = new FileAttachment
            {
                //ODataType = "#microsoft.graph.fileAttachment",
                ContentBytes = image.ContentBytes,
                ContentType = image.MimeType,
                ContentId = image.ContentId,
                Name = image.Name,
                IsInline = true
            };

            msg.Attachments.Add(att);
        }
    }
}