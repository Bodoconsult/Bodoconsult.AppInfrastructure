// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.LocalesProviders;
using Bodoconsult.I18N.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.I18N.Test.ProviderTests;

internal class I18NFileLocalesProviderTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void TestRegisterLocaleItems()
    {

        // Arrange
        const string key = "de";
        const string resourceFolder = "LocalesFiles";
        //const string value = "Is not null";

        ILocalesProvider provider = new I18NFileLocalesProvider(TestHelper.CurrentAssembly,
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


    [TestCase("de", "enthält")]
    [TestCase("pt", "contem")]
    [TestCase("fr-FR", "Französisch")]
    [TestCase("en", "Contains")]
    public void TestLoadResourceItem(string language, string expectedResult)
    {

        // Arrange
        const string resourceFolder = "LocalesFiles";
        const string translationKey = "Contains";

        ILocalesProvider provider = new I18NFileLocalesProvider(TestHelper.CurrentAssembly,
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