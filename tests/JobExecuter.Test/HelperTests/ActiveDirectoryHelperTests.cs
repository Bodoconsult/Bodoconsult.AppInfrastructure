// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using JobExecuter.Dal.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.HelperTests;

[TestFixture]
public class ActiveDirectoryHelperTests
{
    [Test]
    public void TestGetUsernamesDomainGroup()
    {
        // Arrange
        const string domain = "bcg-ad";
        const string group = "SG-BCG-Faktura-EÜRechnung";

        //Act
        var erg = ActiveDirectoryHelper.GetUsernames(domain, group);

        // Assert
        Assert.That(erg, Is.Not.Null);
        Assert.That(erg.Count > 0);

        foreach (var user in erg)
        {
            Debug.Print(user);
        }
    }

    [Test]
    public void TestGetUsernamesGroup()
    {
        // Arrange
        const string group = "bcg-ad\\SG-BCG-Faktura-EÜRechnung";

        //Act
        var erg = ActiveDirectoryHelper.GetUsernames(group);

        // Assert
        Assert.That(erg, Is.Not.Null);
        Assert.That(erg.Count > 0);

        foreach (var user in erg)
        {
            Debug.Print(user);
        }
    }

    [Test]
    public void TestGetUsername()
    {
        // Arrange
        const string user = "bcg-ad\\rleisner";

        //Act
        var erg = ActiveDirectoryHelper.GetAdUser(user);

        // Assert
        Assert.That(erg, Is.Not.Null);
    }
}