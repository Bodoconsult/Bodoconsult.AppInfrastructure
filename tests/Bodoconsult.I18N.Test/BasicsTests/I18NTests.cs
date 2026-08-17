// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Generic;
using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.LocalesProviders;
using Bodoconsult.I18N.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.I18N.Test.BasicsTests;

[TestFixture]
internal class I18NTests : BaseTests
{
    [Test]
    public void AddProvider_ValidProvider_LanguagesLoaded()
    {

        // Arrange
        I18N.Current.Reset();

        var provider = new I18NEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            "Bodoconsult.I18N.Test.Samples.Locales");

        // Add providers
        I18N.Current.AddProvider(provider);

        // Assert
        Assert.That(I18N.Current.Languages.Any());
    }


    [Test]
    public void AddMultiple_TwoProviders_LanguagesLoaded()
    {
        // **** Load all resources from one or more sources ****
        // Add provider 1
        I18N.Current.Reset();

        ILocalesProvider provider = new I18NEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            "Bodoconsult.I18N.Test.Samples.Locales");

        I18N.Current.AddProvider(provider);

        // Add provider 2
        provider = new I18NEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            "Bodoconsult.I18N.Test.Locales");

        I18N.Current.AddProvider(provider);

        Assert.That(I18N.Current.Languages.Any());

        // **** Initialize all ****
        // Set a fallback locale for the case current thread language is not available in the resources
        I18N.Current.SetFallbackLocale("en");

        // Load the default language from thread culture
        I18N.Current.Init();

        // **** Use it ****
        // change to spanish (not necessary if thread language is ok)
        I18N.Current.Locale = "es";

        var translation = I18N.Current.Translate("one");
        Assert.That( translation, Is.EqualTo("uno"));

        translation = "Contains".Translate();
        Assert.That( translation, Is.EqualTo("Contains"));


        // Change to english
        I18N.Current.Locale = "en";

        translation = I18N.Current.Translate("one");
        Assert.That(translation, Is.EqualTo("one"));

        translation = "Contains".Translate();
        Assert.That(translation, Is.EqualTo("Contains"));
    }




    [Test]
    public void Logger_CanBeSet_AsAction()
    {
        var provider = new I18NEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            "Bodoconsult.I18N.Test.Samples.Locales");


        I18N.Current.AddProvider(provider);

        var logs = new List<string>();
        void Logger(string text) => logs.Add(text);

        I18N.Current.SetLogger(Logger);
        I18N.Current.Locale = "en";
        I18N.Current.Locale = "es";

        Assert.That(logs.Count > 0);
    }

    //[Test]
    //public void I18N_CanBeMocked()
    //{
    //    var mock = new I18NMock();
    //    I18N.Current = mock;

    //    Assert.That( I18N.Current.Translate("something"), Is.EqualTo("mocked translation"));
    //}

    //[Test]
    //public void I18N_CanBe_Disposed()
    //{
    //    I18N.Current.PropertyChanged += (_, _) => { };
    //    I18N.Current.Dispose();

    //    Assert.That(I18N.Current, Is.Null);
    //}
}