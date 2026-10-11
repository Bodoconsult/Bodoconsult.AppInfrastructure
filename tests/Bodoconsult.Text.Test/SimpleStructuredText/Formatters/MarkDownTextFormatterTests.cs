// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.IO;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Model;
using Bodoconsult.Text.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.Text.Test.SimpleStructuredText.Formatters;

[TestFixture]
internal class MarkDownTextFormatterTests
{
    private const string MassText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

    private const string FormattedMasstext =
        "??pa??Task to take a documentation for all SQL databases on a Microsoft(R) SqlServer with exception of databases Master, Msdb, Model and TempDb.??pe??" +
        "??pa??Task to take a documentation for all SQL databases on a Microsoft(R) SqlServer with exception of databases Master, Msdb, Model and TempDb.??pe??";

    [Test]
        
    public void GetFormattedText_NoTemplate_TextCreatedSuccessfully()
    {
        var sr = new StructuredText();
        sr.AddHeader1("Überschrift 1 '& &&&' ");
        sr.AddParagraph(MassText, "CssTestFixture");
        sr.AddDefinitionListLine("Def1", "Value1");
        sr.AddDefinitionListLine("Definition 2", "Value1234");
        sr.AddDefinitionListLine("Defini 3", "Value234556666");
        sr.AddParagraph("");

        sr.AddParagraph(FormattedMasstext,
            "CssTestFixture");

        sr.AddCode(MassText, "CssTestFixture");

        sr.AddParagraph(MassText);

        sr.AddListItem("Bahnhof");
        sr.AddListItem("HauptBahnhof");
        sr.AddListItem("SüdBahnhof");
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);

        var f = new MarkDownTextFormatter { StructuredText = sr };
        var result = f.GetFormattedText();

