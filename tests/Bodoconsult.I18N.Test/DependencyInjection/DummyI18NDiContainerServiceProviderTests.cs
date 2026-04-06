// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.DependencyInjection;
using NUnit.Framework;

namespace Bodoconsult.I18N.Test.DependencyInjection;

[TestFixture]
internal class DummyI18NDiContainerServiceProviderTests
{
    [OneTimeTearDown]
    public void Cleanup()
    {
        I18N.IsDummyRequested = false;
        I18N.ResetCurrent();
    }

    [Test]
    public void AddServices_DefaultSetup_InstanceLoadedInDiContainer()
    {
        // Arrange 
        var diContainer = new DiContainer();
        var diProvider = new DummyI18NDiContainerServiceProvider();

        // Act  
        diProvider.AddServices(diContainer);

        diContainer.BuildServiceProvider();

        // Assert
        var instance = diContainer.Get<II18N>();

        Assert.That(instance, Is.Not.Null);
        Assert.That(instance.Providers.Count, Is.Zero);

        // **** Use it ****
        // change to spanish (not necessary if thread language is ok)
        instance.Locale = "es";

        var translation = instance.Translate("one");
        Assert.That(translation, Is.EqualTo("one"));

        translation = "Contains".Translate();
        Assert.That(translation, Is.EqualTo("Contains"));

        // Change to english
        instance.Locale = "en";

        translation = instance.Translate("one");
        Assert.That(translation, Is.EqualTo("one"));

        translation = "Contains".Translate();
        Assert.That(translation, Is.EqualTo("Contains"));
    }

    [Test]
    public void AddServices_DefaultSetup_DelegatesDelivered()
    {
        // Arrange 
        var diContainer = new DiContainer();
        var diProvider = new DummyI18NDiContainerServiceProvider();

        I18N.IsDummyRequested = true;

        // Act  
        diProvider.AddServices(diContainer);

        diContainer.BuildServiceProvider();

        var i18N = diContainer.Get<II18N>();
        i18N.Locale = "es";

        // Assert
        var instance = diContainer.Get<TranslateDelegate>();

        Assert.That(instance, Is.Not.Null);

        // **** Use it ****
        var result = instance.Invoke("one");
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("one"));

        var instance2 = diContainer.Get<TranslateWithParamsDelegate>();

        Assert.That(instance2, Is.Not.Null);

        // **** Use it ****
        result = instance2.Invoke("one");
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("one"));
    }
}