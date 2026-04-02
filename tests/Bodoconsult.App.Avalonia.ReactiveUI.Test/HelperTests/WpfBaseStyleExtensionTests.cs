//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using Avalonia.Controls;
//using Avalonia.Media;
//using Avalonia.Styling;
//using NUnit.Framework;

//// ReSharper disable InconsistentNaming

//namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.HelperTests;

//[TestFixture]
//public class AvaloniaBaseStyleExtensionTests
//{
//    public AvaloniaBaseStyleExtensionTests()
//    {
//        var s = System.IO.Packaging.PackUriHelper.UriSchemePack;

//        //const string scheme = "pack";
//        //if (!UriParser.IsKnownScheme(scheme))
//        //{
//        //    Assert.That(PackUriHelper.UriSchemePack, Is.EqualTo(scheme));
//        //}
//        //else
//        //{
//        //    Assert.Fail("Pack scheme not found");
//        //}
//    }


//    [Test]
//    public void ProvideValue_ExistingStyleRowDefinition_StyleLoaded()
//    {
//        //Arrange
//        var Avalonia = new AvaloniaBaseResourceExtension();

//        var rk = "GridRowNormalStyle";

//        Avalonia.ResourceKey = rk;

//        var erg = (Style)Avalonia.ProvideValue(null);

//        Assert.That(erg, Is.Not.Null);
//        Assert.That(erg.TargetType, Is.EqualTo(typeof(RowDefinition)));
//    }

//    [Test]
//    public void ProvideValue_ExistingStyleBrush_StyleLoaded()
//    {
//        //Arrange
//        var Avalonia = new AvaloniaBaseResourceExtension();

//        var rk = "InputBackgroundBrush01";

//        Avalonia.ResourceKey = rk;

//        var erg = (Brush)Avalonia.ProvideValue(null);

//        Assert.That(erg, Is.Not.Null);
//    }
//}