// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;
using ReactiveUI.Primitives;
using ReactiveUI.SourceGenerators;
using SkiaSharp;
using System.Reflection;
using Bodoconsult.App.ReactiveUI.Ui;

namespace Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;

/// <summary>
/// Viewmodel for an image
/// </summary>
public partial class ImageViewModel : ReactiveObject, IImageViewModel
{
    private byte[] _image = [];
    private readonly IUiFileDialogService _fileDialogService;
    private readonly IAvaloniaUiClipboardService _clipboardService;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="fileDialogService">Current file dialog service</param>
    /// <param name="clipboardService">Current clipboard service</param>
    public ImageViewModel(IUiFileDialogService fileDialogService, IAvaloniaUiClipboardService clipboardService)
    {
        _fileDialogService = fileDialogService;
        _clipboardService = clipboardService;
        SaveAsBitmapCommand = ReactiveCommand.CreateFromTask(SaveAsBitmapCommandTask);
        SaveToClipboardCommand = ReactiveCommand.CreateFromTask(SaveToClipboardCommandTask);
        Title =  string.Empty;
        SaveAsBitmapText = "Save image as JPEG or PNG file";
        SaveToClipboardText = "Save image to clipboard";
    }

    /// <summary>
    /// Gets a string token representing the current view model, such as "login" or "user".
    /// </summary>
    public string UrlPathSegment => "ImageViewModel";

    /// <summary>
    /// UI region the viewmodel is loaded in
    /// </summary>
    public UiRegion? UiRegion { get; private set; }

    /// <summary>
    /// Gets the IScreen that this ViewModel is currently being shown in. This
    /// is usually passed into the ViewModel in the Constructor and saved
    /// as a ReadOnly Property.
    /// </summary>
    public IScreen HostScreen { get; private set; } = new DummyScreen();

    /// <summary>
    /// Current logo to show
    /// </summary>
    [Reactive]
    public partial Bitmap? Bitmap { get; set; }

    /// <summary>
    /// Title to show for the image
    /// </summary>
    [Reactive]
    public partial string Title { get; set; }

    /// <summary>
    /// Button text for the SaveAsBitmap button
    /// </summary>
    [Reactive]
    public partial string SaveAsBitmapText { get; set; }

    /// <summary>
    /// Button text for the SaveToClipboard button
    /// </summary>
    [Reactive]
    public partial string SaveToClipboardText { get; set; }

    /// <summary>
    /// Save as bitmap command
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> SaveAsBitmapCommand { get; }

    /// <summary>
    /// Save the birtmap to clipboard
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> SaveToClipboardCommand { get;}

    /// <summary>
    /// Load bitmap from file
    /// </summary>
    public void LoadBitmapFromFile(string filename)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
        {
            return;
        }

        var bytes = File.ReadAllBytes(filename);

        if (bytes.Length == 0)
        {
            return;
        }

        _image = bytes;

        var bitmapStream = new MemoryStream(bytes);
        CreateBitmap(bitmapStream);
    }

    /// <summary>
    /// Load bitmap
    /// </summary>
    /// <param name="bitmap">Bitmap array to load</param>
    public void LoadBitmap(Memory<byte> bitmap)
    {
        if (bitmap.Length == 0)
        {
            return;
        }

        _image = bitmap.ToArray();

        var bitmapStream = new MemoryStream(_image);
        CreateBitmap(bitmapStream);
    }

    /// <summary>
    /// Save the bitmap as JPEG file
    /// </summary>
    /// <param name="fileName">Full filename to save the bitmap in</param>
    /// <param name="quality">Quality 0 - 100</param>
    public void SaveAsJpeg(string fileName, byte quality)
    {
        if (_image.Length == 0)
        {
            return;
        }

        SaveImage(fileName, SKEncodedImageFormat.Jpeg, quality);
    }

    private void SaveImage(string fileName, SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Png, byte quality = 100)
    {
        var bmp = SKBitmap.Decode(_image);

        if (bmp is null)
        {
            return;
        }

        using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        using var image = SKImage.FromBitmap(bmp);
        using var encodedImage = image.Encode(imageFormat, quality);
        encodedImage.SaveTo(stream);
    }

    /// <summary>
    /// Save the bitmap as PNG file
    /// </summary>
    /// <param name="fileName">Full filename to save the bitmap in</param>
    public void SaveAsPng(string fileName)
    {
        if (_image.Length == 0)
        {
            return;
        }

        SaveImage(fileName);
    }

    /// <summary>
    /// Task starting saving as bitmap
    /// </summary>
    public async Task<RxVoid> SaveAsBitmapCommandTask()
    {
        // Select file name
        var file = await _fileDialogService.SaveFileAsync(PredefinedUiFilePickerOptions.BitmapSaveOptions);

        if (file is null)
        {
            return RxVoid.Default;
        }

        // save now
        var path = file.Path.AbsolutePath;

        if (path.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase))
        {
            SaveAsPng(path);
        }
        else
        {
            SaveAsJpeg(path, 80);
        }

        return RxVoid.Default;
    }

    /// <summary>
    /// Save the image to clipboard
    /// </summary>
    public async Task<RxVoid> SaveToClipboardCommandTask()
    {
        if (Bitmap is null)
        {
            return RxVoid.Default;
        }
        await _clipboardService.SetBitmap(Bitmap);
        return RxVoid.Default;
    }


    /// <summary>
    /// Load logo from ressources defined in <see cref="IAppGlobals"/>.AppStartParameter.LogoRessourcePath
    /// </summary>
    public void LoadLogoFromRessources(Assembly assembly, string ressourcePath)
    {
        if (string.IsNullOrEmpty(ressourcePath))
        {
            return;
        }

        var bitmapStream = assembly.GetManifestResourceStream(ressourcePath);

        if (bitmapStream is null)
        {
            return;
        }

        bitmapStream.Position = 0;

        _image = new byte[bitmapStream.Length];

        bitmapStream.ReadExactly(_image);

        CreateBitmap(bitmapStream);
    }

    private void CreateBitmap(Stream bitmapStream)
    {
        bitmapStream.Position = 0;

        Dispatcher.UIThread.Post(() =>
        {
            Bitmap = new Bitmap(bitmapStream);
        });
    }

    /// <summary>
    /// Method based late injection of <see cref="IScreen"/> instance for navigation
    /// </summary>
    /// <param name="screen"></param>
    public void InjectScreen(UiRegion screen)
    {
        HostScreen = screen;
        UiRegion = screen;
    }
}