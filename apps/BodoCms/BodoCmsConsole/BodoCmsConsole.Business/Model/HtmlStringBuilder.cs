// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Text;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Helpers;
using WebMarkupMin.Core;

namespace BodoCmsConsole.Business.Model;

public class HtmlStringBuilder
{
    private readonly StringBuilder _s;

    /// <summary>
    /// Default ctor
    /// </summary>
    public HtmlStringBuilder()
    {
        _s = new StringBuilder();
    }

    /// <summary>
    /// Ctor setting a size in byte
    /// </summary>
    /// <param name="size">Size in byte to set</param>
    public HtmlStringBuilder(int size)
    {
        _s = new StringBuilder(size);
    }

    /// <summary>
    /// Current state message delegate or null
    /// </summary>
    public StatusMessageDelegate? StatusMessageDelegate { get; set; }

    /// <summary>
    /// List with paring errors
    /// </summary>
    public List<string> ParserErrors = new();

    /// <summary>
    /// Target file name
    /// </summary>
    public string? TargetFile { get; set; }

    /// <summary>
    /// Replace a string with another one
    /// </summary>
    /// <param name="toReplace">String to replace</param>
    /// <param name="replaceWith">String to replace with</param>
    public void Replace(string toReplace, string replaceWith)
    {
        _s.Replace(toReplace, replaceWith);
    }

    /// <summary>
    /// Save as UTF8-HTML
    /// </summary>
    public void SaveHtml()
    {
        ArgumentNullException.ThrowIfNull(TargetFile);

        try
        {
#if DEBUG
            SaveAsCleanHtml(_s.ToString(), TargetFile);
            //File.WriteAllText(TargetFile, _s.ToString(), Encoding.UTF8);
#else
                SaveAsCleanHtml(_s.ToString(), TargetFile);               
#endif

        }
        catch (Exception ex)
        {
            SetMessage($"{TargetFile}:Error: {ex.Message}");
        }
    }


    /// <summary>
    /// Save as clean HTML
    /// </summary>
    /// <param name="html">HTML code</param>
    /// <param name="fileName">Target filename to save HTML in</param>
    /// <param name="showStatistics">Show statistics. Default: false</param>
    public void SaveAsCleanHtml(string html, string fileName, bool showStatistics = false)
    {
        FileSystemHelper.DeleteFile(fileName);

#if DEBUG
        File.WriteAllText(fileName,
            html,
            Encoding.UTF8
        );
#else
            var htmlMinifier = new HtmlMinifier();

            var result = htmlMinifier.Minify(html, generateStatistics: showStatistics);
            if (result.Errors.Count == 0)
            {

                File.WriteAllText(fileName,
                    result.MinifiedContent,
                    Encoding.UTF8
                );

                if (!showStatistics)
                {
                    return;
                }

                var statistics = result.Statistics;
                if (statistics != null)
                {
                    Console.WriteLine("Original size: {0:N0} Bytes",
                        statistics.OriginalSize);
                    Console.WriteLine("Minified size: {0:N0} Bytes",
                        statistics.MinifiedSize);
                    Console.WriteLine("Saved: {0:N2}%",
                        statistics.SavedInPercent);
                }
                Console.WriteLine("Minified content:{0}{0}{1}", Environment.NewLine, result.MinifiedContent);
            }
            else
            {

                File.WriteAllText(fileName,
                    html,
                    Encoding.UTF8
                );

                var errors = result.Errors;

                var msg = $"{fileName}: Found {errors.Count} error(s):";

                //Console.WriteLine(fileName);
                //Console.WriteLine("Found {0} error(s):", errors.Count);
                //Console.WriteLine();

                foreach (var error in errors)
                {
                    msg += $"Line {error.LineNumber} Column {error.ColumnNumber}: {error.Message}: {error.SourceFragment}\r\n";

                    //Console.WriteLine("Line {0}, Column {1}: {2}",
                    //    error.LineNumber, error.ColumnNumber, error.Message);
                    //Console.WriteLine();
                }

                ParserErrors.Add(msg);
            }

            //using (Document doc = Document.FromString(html))
            //{
            //    //doc.OutputBodyOnly = AutoBool.Yes;


            //    //doc.Quiet = true;
            //    //doc.MakeBare = true;

            //    //
            //    //doc.LineBreakBeforeBR = false;
            //    //doc.MakeClean = true;

            //    doc.WrapAt = 200;
            //    doc.ShowInfo = false;
            //    doc.ShowWarnings = false;

            //    doc.RemoveComments = true;
            //    doc.CleanAndRepair();


            //    var erg = doc.Save().Replace("<meta name=\"generator\" content=\"HTML Tidy for HTML5 for Windows version 5.2.0\">\r\n", "");



            //    File.WriteAllText(fileName,
            //            erg,
            //            Encoding.UTF8
            //        );

            //}
#endif
    }

    /// <summary>
    /// Clone the instance
    /// </summary>
    /// <returns>Cloned instance</returns>
    public HtmlStringBuilder Clone()
    {
        return (HtmlStringBuilder)MemberwiseClone();
    }

    /// <summary>
    /// Add a string
    /// </summary>
    /// <param name="addString">String to add</param>
    public void Add(string addString)
    {
        _s.Append(addString);
    }


    /// <summary>
    /// Show a message
    /// </summary>
    /// <param name="msg">Message</param>
    private void SetMessage(string msg)
    {
        StatusMessageDelegate?.Invoke(msg);
    }
}