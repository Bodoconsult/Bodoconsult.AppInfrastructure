//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Bodoconsult.App.Wpf.Helpers;
//using NUnit.Framework;

//namespace Bodoconsult.App.Wpf.Test;

//[TestFixture]
//public class ResourceFinderServiceTests
//{

//    private const string German =
//        "pack://application:,,,/Bodoconsult.App.Wpf.Test;component/Resources/Culture.de.xaml";

//    private const string English =
//        "pack://application:,,,/Bodoconsult.App.Wpf.Test;component/Resources/Culture.en.xaml";


//    ///// <summary>
//    ///// Constructor to initialize class
//    ///// </summary>
//    //public UnitTestResourceFinderService()
//    //{

//    //}


//    /// <summary>
//    /// Runs in front of each test method
//    /// </summary>
//    [SetUp]
//    public void Setup()
//    {
//        ResourceFinderHelper.ClearCache();
//    }

//    ///// <summary>
//    ///// Cleanup aft test methods
//    ///// </summary>
//    //[TestCleanup]
//    //public void Cleanup()
//    //{


//    //}




//    [Test]
//    public void FindResource_ExistingResources_ResourcesFound()
//    {
//        //Arrange

//        // Act
//        var erg = (string)ResourceFinderHelper.FindResource(German, "Simulation.Asset");

//        //Assert
//        Assert.That(erg, Is.EqualTo("Gegenstand"));

//        erg = (string)ResourceFinderHelper.FindResource(English, "Simulation.Asset");
//        Assert.That(erg, Is.EqualTo("Asset"));

//        erg = (string)ResourceFinderHelper.FindResource(German, "Simulation.Asset");

//        //Assert
//        Assert.That(erg, Is.EqualTo("Gegenstand"));

//        Assert.That(ResourceFinderHelper.Count, Is.EqualTo(2));
//    }


//    [Test]
//    public void SetResource_ExistingResource_ResourceIsSet()
//    {
//        //Arrange

//        // Act
//        var erg = (string)ResourceFinderHelper.FindResource(German, "Simulation.Asset");

//        //Assert
//        Assert.That(erg, Is.EqualTo("Gegenstand"));

//        ResourceFinderHelper.SetResource(German, "Simulation.Asset", "Asset");
//        erg = (string)ResourceFinderHelper.FindResource(German, "Simulation.Asset");
//        Assert.That(erg, Is.EqualTo("Asset"));
//        ResourceFinderHelper.SetResource(German, "Simulation.Asset", "Gegenstand");

//        erg = (string)ResourceFinderHelper.FindResource(German, "Simulation.Asset");

//        //Assert
//        Assert.That(erg, Is.EqualTo("Gegenstand"));

//        Assert.That(ResourceFinderHelper.Count, Is.EqualTo(1));
//    }

//    [Test]
//    public void SetResourceGeneric_ExistingResource_ResourceIsSet()
//    {
//        //Arrange

//        // Act
//        var erg = (string)ResourceFinderHelper.FindResource(German, "Simulation.Asset");

//        //Assert
//        Assert.That(erg == "Gegenstand");

//        ResourceFinderHelper.SetResource<string>(German, "Simulation.Asset", "Asset");
//        erg = (string)ResourceFinderHelper.FindResource(German, "Simulation.Asset");
//        Assert.That(erg == "Asset");
//        ResourceFinderHelper.SetResource<string>(German, "Simulation.Asset", "Gegenstand");

//        erg = (string)ResourceFinderHelper.FindResource(German, "Simulation.Asset");

//        //Assert
//        Assert.That(erg == "Gegenstand");

//        Assert.That(ResourceFinderHelper.Count, Is.EqualTo(1));
//    }
//}