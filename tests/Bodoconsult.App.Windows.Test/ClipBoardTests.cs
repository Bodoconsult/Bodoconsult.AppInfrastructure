// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App.Windows.System;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
public class ClipBoardTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void TestSetText()
    {

        // Arrange
        const string text = "CopyToClipboard";

        // Act
        Clipboard.SetText(text);

        var result = Clipboard.GetText();


        // Assert
        Assert.That(result, Is.EqualTo(text));
    }
}