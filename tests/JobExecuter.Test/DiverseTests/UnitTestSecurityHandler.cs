//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System.Diagnostics;
//using NUnit.Framework;

//namespace JobExecuter.Test.DiverseTests;

//[TestFixture]
//public class UnitTestSecurityHandler
//{
//    [Test]
//    public void TestGetCreateSalt()
//    {
//        var salt = SecurityHandler.CreateSalt();

//        var hash = SecurityHandler.CreateHash("Test123");

//        Assert.That(!string.IsNullOrEmpty(salt));
//    }

//    [Test]
//    public void TestGetCreateHash()
//    {
//        SecurityHandler.LoadAlldata();

//        var hash = SecurityHandler.CreateHash("Test123");

//        Assert.That(hash.Length > 0);
//        Debug.Print(hash);
//    }

//    [Test]
//    public void TestGetCreateHashTwoTimes()
//    {
//        SecurityHandler.LoadAlldata();

//        var hash1 = SecurityHandler.CreateHash("Test123");

//        Assert.That(hash1.Length > 0);
//        Debug.Print(hash1);

//        var hash2 = SecurityHandler.CreateHash("Test123");

//        Assert.That(hash2.Length > 0);
//        Debug.Print(hash2);

//        Assert.That(hash1, Is.EqualTo(hash2));
//    }

//    [Test]
//    public void TestValidateHash()
//    {
//        var password = "Test123";

//        SecurityHandler.LoadAlldata();

//        var hash = SecurityHandler.ValidatePassword1(password);

//        Assert.That(hash);
//    }
//}