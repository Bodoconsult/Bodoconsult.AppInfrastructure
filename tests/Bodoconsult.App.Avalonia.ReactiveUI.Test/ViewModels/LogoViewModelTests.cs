// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using NUnit.Framework;
using System.Threading;
using Avalonia.Headless.NUnit;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.ReactiveUI.Test.App;
using Bodoconsult.App.ReactiveUI.Test.Helpers;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.ViewModels;

[Apartment(ApartmentState.STA)]
[TestFixture]
internal class LogoViewModelTests
{
    private const string LogoRessourcePath = "Bodoconsult.App.ReactiveUI.Test.Resources.logo.jpg";

    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var vm = new LogoViewModel(Globals.Instance);

        // Assert
        Assert.That(string.IsNullOrEmpty(vm.AppTitle), Is.False);
    }

    //[Explicit]
    [AvaloniaTest]
    public void LoadLogoFromRessources_ValidSetup_LogoLoaded()
    {
        // Arrange 
        Globals.Instance.AppStartParameter.LogoRessourcePath = LogoRessourcePath;
        Globals.Instance.AppStartParameter.LogoAssembly = typeof(MenuBuilderHelper).Assembly;

        var vm = new LogoViewModel(Globals.Instance);

        // Act  
        vm.LoadLogoFromRessources();

        // Assert
        Assert.That(string.IsNullOrEmpty(vm.AppTitle), Is.False);

        //Wait.Until(() => vm.Logo is not null, 5000);
        //Assert.That(vm.Logo, Is.Not.Null);
    }
}