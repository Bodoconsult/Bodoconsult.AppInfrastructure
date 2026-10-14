// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Create a HTML documentation from diverse JSON files with <see cref="T:Bodoconsult.Text.Interfaces.IStructuredText" /> objects
/// </summary>
internal class DocumentationExecuter : BaseExecuter
{
    private readonly DocumentationJobTask _jobTask;

    private readonly StructuredText _result = new();

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        foreach (var jsonFile in _jobTask.JsonFiles)
        {
            AddTextFromJson(jsonFile);
        }

        if (_jobTask.AppendHelpFile && _result.TextItems.Count > 0)
        {
            var sr = JsonHelper.LoadJsonFromResource<StructuredText>("JobExecuter.Dal.Resources.HelpFile.json");
            _result.AppendText(sr);
        }

        SaveAsHtmlIntern();

        return true;
    }
    /// <summary>
    /// Save test <see cref="_result"/> as HTML
    /// </summary>
    private void SaveAsHtmlIntern()
    {
        if (_result.TextItems.Count == 0) return;

        _result.AddHeader1("Meta data");
        _result.AddDefinitionListLine("Date:", $"{DateTime.Now:g}");
        _result.AddDefinitionListLine("User:", Environment.UserName);
        _result.AddDefinitionListLine("Host:", Environment.MachineName);
        _result.AddDefinitionListLine("IP address:", NetworkHelper.GetLocalIpAddress());
        _result.AddDefinitionListLine("OS:", Environment.OSVersion.VersionString);
        _result.AddDefinitionListLine("64-bit:", Environment.Is64BitOperatingSystem.ToString());

//#if DEBUG
//            JsonHelper.SaveAsFile(@"D:\temp\StructuredText.json", _result);

//#endif


        // Get HTML
        var f = new HtmlTextFormatter
        {
            Title = _jobTask.Title,
            StructuredText = _result,
            Template = FileHelper.GetTemplate("HtmlFile"),
            AddTableOfContent = true
        };
        var result = f.GetFormattedText();

        // Save HTML
        foreach (var html in _jobTask.HtmlTargetFiles)
        {
            try
            {
                var dir = new FileInfo(html).DirectoryName;
                FileHelper.CopyTemplate("printer.css", dir);
                File.WriteAllText(html, result, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                Errors.Add(new Exception($"DocumentationExecuter:SaveAsHtml:{html}", ex));
            }
        }
    }

    /// <summary>
    /// Add a <see cref="IStructuredText"/> object from a JSON file
    /// </summary>
    /// <param name="jsonFile">JSON file to add</param>
    private void AddTextFromJson(string jsonFile)
    {
        try
        {
            if (!File.Exists(jsonFile))
            {
                var msg = $"File {jsonFile} was not found for documentation!";
                _result.AddParagraph(msg);
                SetStatus(msg);
                return;
            }

            var text = JsonHelper.LoadJsonFile<StructuredText>(jsonFile);
            _result.AppendText(text);
        }
        catch (Exception ex)
        {
            Debug.Print(ex.Message);

            Errors.Add(new Exception($"DocumentationExecuter:AddTextFromJson:{jsonFile}", ex));
        }
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public DocumentationExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (DocumentationJobTask)jobTask;
    }
}