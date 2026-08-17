// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Diagnostics;
using System.Runtime.Versioning;
using Bodoconsult.Pdf.PdfSharp;
using NUnit.Framework;

namespace Bodoconsult.Pdf.Test;

[TestFixture]
[SupportedOSPlatform("windows")]
public class WindowsFontResolverTests
{
    [Test]
    public void Ctor_Default_InstalledFontsLoaded()
    {
        // Arrange 

        // Act  
        var r = new WindowsFontResolver();

        // Assert
        Assert.That(r.InstalledFonts.Count, Is.Not.EqualTo(0));

        foreach (var pair in r.InstalledFonts)
        {
            Debug.Print($"{pair.Key} :: {pair.Value}");
        }
    }

    [Test]
    public void ResolveTypeface_Arial_IsInstalled()
    {
        // Arrange 
        var r = new WindowsFontResolver();

        const string fontFamily = "Arial";

        // Act  
        var result = r.ResolveTypeface(fontFamily, false, false);

        // Assert
        Assert.That(result.FaceName, Is.EqualTo(fontFamily.ToUpperInvariant()));
    }

    [Test]
    public void ResolveTypeface_ArialBold_IsInstalled()
    {
        // Arrange 
        var r = new WindowsFontResolver();

        const string fontFamily = "Arial";

        // Act  
        var result = r.ResolveTypeface(fontFamily, true, false);

        // Assert
        Assert.That(result.FaceName, Is.EqualTo($"{fontFamily} BOLD".ToUpperInvariant()));
    }

    [Test]
    public void ResolveTypeface_ArialItalic_IsInstalled()
    {
        // Arrange 
        var r = new WindowsFontResolver();

        const string fontFamily = "Arial";

        // Act  
        var result = r.ResolveTypeface(fontFamily, false, true);

        // Assert
        Assert.That(result.FaceName, Is.EqualTo($"{fontFamily} ITALIC".ToUpperInvariant()));
    }

    [Test]
    public void ResolveTypeface_ArialBoldItalic_IsInstalled()
    {
        // Arrange 
        var r = new WindowsFontResolver();

        const string fontFamily = "Arial";

        // Act  
        var result = r.ResolveTypeface(fontFamily, true, true);

        // Assert
        Assert.That(result.FaceName, Is.EqualTo($"{fontFamily} BOLD ITALIC".ToUpperInvariant()));
    }

    [Test]
    public void ResolveTypeface_NotInstalled_IsInstalled()
    {
        // Arrange 
        var r = new WindowsFontResolver();

        const string fontFamily = "Blubb";

        // Act  
        var result = r.ResolveTypeface(fontFamily, true, false);

        // Assert
        Assert.That(result.FaceName, Is.EqualTo("Aptos"));
    }

    [Test]
    public void GetFont_ArialBoldItalic_FileLoaded()
    {
        // Arrange 
        var r = new WindowsFontResolver();

        const string fontFamily = "Arial";

        var fontInfo = r.ResolveTypeface(fontFamily, true, true);

        // Act  
        var result = r.GetFont(fontInfo.FaceName);

        // Assert
        Assert.That(result.Length, Is.Not.EqualTo(0));
    }
}