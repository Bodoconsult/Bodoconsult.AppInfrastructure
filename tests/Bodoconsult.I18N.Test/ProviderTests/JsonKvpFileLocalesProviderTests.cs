// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.LocalesProviders;
using Bodoconsult.I18N.Test.Helpers;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Bodoconsult.I18N.Test.ProviderTests;

internal class JsonKvpFileLocalesProviderTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void TestRegisterLocaleItems()
    {

        // Arrange
        const string key = "en";
        var resourceFolder = Path.Combine(TestHelper.GetFolderPath, "SamplesFiles\\JsonKvpLocales");
        //const string value = "Is not null";

        ILocalesProvider provider = new JsonKvpFileLocalesProvider(resourceFolder);

        Assert.That(!provider.LocaleItems.Any());

        // Act
        provider.RegisterLocaleItems();

        // Assert
        Assert.That(provider.LocaleItems.Any());

        var success = provider.LocaleItems.TryGetValue(key, out var result);

        Assert.That(success);
        //Assert.That(value, result);

        Debug.Print(provider.ToString());

    }


    [TestCase("en", "three")]
    [TestCase("es", "tres")]
    public void TestLoadResourceItem(string language, string expectedResult)
    {
        // Arrange
        var resourceFolder = Path.Combine(TestHelper.GetFolderPath, "SamplesFiles\\JsonKvpLocales");
        const string translationKey = "three";

        ILocalesProvider provider = new JsonKvpFileLocalesProvider(resourceFolder);

        Assert.That(!provider.LocaleItems.Any());

        provider.RegisterLocaleItems();

        Assert.That(provider.LocaleItems.Any());

        var success = provider.LocaleItems.TryGetValue(language, out var result);

        Assert.That(success);

        // Act
        var translations = provider.LoadLocaleItem(language);

        // Assert
        Assert.That(translations.Any());

        success = translations.TryGetValue(translationKey, out result);
        Assert.That(success);
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}