// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Helper class for JobExecuter help
/// </summary>
public class HelpHelper
{
    /// <summary>
    /// Get the help file as string
    /// </summary>
    /// <returns></returns>
    public static string GetHelp()
    {
        var sr = JsonHelper.LoadJsonFromResource<StructuredText>("JobExecuter.Dal.Resources.HelpFile.json");

        var f = new PlainTextFormatter
        {
            StructuredText = sr
        };
        var result = f.GetFormattedText();

        return result;
    }

    /// <summary>
    /// Add the general help to a text
    /// </summary>
    public static void AppendHelpToText(IStructuredText text)
    {
        var sr = JsonHelper.LoadJsonFromResource<StructuredText>("JobExecuter.Dal.Resources.HelpFile.json");

        foreach (var item in sr.TextItems)
        {
            text.TextItems.Add(item);
        }
    }
}