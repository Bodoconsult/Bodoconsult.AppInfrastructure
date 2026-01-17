// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.LocalesProviders;
using Bodoconsult.I18N.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.I18N.Test.ProviderTests;

internal class JsonKvpEmbeddedResourceLocalesProviderTests
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
        const string resourceFolder = "Bodoconsult.I18N.Test.Samples.JsonKvpLocales";
        //const string value = "Is not null";

        ILocalesProvider provider = new JsonKvpEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            resourceFolder);

        Assert.That(!provider.LocaleItems.Any());

        // Act
        provider.RegisterLocaleItems();

        // Assert
        Assert.That(provider.LocaleItems.Any());

        var success = provider.LocaleItems.TryGetValue(key, out var result);

        Assert.That(success);
        //Assert.That(value, result);

    }


    [TestCase("en", "three")]
    [TestCase("es", "tres")]
    public void TestLoadResourceItem(string language, string expectedResult)
    {

        // Arrange
        const string resourceFolder = "Bodoconsult.I18N.Test.Samples.JsonKvpLocales";
        const string translationKey = "three";

        ILocalesProvider provider = new JsonKvpEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            resourceFolder);

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