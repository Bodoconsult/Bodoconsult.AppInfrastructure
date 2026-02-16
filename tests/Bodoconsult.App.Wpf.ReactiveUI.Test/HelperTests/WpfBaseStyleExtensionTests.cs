// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.Helper;
using NUnit.Framework;
using System;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.HelperTests;

[TestFixture]
public class WpfBaseStyleExtensionTests
{
    public WpfBaseStyleExtensionTests()
    {
        var s = System.IO.Packaging.PackUriHelper.UriSchemePack;

        //const string scheme = "pack";
        //if (!UriParser.IsKnownScheme(scheme))
        //{
        //    Assert.That(PackUriHelper.UriSchemePack, Is.EqualTo(scheme));
        //}
        //else
        //{
        //    Assert.Fail("Pack scheme not found");
        //}
    }


    [Test]
    public void ProvideValue_ExistingStyleRowDefinition_StyleLoaded()
    {
        //Arrange
        var wpf = new WpfBaseResourceExtension();

        var rk = "GridRowNormalStyle";

        wpf.ResourceKey = rk;

        var erg = (Style)wpf.ProvideValue(null);

        Assert.That(erg, Is.Not.Null);
        Assert.That(erg.TargetType, Is.EqualTo(typeof(RowDefinition)));
    }

    [Test]
    public void ProvideValue_ExistingStyleBrush_StyleLoaded()
    {
        //Arrange
        var wpf = new WpfBaseResourceExtension();

        var rk = "InputBackgroundBrush01";

        wpf.ResourceKey = rk;

        var erg = (Brush)wpf.ProvideValue(null);

        Assert.That(erg, Is.Not.Null);
    }
}