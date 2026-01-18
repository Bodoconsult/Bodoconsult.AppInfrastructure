// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
internal class TypeHelperTests
{


    [Test]
    public void CurrentAssembly_DefaultSetup_AssemblyLoaded()
    {
        // Arrange 


        // Act  
        var ass = TypeHelper.CurrentAssembly;

        // Assert
        Assert.That(ass, Is.Not.Null);

    }

    [Test]
    public void GetTypesDerivedFrom_DefaultSetup_ListWithTypesReturned()
    {
        // Arrange 
            

        var baseType = typeof(IBusinessTransactionReply);
        var ass = typeof(IBusinessTransactionReply).Assembly;

        // Act  
        var result = TypeHelper.GetTypesDerivedFrom(ass, baseType);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.Not.EqualTo(0));

        foreach (var type in result)
        {
            Debug.Print(type.Name);
        }
    }

    [Test]
    public void GetTypesDerivedFromCurrentAssembly_DefaultSetup_ListWithTypesReturned()
    {
        // Arrange 
        var baseType = typeof(IBusinessTransactionReply);

        // Act  
        var result = TypeHelper.GetTypesDerivedFromCurrentAssembly(baseType);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));

        //foreach (var type in result)
        //{
        //    Debug.Print(type.Name);
        //}
    }

}