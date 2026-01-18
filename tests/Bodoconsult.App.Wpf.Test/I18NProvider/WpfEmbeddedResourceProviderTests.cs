// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.I18N;
using Bodoconsult.App.Wpf.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Test.I18NProvider;

// https://medium.com/younited-tech-blog/cant-load-embedded-resources-with-culture-name-suffix-in-net-core-21f279b9327b

[TestFixture]
internal class WpfEmbeddedResourceProviderTests
{

    private const string Path = "Locales";

    [Test]
    public void RegisterLocaleItems_ExistingResources_ResourceItemsLoaded()
    {
        // Arrange 
        var ass = TestHelper.CurrentAssembly;

        var provider = new WpfEmbeddedResourceProvider(ass, Path);

        // Act  
        provider.RegisterLocaleItems();

        // Assert
        Assert.That(provider.LocaleItems.Count, Is.Not.EqualTo(0));
    }

    [Test]
    public void RegisterLocaleItems_ExistingResources_TranslationsLoaded()
    {
        // Arrange 
        var ass = TestHelper.CurrentAssembly;

        var provider = new WpfEmbeddedResourceProvider(ass, Path);
        provider.RegisterLocaleItems();

        // Act  
        var translations = provider.LoadLocaleItem("de");

        // Assert
        Assert.That(translations.Count, Is.Not.EqualTo(0));
    }

    [Test]
    public void RegisterLocaleItems_ExistingResourcesTrailingSlashes_TranslationsLoaded()
    {
        // Arrange 
        var ass = TestHelper.CurrentAssembly;

        var provider = new WpfEmbeddedResourceProvider(ass, Path);
        provider.RegisterLocaleItems();

        // Act  
        var translations = provider.LoadLocaleItem("de");

        // Assert
        Assert.That(translations.Count, Is.Not.EqualTo(0));
    }

}