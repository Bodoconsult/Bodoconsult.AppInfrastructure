// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.I18N.Test.Samples;
using NUnit.Framework;

namespace Bodoconsult.I18N.Test.DependencyInjection;

[TestFixture]
internal class TestI18NFactory2Tests
{

    [Test]
    public void CreateInstance_ValidSetup_InstanceCreated()
    {
        // Arrange 
        var factory = new TestI18NFactory2();

        // Act  
        var instance = factory.CreateInstance();

        // Assert
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance.Providers.Count, Is.Not.EqualTo(0));

        // **** Use it ****
        // change to spanish (not necessary if thread language is ok)
        instance.Locale = "es";

        var translation = instance.Translate("one");
        Assert.That(translation, Is.EqualTo("uno"));

        translation = "Contains".Translate();
        Assert.That(translation, Is.EqualTo("Contains"));

        // Change to english
        instance.Locale = "en";

        translation = instance.Translate("one");
        Assert.That(translation, Is.EqualTo("one"));

        translation = "Contains".Translate();
        Assert.That(translation, Is.EqualTo("Contains"));
    }

}