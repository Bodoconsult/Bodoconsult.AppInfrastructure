// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Bodoconsult.App.Wpf.ReactiveUI.Helper;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.HelperTests;

[TestFixture]
public class WpfBaseStyleExtensionTests
{
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