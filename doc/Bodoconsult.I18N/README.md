Bodoconsult.I18N
=================

# What does the library

## Basic ideas

Bodoconsult.I18N library is a simple localization library based on I18N-Portable <https://github.com/xleon/I18N-Portable> by Diego Ponce de León (xleon).

To use I18N-Portable directly was not possible for us. We required a localization library supporting resources from multiple assemblies and sources. We had to not only localize a WPF UI but report generation to PDF files and localized charting to images. The localization resources used for all this were spread of multiple assemblies and in different file formats. This led us to the need of a unique localization implementation usable in multiple environments like WinForms, WPF, console or Avalonia apps.

I18N-Portable did not support this. 
But the general idea of I18N-Portable is cool. Therefore we decided to develop an own I18N-Portable derivative with similar published interfaces but working a bit different behind the scenes.

So the basic idea of Bodoconsult.I18N library is to simplify the usage of multiple technologies existing for localization like RESX resources and XAML resource dictionary under one roof.

Bodoconsult.I18N library currently supports I18N for apps with one currently loaded language at a time only but this language can be changed during runtime. Apps with multiple UI threads running on different languages are not supported currently.

Our implementation was intended for using it with console apps, WinForms apps or WPF apps under .Net8 or later. All other uses case may work but are not tested.

* Usable with a DI container

* Simple initialization setup

* Readable and lean locale files (UTF8 .txt with key/value pairs)

* Support for custom file formats via ILocalesProvider interface (json, xml, etc)

* Light weight

* Well tested

## Localization data sources

Bodoconsult.I18N supports the following localization sources hosted maybe in different assemblies:

-   TXT files

-   CSV files

-   JSON key value pairs files

-   Resx resource files

-   Embedded or external XAML resource files (.xaml extension) for WPF (additional Nuget package Bodoconsult.App.Wpf is required)

-   External AXAML resource files (.axaml extension) for Avalonia (additional Nuget package Bodoconsult.App.Avalonia is required)

Implementing your own implemenation of IResourceProvider normally by deriving from BaseResourceProvider class you can implement your own localization provider based providing localization data from diverse source like file, databases, web resources via REST etc.. 

# How to use the library

The source code contains NUnit test classes, the following source code samples are extracted from. The samples below show the most helpful use cases for the library.

## Setup locales

- In your Net8 project, create a directory called "Locales".

