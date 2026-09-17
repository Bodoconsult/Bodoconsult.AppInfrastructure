// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFileTransfer.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Business.Test;

internal class MailTestHelperTests
{
    [Test]
    public void TestGetO365Credentials()
    {
        // Act
        var result = TestHelper.GetTestO365Account();

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}