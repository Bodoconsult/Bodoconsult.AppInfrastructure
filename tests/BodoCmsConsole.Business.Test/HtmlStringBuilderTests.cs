// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoCmsConsole.Business.Model;
using NUnit.Framework;

namespace BodoCmsConsole.Business.Test;

[TestFixture]
public class HtmlStringBuilderTests
{
    [Test]
    public void TestSaveAsCleanHtmlFromString()
    {
        var html = "<p>„Nachhaltige Entwicklung ist eine €-Entwicklung, die die Bedürfnisse der Gegenwart befriedigt, ohne zu riskieren, daß künftige Generationen ihre eigenen Bedürfnisse nicht befriedigen können.“</p>";

        var s = new HtmlStringBuilder();
        s.Add(html);

        s.SaveAsCleanHtml(html, @"D:\temp\test1.htm");
    }

    [Test]
    public void TestSaveAsCleanHtmlFromFile()
    {
        var html = File.ReadAllText(@"S:\Websites\Bodoconsult\about.htm");

        var s = new HtmlStringBuilder();
        s.Add(html);

        s.SaveAsCleanHtml(html, @"D:\temp\test2.htm");
    }
}