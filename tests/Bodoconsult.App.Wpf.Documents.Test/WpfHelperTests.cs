// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Wpf.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Documents.Test;

[TestFixture]
public class WpfHelperTests
{
    ///// <summary>
    ///// Constructor to initialize class
    ///// </summary>
    //public UnitTestWpfUtility()
    //{

    //}


    ///// <summary>
    ///// Runs in front of each test method
    ///// </summary>
    //[TestInitialize]
    //public void Setup()
    //{


    //}

    ///// <summary>
    ///// Cleanup aft test methods
    ///// </summary>
    //[TestCleanup]
    //public void Cleanup()
    //{


    //}


    [Test]
    public void TestLoadPlainTextFile()
    {
        //Arrange
        const string fileName = @"pack://siteOfOrigin:,,,/Resources/Content/SimulationMethodDescription.txt";
        // Act

        var erg = WpfHelper.LoadPlainTextFile(fileName);

        //Assert
        Assert.That(string.IsNullOrEmpty(erg), Is.False);
    }
}