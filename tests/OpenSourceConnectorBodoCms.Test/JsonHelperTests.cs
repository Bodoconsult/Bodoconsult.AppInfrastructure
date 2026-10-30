// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.IO;
using Bodoconsult.App.Helpers;
using NUnit.Framework;
using OpenSourceConnectorBodoCms.Business.Helpers;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Test;

[TestFixture]
internal class JsonHelperTests
{
    [Test]
    public void SaveAsFile_ValidObject_SerializedAandSavedAsFile()
    {
        // Arrange 
        var list = new List<OpenSourceMetaData> { new() };

        var fileName = Path.Combine(Path.GetTempPath(), "test.json");

        // Act  
        JsonHelper.SaveAsFile(fileName, list);

        // Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }

    [Test]
    public void LoadJsonFile_ValidObject_SerializedAandSavedAsFile()
    {
        // Arrange 
        var list = new List<OpenSourceMetaData> { new(), new() };

        var fileName = Path.Combine(Path.GetTempPath(), "test.json");

        JsonHelper.SaveAsFile(fileName, list);
        Assert.That(File.Exists(fileName));

        // Act  
        var result = JsonHelper.LoadJsonFile<List<OpenSourceMetaData>>(fileName);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.Not.Zero);
    }
}