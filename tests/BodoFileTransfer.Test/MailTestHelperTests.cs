// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFileTransfer.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Test;

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