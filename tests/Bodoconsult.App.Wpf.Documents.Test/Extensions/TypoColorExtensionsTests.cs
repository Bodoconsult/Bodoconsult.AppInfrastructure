// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Media;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Documents.Extensions;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Documents.Test.Extensions;

[TestFixture]
internal class TypoColorExtensionsTests
{

    [Test]
    public void ToWpfColor_Black_ColorConverted()
    {
        // Arrange 
        var input = TypoColors.Black;

        // Act  
        var result = input.ToWpfColor();

        // Assert
        Assert.That(result, Is.EqualTo(Colors.Black));

    }

}