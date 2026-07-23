// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Threading;
using Avalonia.Headless.NUnit;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using Bodoconsult.App.ReactiveUI.Tests.App;
using Bodoconsult.App.ReactiveUI.Tests.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.ViewModels;

[Apartment(ApartmentState.STA)]
[TestFixture]
internal class ImageViewModelTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var vm = new ImageViewModel();

        // Assert
        Assert.That(vm.Bitmap, Is.Null);
    }

    //[Explicit]
    [AvaloniaTest]
    public void LoadLogoFromRessources_ValidSetup_LogoLoaded()
    {
        // Arrange 
        Globals.Instance.AppStartParameter.LogoRessourcePath = "Bodoconsult.App.ReactiveUI.Tests.Resources.logo.jpg";
        Globals.Instance.AppStartParameter.LogoAssembly = typeof(MenuBuilderHelper).Assembly;

        var vm = new ImageViewModel();

        // Act  
        vm.LoadLogoFromRessources(Globals.Instance.AppStartParameter.LogoAssembly, Globals.Instance.AppStartParameter.LogoRessourcePath);

        // Assert
        Assert.That(vm.Bitmap, Is.Not.Null);
    }

    [AvaloniaTest]
    public void LoadLogoFromRessources_ValidSetup_LogoLoaded()
    {
        // Arrange 
        Globals.Instance.AppStartParameter.LogoRessourcePath = "Bodoconsult.App.ReactiveUI.Tests.Resources.logo.jpg";
        Globals.Instance.AppStartParameter.LogoAssembly = typeof(MenuBuilderHelper).Assembly;

        var vm = new ImageViewModel();

        // Act  
        vm.LoadLogoFromRessources(Globals.Instance.AppStartParameter.LogoAssembly, Globals.Instance.AppStartParameter.LogoRessourcePath);

        // Assert
        Assert.That(vm.Bitmap, Is.Not.Null);
    }

}