// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.DependencyInjection;
using Bodoconsult.I18N.Test.Samples;
using NUnit.Framework;

namespace Bodoconsult.I18N.Test.DependencyInjection;

[TestFixture]
internal class I18NClientServerDiContainerServiceProviderTests
{
    [Test]
    public void AddServices_DefaultSetup_InstanceLoadedInDiContainer()
    {
        // Arrange 
        var diContainer = new DiContainer();

        var factory = new TestI18NServerFactory();
        var diProvider = new I18NClientServerDiContainerServiceProvider(factory);

        // Act  
        diProvider.AddServices(diContainer);

        diContainer.BuildServiceProvider();

        // Assert server
        var instance = diContainer.Get<II18NServer>();
        instance.FallBackLocale = "en";

        Assert.That(instance, Is.Not.Null);
        Assert.That(instance.Providers.Count, Is.Not.EqualTo(0));


        // Assert scoped client
        var clientInstance = diContainer.Get<II18NClient>();
        clientInstance.Init();

        // **** Use it ****
        // change to spanish (not necessary if thread language is ok)
        clientInstance.Locale = "es";

        var translation = clientInstance.Translate("one");
        Assert.That(translation, Is.EqualTo("uno"));

        // Change to english
        clientInstance.Locale = "en";

        translation = clientInstance.Translate("one");
        Assert.That(translation, Is.EqualTo("one"));
    }
}