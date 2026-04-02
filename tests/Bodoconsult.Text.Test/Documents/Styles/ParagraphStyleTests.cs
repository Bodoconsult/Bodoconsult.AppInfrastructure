// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using NUnit.Framework;

namespace Bodoconsult.Text.Test.Documents.Styles;

[TestFixture]
internal class ParagraphStyleTests
{


    [Test]
    public void Ctor_DefaultCtor_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var style = new ParagraphStyle();

        // Assert
        Assert.That(style.ChildBlocks.Count, Is.EqualTo(0));
        Assert.That(style.ChildInlines.Count, Is.EqualTo(0));
        Assert.That(style.FontSize, Is.Not.EqualTo(0));
    }

    [Test]
    public void ToLdmlString_StyleWithoutBrush_LdmlCreatedWellformed()
    {
        // Arrange 
        var style = new ParagraphStyle();

        var sb = new StringBuilder();

        // Act  
        style.ToLdmlString(sb, string.Empty);

        // Assert
        Assert.That(sb.Length, Is.Not.EqualTo(0));

        Debug.Print(sb.ToString());
    }


    [Test]
    public void ToLdmlString_StyleWithBrush_LdmlCreatedWellformed()
    {
        // Arrange 
        var brush = new SolidColorBrush(TypoColors.AliceBlue.ToLdmlColor());
        
        var style = new ParagraphStyle
        {
            BorderBrush = brush
        };


        var sb = new StringBuilder();

        // Act  
        style.ToLdmlString(sb, string.Empty);

        // Assert
        Assert.That(sb.Length, Is.Not.EqualTo(0));

        Debug.Print(sb.ToString());
    }
}