// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Web.Html.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Web.Html.Test.Html;

[TestFixture]
public class HtmlHelperTests
{
    private const string Content = "Testinhalt eines Paragraphen";
    private const string Url = "http://www.test123.de/image,jpg";
    private const string Alt = "Alternative Text";
    private const string Css = "cssClassName";

    [Test]
    public void TestParagraphSimple()
    {

        var p1 = HtmlHelper.Paragraph(Content);

        Assert.That(!string.IsNullOrEmpty(p1));
        Assert.That(p1.Contains("<p>"));

        Assert.That(p1.Contains("</p>"));
        Assert.That(!p1.Contains("class=\""));

        Debug.Print("Simple paragraph:");
        Debug.Print(p1);
    }


    [Test]
    public void TestParagraphSimpleWithCss()
    {

        var p1 = HtmlHelper.Paragraph(Content, Css);

        Assert.That(!string.IsNullOrEmpty(p1));
        Assert.That(p1.Contains("<p class=\""));

        Assert.That(p1.Contains("</p>"));
        Assert.That(p1.Contains("class=\""));

        Debug.Print("Simple paragraph with css:");
        Debug.Print(p1);
    }


    [Test]
    public void TestDivSimple()
    {

        var p1 = HtmlHelper.Div(Content);

        Assert.That(!string.IsNullOrEmpty(p1));
        Assert.That(p1.Contains("<div>"));

        Assert.That(p1.Contains("</div>"));
        Assert.That(!p1.Contains("class=\""));

        Debug.Print("Simple DIV:");
        Debug.Print(p1);
    }


    [Test]
    public void TestDivSimpleWithCss()
    {

        var p1 = HtmlHelper.Div(Content, Css);

        Assert.That(!string.IsNullOrEmpty(p1));
        Assert.That(p1.Contains("<div class=\""));

        Assert.That(p1.Contains("</div>"));
        Assert.That(p1.Contains("class=\""));

        Debug.Print("Simple DIV with css:");
        Debug.Print(p1);
    }
}