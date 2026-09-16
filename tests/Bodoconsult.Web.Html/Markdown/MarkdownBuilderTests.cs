// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Web.Html.Markdown;
using NUnit.Framework;
using System;
using System.IO;

namespace Bodoconsult.Web.Html.Test.Markdown;

[TestFixture]
internal class MarkdownBuilderTests
{
    [Test]
    public void Ctor_DefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var builder = new MarkdownBuilder();

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public void AddHeadline1_ValidContent_HeadlineAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";

        // Act  
        builder.AddHeadline1(content);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"# {content}{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddHeadline2_ValidContent_HeadlineAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";

        // Act  
        builder.AddHeadline2(content);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"## {content}{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddHeadline3_ValidContent_HeadlineAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";

        // Act  
        builder.AddHeadline3(content);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"### {content}{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddHeadline34ValidContent_HeadlineAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";

        // Act  
        builder.AddHeadline4(content);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"#### {content}{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddHeadline5_ValidContent_HeadlineAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";

        // Act  
        builder.AddHeadline5(content);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"##### {content}{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddParagraph_ValidContent_ContentAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";

        // Act  
        builder.AddParagraph(content);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"{content}{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddCode_ValidContent_ContentAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";

        // Act  
        builder.AddCode(content);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"```{Environment.NewLine}{Environment.NewLine}{content}{Environment.NewLine}{Environment.NewLine}```{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddCode_ValidContentWithType_ContentAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";
        const string type = "csharp";

        // Act  
        builder.AddCode(content, type);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"``` {type}{Environment.NewLine}{Environment.NewLine}{content}{Environment.NewLine}{Environment.NewLine}```{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddList_ValidContent_ListAdded()
    {
        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test";

        // Act  
        builder.AddList(content);

        // Assert
        var result = builder.GetContent();
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"-{content}{Environment.NewLine}{Environment.NewLine}"));
    }

    [Test]
    public void AddBold_ValidContent_BoldAdded()
    {
        // Arrange 
        const string content = "Test";

        // Act  
        var result = MarkdownBuilder.AddBold(content);

        // Assert
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"**{content}**"));
    }

    [Test]
    public void AddItalic_ValidContent_ItalicAdded()
    {
        // Arrange 
        const string content = "Test";

        // Act  
        var result = MarkdownBuilder.AddItalic(content);

        // Assert
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"*{content}*"));
    }


    [Test]
    public void AddItalicBold_ValidContent_ItalicBoldAdded()
    {
        // Arrange 
        const string content = "Test";

        // Act  
        var result = MarkdownBuilder.AddItalicBold(content);

        // Assert
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"***{content}***"));
    }

    [Test]
    public void AddUrl_ValidContent_UrlAdded()
    {
        // Arrange 
        const string url = "Test";

        // Act  
        var result = MarkdownBuilder.AddUrl(url);

        // Assert
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"[{url}]({url})"));
    }

    [Test]
    public void AddUrlDescription_ValidContent_UrlDescpriptionAdded()
    {
        // Arrange 
        const string url = "Test";
        const string description = "Test";

        // Act  
        var result = MarkdownBuilder.AddUrl(url, description);

        // Assert
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"[{description}]({url})"));
    }

    [Test]
    public void AddUrlImage_ValidContent_UrlImageAdded()
    {
        // Arrange 
        const string url = "Test";

        // Act  
        var result = MarkdownBuilder.AddUrlImage(url);

        // Assert
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"![{url}]({url})"));
    }

    [Test]
    public void AddUrlImageDescription_ValidContent_UrlImageDescpriptionAdded()
    {
        // Arrange 
        const string url = "Test";
        const string description = "Test";

        // Act  
        var result = MarkdownBuilder.AddUrlImageDescription(url, description);

        // Assert
        Assert.That(result.Length, Is.Not.EqualTo(0));
        Assert.That(result, Is.EqualTo($"![{description}]({url})"));
    }

    [Test]
    public void Save_ValidContent_FileSaved()
    {
        var path = Path.Combine(Path.GetTempPath(), "test.md");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        Assert.That(File.Exists(path), Is.False);

        // Arrange 
        var builder = new MarkdownBuilder();
        const string content = "Test blubb blubb";

        // Act  
        builder.AddHeadline1("Headline 1");
        builder.AddParagraph(content);
        builder.AddRawMarkdown("## SubHeadline\r\n\r\nBlubb blabb blubb\r\n\r\n");
        builder.AddParagraph(content);
        builder.AddParagraph(content);

        builder.AddHeadline1("Headline 2");

        builder.AddParagraph(content);
        builder.AddParagraph(content);
        builder.AddParagraph(content);

        builder.Save(path);

        // Assert
        Assert.That(File.Exists(path));

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    // ToDo: add separate tests for all new methods

    // ToDo: add a test using multiple method for creating a demo markdown file. This test will be used as demo code for MarkDownBuilder class docu

    // ToDo: write docu for MarkDownBuilder class in /doc/Bodoconsult.Web.Html/readme.md. Add code sample from demo code abov

}