// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFtpTransferCore.Business.Helpers;
using NUnit.Framework;

namespace BodoFtpTransferCore.Business.Test;

[TestFixture]
public class UnitTestsPasswordHelper
{
    [Test]
    public void TestEncrypt()
    {
        // Arrange 
        var p = "c28d7adc74_BodoFtp2022!";

        // Act  
        var p1 = PasswordHelper.Encrypt(p);

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(p1));

    }
}