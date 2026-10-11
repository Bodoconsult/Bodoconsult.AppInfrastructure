// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using System.Runtime.Versioning;
using Bodoconsult.App.Helpers;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Model;
using Bodoconsult.Text.Test.Helpers;
using NUnit.Framework;
using ResourceHelper = Bodoconsult.Text.Test.Helpers.ResourceHelper;

namespace Bodoconsult.Text.Test.SimpleStructuredText.Formatters;

[SupportedOSPlatform("windows")]
[TestFixture]
internal class PdfTextFormatterTests
{
    [Test]
    public void SaveAsFile_ValidInput_PdfSaved()
    {

        var fileName = Path.Combine(TestHelper.TempPath, "TextPdf.pdf");

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        Assert.That(File.Exists(fileName), Is.False);

        var st = new StructuredText();

        st.AddHeader1("Überschrift 1");

        st.AddParagraph(TestDataHelper.MassText);

        var code = ResourceHelper.GetTextResource("code1.txt");

        st.AddCode(code);

        st.AddParagraph(TestDataHelper.MassText);

        st.AddDefinitionListLine("Left1", "Right1");
        st.AddDefinitionListLine("Left2", "Right2");
        st.AddDefinitionListLine("Left3", "Right3");
        st.AddDefinitionListLine("Left4", "Right4");

        st.AddParagraph(TestDataHelper.MassText);

        st.AddTable("Tabelle", DataHelper.GetDataTable());

        st.AddHeader1("Überschrift 2");

        st.AddDefinitionListLine("Left1", "Right1");
        st.AddDefinitionListLine("Left2", "Right2");
        st.AddDefinitionListLine("Left3", "Right3");
        st.AddDefinitionListLine("Left4", "Right4");

        var f = new PdfTextFormatter
        {
            Title = "Testreport",
            StructuredText = st,
            DateString = $"Date created: {DateTime.Now:G}",
            Author = "Testautor"

        };
        f.GetFormattedText();

        // Act
        f.SaveAsFile(fileName);

        // Assert
        Assert.That(File.Exists(fileName));

        FileSystemHelper.RunInDebugMode(fileName);
    }
}