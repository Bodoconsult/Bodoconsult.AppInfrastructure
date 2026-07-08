// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Metadata;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using Bodoconsult.App.ReactiveUI.Tests.App;
using Bodoconsult.App.ReactiveUI.Tests.Helpers;
using NUnit.Framework;
using System.Threading;
using Avalonia.Headless.NUnit;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.ViewModels;

[Apartment(ApartmentState.STA)]
[TestFixture]
internal class LogoViewModelTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var vm = new LogoViewModel(Globals.Instance);

        // Assert
        Assert.That(string.IsNullOrEmpty( vm.AppTitle), Is.False);
    }

    [Explicit]
    [AvaloniaTest]
    public void LoadLogoFromRessources_ValidSetup_LogoLoaded()
    {
        // Arrange 
        Globals.Instance.AppStartParameter.LogoRessourcePath = "Bodoconsult.App.ReactiveUI.Tests.Resources.logo.jpg";
        Globals.Instance.AppStartParameter.LogoAssembly = typeof(MenuBuilderHelper).Assembly;

        var vm = new LogoViewModel(Globals.Instance);

        // Act  
        vm.LoadLogoFromRessources();

        // Assert
        Assert.That(string.IsNullOrEmpty(vm.AppTitle), Is.False);
        Assert.That(vm.Logo, Is.Not.Null);
    }

}