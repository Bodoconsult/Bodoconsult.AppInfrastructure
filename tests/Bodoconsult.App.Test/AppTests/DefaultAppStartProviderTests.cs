// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Test.App;

namespace Bodoconsult.App.Test.AppTests;

[TestFixture]
internal class DefaultAppStartProviderTests
{
    [Test]
    public void ReadLongProperty_SectionContainsProperty_ReturnsValueFromSection()
    {
        // Arrange
        const long startValue = 0;
        const string propertyName = "TestLong";

        // Act 
        var section = Globals.Instance.ConfigurationRoot.GetSection("AppStartParameter");

        var result = DefaultAppStartProvider.ReadLongProperty(section, propertyName, startValue);

        // Assert
        Assert.That(result, Is.EqualTo(98));
    }

    [Test]
    public void ReadLongProperty_SectionDoesNotContainProperty_ReturnsStartValue()
    {
        // Arrange
        const long startValue = 5;
        const string propertyName = "TestLong1";

        // Act 
        var section = Globals.Instance.ConfigurationRoot.GetSection("AppStartParameter");

        var result = DefaultAppStartProvider.ReadLongProperty(section, propertyName, startValue);

        // Assert
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void ReadIntProperty_SectionContainsProperty_ReturnsValueFromSection()
    {
        // Arrange
        const int startValue = 0;
        const string propertyName = "TestInt";

        // Act 
        var section = Globals.Instance.ConfigurationRoot.GetSection("AppStartParameter");

        var result = DefaultAppStartProvider.ReadIntProperty(section, propertyName, startValue);

        // Assert
        Assert.That(result, Is.EqualTo(97));
    }

    [Test]
    public void ReadIntProperty_SectionDoesNotContainProperty_ReturnsStartValue()
    {
        // Arrange
        const int startValue = 5;
        const string propertyName = "TestInt1";

        // Act 
        var section = Globals.Instance.ConfigurationRoot.GetSection("AppStartParameter");

        var result = DefaultAppStartProvider.ReadIntProperty(section, propertyName, startValue);

        // Assert
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void ReadBoolProperty_SectionContainsProperty_ReturnsValueFromSection()
    {
        // Arrange
        const bool startValue = false;
        const string propertyName = "TestBool";

        // Act 
        var section = Globals.Instance.ConfigurationRoot.GetSection("AppStartParameter");

        var result = DefaultAppStartProvider.ReadBoolProperty(section, propertyName, startValue);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void ReadBoolProperty_SectionDoesNotContainProperty_ReturnsStartValue()
    {
        // Arrange
        const bool startValue = false;
        const string propertyName = "TestBool1";

        // Act 
        var section = Globals.Instance.ConfigurationRoot.GetSection("AppStartParameter");

        var result = DefaultAppStartProvider.ReadBoolProperty(section, propertyName, startValue);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void ReadStringProperty_SectionContainsProperty_ReturnsValueFromSection()
    {
        // Arrange
        const string startValue = "test";
        const string propertyName = "TestString";

        // Act 
        var section = Globals.Instance.ConfigurationRoot.GetSection("AppStartParameter");

        var result = DefaultAppStartProvider.ReadStringProperty(section, propertyName, startValue);

        // Assert
        Assert.That(result, Is.EqualTo(propertyName));
    }

    [Test]
    public void ReadStringProperty_SectionDoesNotContainProperty_ReturnsStartValue()
    {
        // Arrange
        const string startValue = "test";
        const string propertyName = "TestString1";

        // Act 
        var section = Globals.Instance.ConfigurationRoot.GetSection("AppStartParameter");

        var result = DefaultAppStartProvider.ReadStringProperty(section, propertyName, startValue);

        // Assert
        Assert.That(result, Is.EqualTo(startValue));
    }

    [Test]
    public void Ctor_DefaultSetup_AllPropsAreNull()
    {

        // Arrange
        var globals = Globals.Instance;

        // Act 
        var provider = new DefaultAppStartProvider(globals);

        // Assert
        Assert.That(provider.AppConfigurationProvider, Is.Null);
        Assert.That(provider.AppGlobals.AppStartParameter, Is.Not.Null);
        Assert.That(provider.DefaultAppLoggerProvider, Is.Null);

    }

    [Test]
    public void LoadConfigurationProvider_DefaultSetup_AppConfigIsLoaded()
    {

        // Arrange
        var globals = Globals.Instance;
        var provider = new DefaultAppStartProvider(globals);

        // Act 
        provider.LoadConfigurationProvider();

        // Assert
        Assert.That(provider.AppConfigurationProvider, Is.Not.Null);
    }



    [Test]
    public void LoadAppStartParameter_DefaultSetup_AppStartParameterIsLoaded()
    {

        // Arrange
        var globals = Globals.Instance;
        var provider = new DefaultAppStartProvider(globals);
        provider.LoadConfigurationProvider();

        // Act 
        provider.LoadAppStartParameter();

        // Assert
        Assert.That(provider.AppGlobals.AppStartParameter, Is.Not.Null);
    }

    [Test]
    public void LoadDefaultAppLoggerProvider_DefaultSetup_DefaultAppLoggerProviderIsLoaded()
    {

        // Arrange
        var globals = Globals.Instance;
        var provider = new DefaultAppStartProvider(globals);
        provider.LoadConfigurationProvider();
        provider.LoadAppStartParameter();

        // Act 
        provider.LoadDefaultAppLoggerProvider();

        // Assert
        Assert.That(provider.DefaultAppLoggerProvider, Is.Not.Null);
    }

}