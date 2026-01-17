// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.ResourceProviders;
using Bodoconsult.I18N.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.I18N.Test;

public class I18NEmbeddedResourceProviderTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void RegisterLocaleItems_ValidLocales_ResourceItems()
    {
        // Arrange
        const string key = "de";
        const string resourceFolder = "Bodoconsult.I18N.Test.Locales";
        //const string value = "Is not null";

        ILocalesProvider provider = new I18NEmbeddedResourceProvider(TestHelper.CurrentAssembly,
            resourceFolder);

        Assert.That(!provider.LocaleItems.Any());

        // Act
        provider.RegisterLocaleItems();

        // Assert
        Assert.That(provider.LocaleItems.Any());

        var success = provider.LocaleItems.TryGetValue(key.ToUpperInvariant(), out var result);

        Assert.That(success);
        //Assert.That(value, result);
    }


    [Test]
    public void LoadResourceItem_De_ValuesLoaded()
    {
        // Arrange
        const string key = "de";
        const string resourceFolder = "Bodoconsult.I18N.Test.Locales";
        //const string value = "Is not null";

        ILocalesProvider provider = new I18NEmbeddedResourceProvider(TestHelper.CurrentAssembly,
            resourceFolder);

        Assert.That(!provider.LocaleItems.Any());

        provider.RegisterLocaleItems();

        Assert.That(provider.LocaleItems.Any());

        var success = provider.LocaleItems.TryGetValue(key.ToUpperInvariant(), out var result);

        Assert.That(success);

        // Act
        var translations = provider.LoadLocaleItem("de");

        // Assert
        Assert.That(translations.Any());
    }
}