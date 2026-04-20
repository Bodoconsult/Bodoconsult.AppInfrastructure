// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Avalonia.I18N;
using Bodoconsult.App.Avalonia.Test.Helpers;

namespace Bodoconsult.App.Avalonia.Test.I18NProvider;

[TestFixture]
internal class AvaloniaFileLocalesProviderTests
{

    private readonly string _localesFiles = Path.Combine(TestHelper.GetFolderPath,  "LocalesFiles");

    [Test]
    public void RegisterLocaleItems_ExistingLocales_LocalesLoaded()
    {
        // Arrange 
        var provider = new AvaloniaFileLocalesProvider(_localesFiles);

        // Act  
        provider.RegisterLocaleItems();

        // Assert
        Assert.That(provider.LocaleItems, Is.Not.Null);
        Assert.That(provider.LocaleItems.Count, Is.Not.EqualTo(0));
    }

    [Test]
    public void LoadResourceItem_CultureEn_TranslationsLoaded()
    {
        // Arrange 
        var provider = new AvaloniaFileLocalesProvider(_localesFiles);
        provider.RegisterLocaleItems();

        // Act  
        var translations = provider.LoadLocaleItem("en");

        // Assert
        Assert.That(translations.Count, Is.Not.EqualTo(0));

    }

    [Test]
    public void LoadResourceItem_CultureDe_TranslationsLoaded()
    {
        // Arrange 
        var provider = new AvaloniaFileLocalesProvider(_localesFiles);
        provider.RegisterLocaleItems();

        // Act  
        var translations = provider.LoadLocaleItem("de");

        // Assert
        Assert.That(translations.Count, Is.Not.EqualTo(0));

    }

    //[Test]
    //public void LoadResourceItem_CultureDe_TranslationsLoaded()
    //{
    //    // Arrange 
    //    var I18N = Bodoconsult.I18N.I18N.Current;


    //    var provider = new WpfFileLocalesProvider(English);

    //    I18N.AddProvider(provider)


    //    // Act  


    //    // Assert
    //    Assert.That(translations.Count, Is.Not.EqualTo(0));

    //}

}