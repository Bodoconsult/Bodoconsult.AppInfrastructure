// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.App.DataProtection;
using Bodoconsult.App.DataProtection.FileProtection;

namespace Bodoconsult.App.Test.DataProtection;

[TestFixture]
internal class NoFileProtectionServiceTests
{
    private const string Secret = "Blubb";

    [Test]
    public void ProtectUnprotect_DefaultSettingsString_SecretUnprotectedSuccessfully()
    {
        // Arrange 
        var service = new NoFileProtectionService();

        var data = Encoding.UTF8.GetBytes(Secret);

        // Act  
        var result = service.Protect(data);

        var result2 = service.Unprotect(result);

        var secret2 = Encoding.UTF8.GetString(result2);

        // Assert
        Assert.That(secret2, Is.EqualTo(Secret));

        Assert.That(result, Is.EqualTo(result2));
    }
}