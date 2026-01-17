// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using NUnit.Framework;
using Bodoconsult.App.Wpf.ReactiveUI.I18N;
using Microsoft.Testing.Platform.Extensions;

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.I18N
{
    [TestFixture]
    internal class WpfLocalesProviderPackageTests
    {


        [Test]
        public void LoadLocalesProviders_EN_Translation()
        {
            // Arrange 
            var i18N = Bodoconsult.I18N.I18N.Current;

            // Set the fallback language
            i18N.SetFallbackLocale("en");

            // Load a provider
            var sample = new WpfLocalesProviderPackage();
            sample.LoadLocalesProviders(i18N);

            // Load more providers or packages if necessary
            // ...

            // Init instance with langugae from running thread
            i18N.Init();

            i18N.Locale = "en";

            // Act  
            var result = i18N.Translate("Wpf.Base.PrintDocumentButtonTooltip");

            // Assert

            Assert.That(result, Is.EqualTo("Print document"));
        }

        [Test]
        public void LoadLocalesProviders_DE_Translation()
        {
            // Arrange 
            var i18N = Bodoconsult.I18N.I18N.Current;

            // Set the fallback language
            i18N.SetFallbackLocale("de");

            // Load a provider
            var sample = new WpfLocalesProviderPackage();
            sample.LoadLocalesProviders(i18N);

            // Load more providers or packages if necessary
            // ...

            // Init instance with langugae from running thread
            i18N.Init();

            i18N.Locale = "de";

            // Act  
            var result = i18N.Translate("Wpf.Base.PrintDocumentButtonTooltip");

            // Assert

            Assert.That(result, Is.EqualTo("Dokument drucken"));
        }

    }
}
