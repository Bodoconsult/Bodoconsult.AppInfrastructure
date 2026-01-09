// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Text.Test.Documents;

[TestFixture]
public class LdmlReaderTests
{

    [Test]
    public void Ctor_DefaultCtor_NoStringLoaded()
    {
        // Arrange 

        // Act  
        var r = new LdmlReader();

        // Assert
        Assert.That(string.IsNullOrEmpty(r.Ldml), Is.True);
    }

    [Test]
    public void Ctor_CtorLoadingString_StringLoaded()
    {
        // Arrange 
        var ldmlFile = Path.Combine(TestHelper.TestDataPath, "Document1.ldml");
        var ldml = File.ReadAllText(ldmlFile);

        // Act  
        var r = new LdmlReader(ldml);

        // Assert
        Assert.That(string.IsNullOrEmpty(r.Ldml), Is.False);
    }

    [Test]
    public void LoadLdmlFile_ExistingFile_StringLoaded()
    {
        // Arrange 
        var r = new LdmlReader();
        var ldmlFile = Path.Combine(TestHelper.TestDataPath, "Document1.ldml");

        // Act  
        r.LoadLdmlFile(ldmlFile);

        // Assert
        Assert.That(string.IsNullOrEmpty(r.Ldml), Is.False);
    }

    [Test]
    public void ParseLdml_ExistingFile_DocParsed()
    {
        // Arrange 
        var r = new LdmlReader();
        var ldmlFile = Path.Combine(TestHelper.TestDataPath, "Document1.ldml");

        r.LoadLdmlFile(ldmlFile);

        // Act  
        r.ParseLdml();

        // Assert
        Assert.That(r.TextElement, Is.Not.Null);
    }

    [Test]
    public void ParseLdml_NewDocument_DocParsed()
    {
        // Arrange 
        var sb = new StringBuilder();

        var doc = TestDataHelper.CreateDocument();
        var section = (Section)doc.ChildBlocks.FirstOrDefault(x=> x.GetType() == typeof(Section));

        Assert.That(section, Is.Not.Null);

        section.IncludeInToc = false;

        doc.ToLdmlString(sb, string.Empty);

        Debug.Print(sb.ToString());

        var r = new LdmlReader(sb.ToString());

        // Act  
        r.ParseLdml();

        // Assert
        Assert.That(r.TextElement, Is.Not.Null);

        var doc2 = (Document)r.TextElement;
        var section2 = (Section)doc2.ChildBlocks.FirstOrDefault(x => x.GetType() == typeof(Section));

        Assert.That(section2, Is.Not.Null);

        Assert.That(section2.IncludeInToc, Is.False);
    }


    [Test]
    public void ParseLdml_Heading1Style_DocParsed()
    {
        // Arrange 
        var sb = new StringBuilder();

        var doc = new Heading1Style
        {
            BorderThickness =
            {
                Top = 5
            },
            BorderBrush = new SolidColorBrush(TypoColors.AliceBlue.ToLdmlColor())
        };

        doc.ToLdmlString(sb, string.Empty);

        Debug.Print(sb.ToString());

        var r = new LdmlReader(sb.ToString());

        // Act  
        r.ParseLdml();

        // Assert
        Assert.That(r.TextElement, Is.Not.Null);

    }

    [Test]
    public void ParseLdml_Table_DocParsed()
    {
        // Arrange 
        var sb = new StringBuilder();

        var dt = DataHelper.GetDataTable();

        var dtp = new DataTableParser(dt);
        dtp.ParseColumns();
        dtp.ParseRows();
        dtp.Table.ToLdmlString(sb, string.Empty);

        Debug.Print(sb.ToString());

        var r = new LdmlReader(sb.ToString());

        // Act  
        r.ParseLdml();

        // Assert
        Assert.That(r.TextElement, Is.Not.Null);

    }

    [Test]
    public void ParseLdml_DefinitionList_DocParsed()
    {
        // Arrange 
        var sb = new StringBuilder();

        var dl = TestDataHelper.CreateDefinitionList();

        dl.ToLdmlString(sb, string.Empty);

        Debug.Print(sb.ToString());

        var r = new LdmlReader(sb.ToString());

        // Act  
        r.ParseLdml();

        // Assert
        Assert.That(r.TextElement, Is.Not.Null);

    }

    [Test]
    public void ToLdmlString_DocumentStyle_DocParsed()
    {
        // Arrange 
        var sb = new StringBuilder();

        var ds = new DocumentStyle();

        // Act
        ds.ToLdmlString(sb, string.Empty);

        Debug.Print(sb.ToString());

        Assert.That(sb.ToString().Contains("PaperFormat"));
    }

}