        Assert.That(result, Is.Not.Null);
        Debug.Print(result);
        Assert.That(!string.IsNullOrEmpty(result));
        Assert.That(result.Contains("<h1"));
        Assert.That(result.Contains("<h2"));
    }

    [Test]
    public void GetFormattedText_MoveHeaderlevel1NoTemplate_TextCreatedSuccessfully()
    {
        var sr = new StructuredText();
        sr.AddHeader1("Überschrift 1 '& &&&' ");

        sr.AddParagraph("??pa??Task to take a documentation for all SQL databases on a Microsoft(R) SqlServer with exception of databases Master, Msdb, Model and TempDb.??pe??");

        sr.AddParagraph(MassText);
        sr.AddParagraph("??pa??Test paragraph PaPe 1??pe????pa??Test paragraph PaPe 2??pe??");

        sr.AddParagraph("<para>Task to take a documentation for all SQL databases on a MSSqlServer with exception of databases Master, Msdb, Model and TempDb.</para><para>A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ for adding database specific " +
                        "content to the docmentation.</para><para>For parts of the documentation the user running the job needs certain permissions." +
                        "To give least possible permissions, this user should be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE." +
                        "You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!</para>");

        sr.AddDefinitionListLine("Test para", "<para>Task to take a documentation for all SQL databases on a MSSqlServer with exception of databases Master, Msdb, Model and TempDb.</para><para>A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ for adding database specific "+
                                              "content to the docmentation.</para><para>For parts of the documentation the user running the job needs certain permissions."+
                                              "To give least possible permissions, this user should be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE."+
                                              "You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!</para>");
        sr.AddDefinitionListLine("Def1", "Value1");
        sr.AddDefinitionListLine("Definition 2", "Value1234");
        sr.AddDefinitionListLine("Defini 3", "Value234556666");
        sr.AddParagraph("");
        sr.AddParagraph(MassText);

        sr.AddListItem("Bahnhof");
        sr.AddListItem("HauptBahnhof");
        sr.AddListItem("SüdBahnhof");
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);

        var f = new MarkDownTextFormatter { StructuredText = sr, MoveHeaderLevel = 1};
        var result = f.GetFormattedText();

        Assert.That(result, Is.Not.Null);
        Debug.Print(result);
        Assert.That(!string.IsNullOrEmpty(result));
        Assert.That(result.Contains("\r\n# "), Is.False);
        Assert.That(result.Contains("## "), Is.True);
        Assert.That(result.Contains("### "), Is.True);
    }



    [Test]
    public void GetFormattedText_MoveHeaderlevel1NoTemplateSynthaxHighLighting_TextCreatedSuccessfully()
    {
        var sr = new StructuredText();
        sr.AddHeader1("Überschrift 1 '& &&&' ");

        sr.AddParagraph("??pa??Task to take a documentation for all SQL databases on a Microsoft(R) SqlServer with exception of databases Master, Msdb, Model and TempDb.??pe??");

        sr.AddCode("public void test()\r\n{{\r\nMessageBox.Show(\"Test\");\r\n}};");

        sr.AddParagraph(MassText);
        sr.AddParagraph("??pa??Test paragraph PaPe 1??pe????pa??Test paragraph PaPe 2??pe??");

        sr.AddParagraph("<para>Task to take a documentation for all SQL databases on a MSSqlServer with exception of databases Master, Msdb, Model and TempDb.</para><para>A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ for adding database specific " +
                        "content to the docmentation.</para><para>For parts of the documentation the user running the job needs certain permissions." +
                        "To give least possible permissions, this user should be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE." +
                        "You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!</para>");

        sr.AddDefinitionListLine("Test para", "<para>Task to take a documentation for all SQL databases on a MSSqlServer with exception of databases Master, Msdb, Model and TempDb.</para><para>A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ for adding database specific " +
                                              "content to the docmentation.</para><para>For parts of the documentation the user running the job needs certain permissions." +
                                              "To give least possible permissions, this user should be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE." +
                                              "You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!</para>");
        sr.AddDefinitionListLine("Def1", "Value1");
        sr.AddDefinitionListLine("Definition 2", "Value1234");
        sr.AddDefinitionListLine("Defini 3", "Value234556666");
        sr.AddParagraph("");
        sr.AddParagraph(MassText);

        sr.AddListItem("Bahnhof");
        sr.AddListItem("HauptBahnhof");
        sr.AddListItem("SüdBahnhof");
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);

        var f = new MarkDownTextFormatter { StructuredText = sr, MoveHeaderLevel = 1 };
        var result = f.GetFormattedText();

        Assert.That(result, Is.Not.Null);
        Debug.Print(result);
        Assert.That(!string.IsNullOrEmpty(result));
        Assert.That(result.Contains("\r\n# "), Is.False);
        Assert.That(result.Contains("## "), Is.True);
        Assert.That(result.Contains("### "), Is.True);
    }

    [Test]
    public void GetFormattedText_WithTemplate_TextCreatedSuccessfully()
    {
        var sr = new StructuredText();
        sr.AddHeader1("Überschrift 'Databanka'");
        sr.AddParagraph(MassText);
        sr.AddDefinitionListLine("Def1", "Value1");
        sr.AddDefinitionListLine("Definition 2", "Value1234");
        sr.AddDefinitionListLine("Defini 3", "Value234556666");
        sr.AddParagraph("");
        sr.AddParagraph(MassText);

        sr.AddListItem("Bahnhof");
        sr.AddListItem("HauptBahnhof");
        sr.AddListItem("SüdBahnhof");
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);

        var f = new MarkDownTextFormatter
        {
            StructuredText = sr,
            Template = "<<<Start>>>{0}<<<Ende>>>",
            AddTableOfContent = true
        };
        var result = f.GetFormattedText();

        Debug.Print(result);
        Assert.That(!string.IsNullOrEmpty(result));
    }

    [Test]
    public void GetFormattedText_WithTable_TextCreatedSuccessfully()
    {
        var sr = new StructuredText();
        sr.AddHeader1("Überschrift 1");

        sr.AddTable("Tabellentitel", DataHelper.GetDataTable());

        sr.AddParagraph(MassText);
        sr.AddDefinitionListLine("Def1", "Value1");
        sr.AddDefinitionListLine("Definition 2", "Value1234");
        sr.AddDefinitionListLine("Defini 3", "Value234556666");
        sr.AddParagraph("");
        sr.AddParagraph(MassText);

        sr.AddListItem("Bahnhof");
        sr.AddListItem("HauptBahnhof");
        sr.AddListItem("SüdBahnhof");
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);
        sr.AddHeader2("Überschrift 2");
        sr.AddParagraph(MassText);
        sr.AddHeader1("Überschrift 1");
        sr.AddParagraph(MassText);
        sr.AddParagraph(MassText);

        var f = new MarkDownTextFormatter
        {
            StructuredText = sr,
            Template = "<<<Start>>>{0}<<<Ende>>>",
            AddTableOfContent = true
        };
        var result = f.GetFormattedText();

        Debug.Print(result);
        Assert.That(!string.IsNullOrEmpty(result));
    }

    [Test]
    public void GetFormattedText_RealLife_TextCreatedSuccessfully()
    {
        var fileName = Path.Combine(TestHelper.TestDataPath, "StructuredText.json");

        var sr = JsonHelper.LoadJsonFile<StructuredText>(fileName);

        var f = new MarkDownTextFormatter
        {
            StructuredText = sr,
            Template = "<<<Start>>>{0}<<<Ende>>>",
            AddTableOfContent = true
        };
        var result = f.GetFormattedText();

        Debug.Print(result);
        Assert.That(!string.IsNullOrEmpty(result));
    }
}