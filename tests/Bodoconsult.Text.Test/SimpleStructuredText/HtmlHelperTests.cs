// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Text.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Text.Test.SimpleStructuredText;

[TestFixture]
internal class HtmlHelperTests
{
    private const string MassText =
        "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

    [Test]

    public void TestParseHtmlSimple()
    {
        var text = "<p>aafa [safasf] adasdas asdAS [Microsoft](http://microsoft.de) asfa asfas [safasf]  f Asfsaf [Microsoft2](http://microsoft2.de) blablb bala  [safasf]";

        var result = HtmlHelper.ParseHtml(text);

        Assert.That(!string.IsNullOrEmpty(result));
        Assert.That(result.Contains("<a href=\""));
        Assert.That(result.Contains("</a>"));
        Debug.Print(result);
    }

    [Test]
    public void TestGetContentAsHtml()
    {
        var text = "<p>ssda [safasf] adasdas \t asdAS [Microsoft](http://microsoft.de) asfa ??br??asfas f  [safasf]  Asfsaf [Microsoft2](http://microsoft2.de) blablb bala  [safasf]";

        var result = HtmlHelper.GetContentAsHtml(text);

        Assert.That(!string.IsNullOrEmpty(result));
        Assert.That(result.Contains("<a href=\""));
        Assert.That(result.Contains("</a>"));
        Assert.That(result.Contains("<br />"));
        Debug.Print(result);
    }
}