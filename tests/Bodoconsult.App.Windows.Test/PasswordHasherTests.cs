//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using Bodoconsult.App.Windows.DataProtection.Hashing;
//using NUnit.Framework;

//namespace Bodoconsult.App.Windows.Test;

//[TestFixture]
//public class PasswordHasherTests
//{
//    [Test]
//    public void TestHash()
//    {

//        var secret = "Test123!";

//        var options = new HashingOptions
//        {
//            Iterations = 10000
//        };

//        var service = new PasswordHasher(options);

//        // Act hash
//        var hash = service.Hash(secret);

//        // Act
//        Assert.That(hash, Is.Not.Null);
//        Assert.That(hash.Length > 0);
//    }


//    [Test]
//    public void TestCheck()
//    {

//        var secret = "Test123!";

//        var options = new HashingOptions
//        {
//            Iterations = 10000
//        };

//        var service = new PasswordHasher(options);

//        // Hash
//        var hash = service.Hash(secret);

//        Assert.That(hash, Is.Not.Null);
//        Assert.That(hash.Length > 0);


//        // Act
//        var result = service.Check(hash, secret);

//        Assert.That(result.Verified);
//    }
//}