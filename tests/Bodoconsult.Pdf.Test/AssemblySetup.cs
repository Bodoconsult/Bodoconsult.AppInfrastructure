// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.PdfSharp;
using MigraDoc;
using NUnit.Framework;
using PdfSharp;
using PdfSharp.Fonts;

namespace Bodoconsult.Pdf.Test;

/// <summary>
/// Setup for the assembly for all tests
/// </summary>
[SetUpFixture]
public static class AssemblySetup
{
    /// <summary>
    /// At startup of the assembly
    /// </summary>
    [OneTimeSetUp]
    public static void AssemblyStartUp()
    {
        GlobalFontSettings.UseWindowsFontsUnderWindows = true;
        //GlobalFontSettings.UseWindowsFontsUnderWsl2 = true;
        PdfSharpLicense.SetAgplLicense();
        GlobalFontSettings.FontResolver = new WindowsFontResolver();

        PredefinedFontsAndChars.ErrorFontName = "Arial";
    }
}