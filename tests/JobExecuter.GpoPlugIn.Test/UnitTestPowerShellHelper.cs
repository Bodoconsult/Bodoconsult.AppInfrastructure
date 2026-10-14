// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using JobExecuter.GpoPlugIn.Helper;
using JobExecuter.GpoPlugIn.Model;
using NUnit.Framework;

namespace JobExecuter.GpoPlugIn.Test;

[TestFixture]
public class UnitTestPowerShellHelper
{
    [Test]
    public void TestExecute()
    {
        // Arrange
        var script = "Get-GPO -domain bcg-ad.de -all";

        // Act

        var results = PowerShellHelper.Execute(script);

        // Assert
        Assert.That(results, Is.Not.Null);


        // print the resulting pipeline objects to the console.
        foreach (var item in results)
        {



            Debug.Print(item.BaseObject.ToString());
        }

    }



}


[TestFixture]
public class UnitTestGpoDomain
{


    [Test]
    public void TestCtor()
    {
        // Arrange
        var domainName = "bcg-ad.de";

        // Act
        var d = new GpDomain(domainName);

        // Assert
        Assert.That(!string.IsNullOrEmpty( d.Domain));
    }


    [Test]
    public void TestGetAllGpos()
    {
        // Arrange
        var domainName = "bcg-ad.de";

            
        var d = new GpDomain(domainName);
        Assert.That(!string.IsNullOrEmpty(d.Domain));

        // Act
        d.GetAllGpos();

        // Assert
        var gpos = d.Gpos;
        Assert.That(gpos, Is.Not.Null);
        Assert.That(gpos.Any());

        // print the resulting pipeline objects to the console.
        foreach (var item in gpos)
        {
            Debug.Print(item.DisplayName);
        }
    }

}