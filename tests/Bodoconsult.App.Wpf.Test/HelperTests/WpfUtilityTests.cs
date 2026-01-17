// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Controls;
using Bodoconsult.App.Wpf.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Test.HelperTests;

[TestFixture]
[RequiresThread(ApartmentState.STA)]
[SupportedOSPlatform("windows")]
public class WpfUtilityTests
{
    //[Test]
    //public void TestFindResource_OnlyRessourceName()
    //{

    //    var brush = (Brush)WpfUtility.FindResource("BackgroundBrush02");

    //    Assert.IsNotNull(brush);
    //}

    private const string XamlFile = @"C:\temp\XamlTestFile.xaml";

    [Test]
    public void TestSaveElementAsXamlFile()
    {
        if (File.Exists(XamlFile))
        {
            File.Delete(XamlFile);
        }

        var button = new Button { Content = "Hallo" };

        WpfHelper.SaveElementAsXamlFile(button, XamlFile);

        Assert.That(File.Exists(XamlFile));
    }


    [Test]
    public void TestLoadElementFromXamlFile()
    {
        if (File.Exists(XamlFile))
        {
            File.Delete(XamlFile);
        }

        var button = new Button { Content = "Hallo" };

        WpfHelper.SaveElementAsXamlFile(button, XamlFile);

        Assert.That(File.Exists(XamlFile));

        var buttonErg = (Button)WpfHelper.LoadElementFromXamlFile(XamlFile);

        Assert.That(buttonErg != null);
        Assert.That(buttonErg.Content.ToString() == "Hallo");
    }

}