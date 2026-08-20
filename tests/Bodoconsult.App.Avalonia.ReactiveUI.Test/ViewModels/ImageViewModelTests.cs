// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.Metadata;
using Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.Services;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Tests.App;
using Bodoconsult.App.ReactiveUI.Tests.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.ViewModels;

[Apartment(ApartmentState.STA)]
[TestFixture]
internal class ImageViewModelTests
{
    private Window _window;
    private IUiFileDialogService _dialogService;
    private IAvaloniaUiClipboardService _clipboardService;

    //public ImageViewModelTests()
    //{
    //    //        var textBox = new TextBox();
    //    //_window = new Window { Content = textBox };
    //    ////_window.Show();

    //    //_dialogService = new FileDialogService(_window);
    //}

    public void Setup()
    {
        if (_window is not null)
        {
            return;
        }

        var textBox = new TextBox();
        _window = new Window { Content = textBox };
        //_window.Show();

        _dialogService = new FileDialogService(_window);
        _clipboardService = new AvaloniaClipboardService(_window);
    }

    [TearDown]
    public void CleanUp()
    {
        _window?.Close();
    }

    [AvaloniaTest]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        Setup();

        // Act  
        var vm = new ImageViewModel(_dialogService, _clipboardService);

        // Assert
        Assert.That(vm.Bitmap, Is.Null);
    }

    //[Explicit]
    [AvaloniaTest]
    public void LoadLogoFromRessources_ValidSetup_LogoLoaded()
    {
        // Arrange 
        Setup();

        Globals.Instance.AppStartParameter.LogoRessourcePath = "Bodoconsult.App.ReactiveUI.Tests.Resources.logo.jpg";
        Globals.Instance.AppStartParameter.LogoAssembly = typeof(MenuBuilderHelper).Assembly;

        var vm = new ImageViewModel(_dialogService, _clipboardService);

        // Act  
        Assert.DoesNotThrow(() =>
        {
            vm.LoadLogoFromRessources(Globals.Instance.AppStartParameter.LogoAssembly, Globals.Instance.AppStartParameter.LogoRessourcePath);
        });

        //Wait.Until(() => vm.Bitmap is not null);

        // Assert
        //Assert.That(vm.Bitmap, Is.Not.Null);
    }

    [AvaloniaTest]
    public void SaveAsJpeg_ValidSetup_JpegSaved()
    {
        // Arrange 
        Setup();

        var path = Path.Combine(Path.GetTempPath(), "test.jpg");

        Globals.Instance.AppStartParameter.LogoRessourcePath = "Bodoconsult.App.ReactiveUI.Tests.Resources.logo.jpg";
        Globals.Instance.AppStartParameter.LogoAssembly = typeof(MenuBuilderHelper).Assembly;

        var vm = new ImageViewModel(_dialogService, _clipboardService);
        vm.LoadLogoFromRessources(Globals.Instance.AppStartParameter.LogoAssembly, Globals.Instance.AppStartParameter.LogoRessourcePath);

        // Act  
        vm.SaveAsJpeg(path, 100);

        // Assert
        Assert.That(File.Exists(path), Is.True);

        FileSystemHelper.RunInDebugMode(path);
    }

    [AvaloniaTest]
    public void SaveAsPng_ValidSetup_PngSaved()
    {
        // Arrange 
        Setup();

        var path = Path.Combine(Path.GetTempPath(), "test.png");

        Globals.Instance.AppStartParameter.LogoRessourcePath = "Bodoconsult.App.ReactiveUI.Tests.Resources.logo.jpg";
        Globals.Instance.AppStartParameter.LogoAssembly = typeof(MenuBuilderHelper).Assembly;

        var vm = new ImageViewModel(_dialogService, _clipboardService);
        vm.LoadLogoFromRessources(Globals.Instance.AppStartParameter.LogoAssembly, Globals.Instance.AppStartParameter.LogoRessourcePath);

        // Act  
        vm.SaveAsJpeg(path, 100);

        // Assert
        Assert.That(File.Exists(path), Is.True);

        FileSystemHelper.RunInDebugMode(path);
    }
}