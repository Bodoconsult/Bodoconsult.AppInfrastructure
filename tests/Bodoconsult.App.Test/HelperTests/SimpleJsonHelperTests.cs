// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
internal class SimpleJsonHelperTests
{
    [Test]
    public void SerializeObjectLoadJsonString_ValidObject_ObjectDeserializedSuccessfully()
    {
        // Arrange 
        var data = new O365MailAccount
        {
            MailAddressSender = "robert.leisner@bodoconsult.de"
        };

        // Act  
        var json = SimpleJsonHelper.Serialize(data);

        Assert.That(string.IsNullOrEmpty(json), Is.False);

        var data2 = SimpleJsonHelper.LoadJsonString<O365MailAccount>(json);

        // Assert
        Assert.That(data2, Is.Not.Null);
        Assert.That(data2.MailAddressSender, Is.EqualTo( data.MailAddressSender));
    }

    [Test]
    public void SerializeAsFileLoadJson_ValidObject_ObjectDeserializedSuccessfully()
    {
        // Arrange 
        var data = new O365MailAccount
        {
            MailAddressSender = "robert.leisner@bodoconsult.de"
        };

        var path = Path.Combine(Path.GetTempPath(), "test.json");

        // Act  
        var json = SimpleJsonHelper.SerializeAsFile(path, data);

        Assert.That(string.IsNullOrEmpty(json), Is.False);
        Assert.That(File.Exists(path));

        var data2 = SimpleJsonHelper.LoadJsonFile<O365MailAccount>(path);

        // Assert
        Assert.That(data2, Is.Not.Null);
        Assert.That(data2.MailAddressSender, Is.EqualTo(data.MailAddressSender));
    }
}