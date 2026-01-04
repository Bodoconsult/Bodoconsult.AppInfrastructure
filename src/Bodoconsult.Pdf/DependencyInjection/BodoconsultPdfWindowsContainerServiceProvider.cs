// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Factories;
using Bodoconsult.Pdf.Interfaces;
using Bodoconsult.Pdf.PdfSharp;
using PdfSharp.Fonts;
using System.Runtime.Versioning;

namespace Bodoconsult.Pdf.DependencyInjection;

/// <summary>
/// DI container service provider for loading PDF creation features like <see cref="IPdfBuilderFactory"/> on Windows OS. Uses WindowsFontResolver class as fomt resolver.
/// </summary>
[SupportedOSPlatform("windows")]
public class BodoconsultPdfWindowsContainerServiceProvider : IDiContainerServiceProvider
{
    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        var resolver = new WindowsFontResolver();
        diContainer.AddSingleton<IFontResolver>(resolver);
        diContainer.AddSingleton<IPdfBuilderFactory, PdfBuilderFactory>();
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void LateBindObjects(DiContainer diContainer)
    {
        // Do nothing
    }
}