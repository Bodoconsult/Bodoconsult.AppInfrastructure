// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.ResourceProviders;
using Bodoconsult.I18N.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.I18N.Test;

public class ResxEmbeddedResourceProviderTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void RegisterLocaleItems_ValidLocales_ResourceItems()
    {

        // Arrange
        const string key = "de-DE";
        const string resourceFolder = "Bodoconsult.I18N.Test.Resources.Language";
        //const string value = "Is not null";

        ILocalesProvider provider = new ResxEmbeddedResourceProvider(TestHelper.CurrentAssembly,
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
        const string key = "de-DE";
        const string resourceFolder = "Bodoconsult.I18N.Test.Resources.Language";
        //const string value = "Is not null";

        ILocalesProvider provider = new ResxEmbeddedResourceProvider(TestHelper.CurrentAssembly,
            resourceFolder);

        Assert.That(!provider.LocaleItems.Any());

        provider.RegisterLocaleItems();

        Assert.That(provider.LocaleItems.Any());

        var success = provider.LocaleItems.TryGetValue(key.ToUpperInvariant(), out var result);

        Assert.That(success);

        // Act
        var translations = provider.LoadLocaleItem(key);

        // Assert
        Assert.That(translations.Any());

        success = translations.TryGetValue("Test.Message1", out var value);

        Assert.That(success);
        Assert.That(value, Is.EqualTo("Blubb"));
    }

    [Test]
    public void LoadResourceItem_En_ValuesLoaded()
    {
        // Arrange
        const string key = "en-US";
        const string resourceFolder = "Bodoconsult.I18N.Test.Resources.Language";
        //const string value = "Is not null";

        ILocalesProvider provider = new ResxEmbeddedResourceProvider(TestHelper.CurrentAssembly,
            resourceFolder);

        Assert.That(!provider.LocaleItems.Any());

        provider.RegisterLocaleItems();

        Assert.That(provider.LocaleItems.Any());

        var success = provider.LocaleItems.TryGetValue(key.ToUpperInvariant(), out var result);

        Assert.That(success);

        // Act
        var translations = provider.LoadLocaleItem(key);

        // Assert
        Assert.That(translations.Any());

        success = translations.TryGetValue("Test.Message1", out var value);

        Assert.That(success);
        Assert.That(value, Is.EqualTo("Message 1 as string"));
    }
}