- Create a `{languageCode}.txt` file for each language you want to support. `languageCode` can be a two letter ISO code or a IETF culture name like "en-US" or "de-DE". See [full list here](https://msdn.microsoft.com/en-us/library/ee825488%28v=cs.20%29.aspx).

- Set "Build Action" to "Embedded Resource" on the properties of each file

-   For available file formats see [File formats for locales](#locales--file-formats)

**Locale content sample**

```
    # key = value (the key will be the same across locales)
    one = uno
    two = dos
    three = tres 
    four = cuatro
    five = cinco
      
    # Enums are supported
    Animals.Dog = Perro
    Animals.Cat = Gato
    Animals.Rat = Rata
    Animals.Tiger = Tigre
    Animals.Monkey = Mono
     
    # Support for string.Format()
    stars.count = Tienes {0} estrellas
     
    TextWithLineBreakCharacters = Line One\nLine Two\r\nLine Three
     
    Multiline = Line One
        Line Two
        Line Three
```

## Fluent initialization

```csharp
I18N.Current
    .SetNotFoundSymbol("$") // Optional: when a key is not found, it will appear as $key$ (defaults to "$")
    .SetFallbackLocale("en") // Optional but recommended: locale to load in case the system locale is not supported
    .SetThrowWhenKeyNotFound(true) // Optional: Throw an exception when keys are not found (recommended only for debugging)
    .SetLogger(text => Debug.WriteLine(text)) // action to output traces
```

## Using I18N without depencency injection

The following code does not use Fluent to show the single steps necessary. It may be shortened by using Fluent. 

```csharp
// **** Load all resources from one or more sources ****
// Add provider 1
ILocalesProvider provider = new I18NEmbeddedResourceLocalesProvider(GetType().Assembly,
    "Bodoconsult.I18N.Test.Samples.Locales");

I18N.Current.AddProvider(provider);

// Add provider 2
provider = new I18NEmbeddedResourceLocalesProvider(GetType().Assembly,
    "Bodoconsult.I18N.Test.Locales");

I18N.Current.AddProvider(provider);

Assert.IsTrue(I18N.Current.Languages.Any());

// **** Initialize all ****
// Set a fallback locale for the case current thread language is not available in the resources
I18N.Current.SetFallbackLocale("en");

// Load the default language from thread culture
I18N.Current.Init();


// **** Use it ****
// change to spanish (not necessary if thread language is ok)
I18N.Current.Locale = "es";

var translation = I18N.Current.Translate("one");
Assert.AreEqual("uno", translation);

translation = "Contains".Translate();
Assert.AreEqual("Contains", translation);


// Change to english
I18N.Current.Locale = "en";

translation = I18N.Current.Translate("one");
Assert.AreEqual("one", translation);

translation = "Contains".Translate();
Assert.AreEqual("Contains", translation);
```

## Using I18N with depencency injection and Bodoconsult.App.Abstractions.DiContainer class

### Create your I18N factory based on BaseI18NFactory class

The I18N factories based on BaseI18NFactory class are intended to create fully configured I18N instances with all locales loaded as required by your app. 
To configure the I18N instance create an override for method CreateInstance() loading all providers as required.

``` csharp
/// <summary>
/// Factory to create a fully configured I18N factory
/// </summary>
public class TestI18NFactory: BaseI18NFactory
{
    /// <summary>
    /// Creating a configured II18N instance
    /// </summary>
    /// <returns>An II18N instance</returns>
    public override II18N CreateInstance()
    {
        // Set the fallback language
        I18NInstance.SetFallbackLocale("en");

        // Load a provider
        var provider = new I18NEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            "Bodoconsult.I18N.Test.Samples.Locales");

        I18NInstance.AddProvider(provider);

        // Load more providers if necessary
        // ...

        // Init instance with language from running thread
        I18NInstance.Init();

        // Return the instance
        return I18NInstance;
    }
}
```

If you want to create your own assemblies using Bodoconsult.I18N it may be a good idea to work with locales provider packages based on ILocalesProviderPackage defined in your assemblies. The following code shows a simple sample implementation for such a locales provider package:

``` csharp
/// <summary>
/// Sample implementation of an <see cref="ILocalesProviderPackage"/>
/// </summary>
internal class SampleLocalesProviderPackage: BaseLocalesProviderPackage
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public SampleLocalesProviderPackage()
    {
        var assembly = typeof(SampleLocalesProviderPackage).Assembly;

        // Load a provider
        ILocalesProvider provider = new I18NEmbeddedResourceLocalesProvider(assembly, "Bodoconsult.I18N.Test.Samples.Locales");

        LocalesProviders.Add(provider);

        // Add provider 2
        provider = new I18NEmbeddedResourceLocalesProvider(assembly, "Bodoconsult.I18N.Test.Locales");

        LocalesProviders.Add(provider);
    }
}
```

Use the package in your BaseI18NFactory derived factory class as shown here:

``` csharp
/// <summary>
/// Factory to create a fully configured I18N factory using a locales provider package
/// </summary>
public class TestI18NFactory2 : BaseI18NFactory
{
    /// <summary>
    /// Creating a configured II18N instance
    /// </summary>
    /// <returns>An II18N instance</returns>
    public override II18N CreateInstance()
    {
        // Set the fallback language
        I18NInstance.SetFallbackLocale("en");

        // Load a provider
        var sample = new SampleLocalesProviderPackage();
        sample.LoadLocalesProviders(I18NInstance);

        // Load more providers or packages if necessary
        // ...

        // Init instance with langugae from running thread
        I18NInstance.Init();

        // Return the instance
        return I18NInstance;
    }
}
```
ILocalesProvider and ILocalesProviderPackage implementations may be used in the factory as required in a combined manner making the setup of I18N as easy as possible.

``` csharp

```

If you use Bodoconsult.App app start infrastructure you can add the DI related code to the implementation of the method IDiContainerServiceProvider.AddServices as shown here in the sample:

``` csharp
namespace AvaloniaApp1.DiContainerProvider;

/// <summary>
/// Load all specific AvaloniaApp1 services to DI container. Intended mainly for production
/// </summary>
public class AvaloniaApp1AllServicesContainerServiceProvider : IDiContainerServiceProvider
{
    ...

    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        // Factories to create tower instance related objects (should be singletons)
        diContainer.AddSingletonInstance(Globals.Instance.LogDataFactory);
        diContainer.AddSingleton<IAppLoggerProxyFactory, AppLoggerProxyFactory>();

        // benchmark
        var benchProxy = AppBenchProxy.CreateAppBenchProxy(_benchmarkFileName, Globals.Instance.LogDataFactory);
        diContainer.AddSingletonInstance(benchProxy);

        // General app management
        diContainer.AddSingleton<IGeneralAppManagementService, GeneralAppManagementService>();
        diContainer.AddSingleton<IGeneralAppManagementManager, GeneralAppManagementManager>();

        // I18N
        II18NFactory i18NFactory = new I18NFactory();
        diContainer.AddSingleton(i18NFactory.CreateInstance());

        // Load all other services required for the app now
        diContainer.AddSingleton<IApplicationService, AvaloniaApp1Service>();

        // ...
    }

    ...
}

```

## Data binding

`I18N` implements `INotifyPropertyChanged` and it has an indexer to translate keys. For instance, you could translate a key like:

    string three = I18N.Current["three"]; 

With that said, the easiest way to bind your views to `I18N` translations is to use the built-in indexer 
by creating a proxy object in your ViewModel:

```csharp
public abstract class BaseViewModel
{
    public II18N TranslationService => I18N.Current;
}
```

**WPF Xaml sample**

```xaml
<Button Content="{Binding TranslationService[key]}" />
```

key is an existing key in a locales file.

See MainForm.xaml in WpfApp1 project in the repository.

**Avalonia XAML sample**

```xaml
<Button Text="{Binding TranslationService[key]}" />
``` 

key is an existing key in a locales file.

See MainForm.axaml in AvaloniaApp1 project in the repository.

**Xamarin.Forms sample**

```xaml
<Button Text="{Binding TranslationService[key]}" />`
```    

key is an existing key in a locales file.

**WinForms sample**

WinForms does not support data binding to a indexer. Therefore you have to create a (readonly) property for each control containing translated text.

``` csharp
/// <summary>
/// ViewModel for alternative main window Form1
/// </summary>
public class WinFormsApp1MainWindowViewModel : MainWindowViewModel
{
  ...

  /// <summary>
  /// Property for binding to Text prop of label TranslationLabel
  /// </summary>
  public string TranslationLabelText => TranslationService.Translate("Contains");

  ...
}
```
See Form1 in WinFormsApp1 project in the repository.

## Locales  file formats

For all file formats currently implemented with Bodoconsult.I18N there are samples in the Bodoconsult.I18N.Test project in the repository.

### Plain text files (.txt)

```
one = one
two = two
three = three

Mailbox.Notification = Hello {0}, you've got {1} emails

TextWithLineBreakCharacters = Line One\nLine Two\r\nLine Three

Multiline = Line One
	Line Two
	Line Three

Animals = List of animals
Animals.Dog = Dog
Animals.Cat = Cat
Animals.Rat = Rat
Animals.Snake = Good\nSnake

Fruit.Orange = great orange
Fruit.Apple = big apple
Fruit.Banana = nice banana
```

### CSV text files

```
one;one
two;two
three;three
Mailbox.Notification;Hello {0}, you've got {1} emails
TextWithLineBreakCharacters;Line One\nLine Two\nLine Three
Animals;List of animals
Animals.Dog;Dog
Animals.Cat;Cat
Animals.Rat;Rat
Animals.Snake;Good\nSnake
Fruit.Orange;great orange
Fruit.Apple;big apple
Fruit.Banana;nice banana
```

### JSON key-value-pairs file

``` json
{
  "one": "one",
  "two": "two",
  "three": "three",
  "Mailbox.Notification": "Hello {0}, you've got {1} emails",
  "TextWithLineBreakCharacters": "Line One\nLine Two\nLine Three",
  "Animals": "List of animals",
  "Animals.Dog": "Dog",
  "Animals.Cat": "Cat",
  "Animals.Rat": "Rat",
  "Animals.Snake": "Good\nSnake",
  "Fruit.Orange": "great orange",
  "Fruit.Apple": "big apple",
  "Fruit.Banana": "nice banana"
}
```

### JSON list file

``` json
[
  {
    "key": "one",
    "value": "one"
  },
  {
    "key": "two",
    "value": "two"
  },
  {
    "key": "three",
    "value": "three"
  },
  {
    "key": "Mailbox.Notification",
    "value": "Hello {0}, you've got {1} emails"
  },
  {
    "key": "TextWithLineBreakCharacters",
    "value": "Line One\nLine Two\nLine Three"
  },
  {
    "key": "Animals",
    "value": "List of animals"
  },
  {
    "key": "Animals.Dog",
    "value": "Dog"
  },
  {
    "key": "Animals.Cat",
    "value": "Cat"
  },
  {
    "key": "Animals.Rat",
    "value": "Rat"
  },
  {
    "key": "Animals.Snake",
    "value": "Good\nSnake"
  },
  {
    "key": "Fruit.Orange",
    "value": "great orange"
  },
  {
    "key": "Fruit.Apple",
    "value": "big apple"
  },
  {
    "key": "Fruit.Banana",
    "value": "nice banana"
  }
]
```

### WPF XAML file as embedded resource or external file

``` xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:system="clr-namespace:System;assembly=mscorlib">
    <system:String x:Key="Simulation.Asset">Asset</system:String>
    <system:String x:Key="Simulation.Distribution">Distribution</system:String>
    <system:String x:Key="Simulation.Parameter1">Parameter 1</system:String>
    <system:String x:Key="Simulation.Parameter2">Parameter 2</system:String>
    <system:String x:Key="Simulation.Parameter3">Parameter 3</system:String>
    <system:String x:Key="Simulation.Percentage">Percentage</system:String>
</ResourceDictionary>
```

### Avalonia XAML file only as external file

For AvaloniaResource files there is currently no way to read the key value pairs.

For embedding localization as resource in an Avalonia project use file formats like TXT, CSV, JSON or RESX as described above.

``` xaml
<ResourceDictionary xmlns="https://github.com/avaloniaui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:system="clr-namespace:System;assembly=System.Runtime">
	<system:String x:Key="Simulation.Asset">Asset</system:String>
	<system:String x:Key="Simulation.Distribution">Distribution</system:String>
	<system:String x:Key="Simulation.Parameter1">Parameter 1</system:String>
	<system:String x:Key="Simulation.Parameter2">Parameter 2</system:String>
	<system:String x:Key="Simulation.Parameter3">Parameter 3</system:String>
	<system:String x:Key="Simulation.Percentage">Percentage</system:String>
</ResourceDictionary>
```

### C# default localization with resx resource files

For more information see [MS Learn: Localization in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/localization).






```

```

# To do

## Supporting apps with multiple UI threads running on different languages

New implementation taking the basic ideas of Bodoconsult.I18N to multithreaded environments like an ASP.NET web application or GRPC based services in an client server environment with central localization of the server side.

# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

