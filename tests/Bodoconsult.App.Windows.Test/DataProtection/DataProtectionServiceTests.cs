// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Windows.DataProtection;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.DataProtection;

[SupportedOSPlatform("windows")]
[TestFixture]
internal class DataProtectionServiceTests
{
    private DataProtectionService _service;

    [SetUp]
    public void Setup()
    {
        _service = new DataProtectionService();
    }

    [Test]
    public void ProtectStringUnprotectString_CurrentUser_Successful()
    {
        const string secret = "Test123!";

        _service.CurrentDataProtectionScope = DataProtectionScope.CurrentUser;

        //Encrypt the data.
        var encryptedSecret = _service.ProtectString(secret);
        Debug.Print($"The encrypted byte array is: {ArrayHelper.GetStringFromArray(encryptedSecret)}");


        // Decrypt the data and store in a byte array.
        var originalData = _service.UnprotectString(encryptedSecret);
        Debug.Print($"The original data is: {originalData}"); 

        Assert.That(originalData, Is.EqualTo(secret));
    }

    [Test]
    public void ProtectStringUnprotectString_LocalMachine_Successful()
    {
        const string secret = "Test123!";

        _service.CurrentDataProtectionScope = DataProtectionScope.LocalMachine;

        //Encrypt the data.
        var encryptedSecret = _service.ProtectString(secret);
        Debug.Print($"The encrypted byte array is: {ArrayHelper.GetStringFromArray(encryptedSecret)}");


        // Decrypt the data and store in a byte array.
        var originalData = _service.UnprotectString(encryptedSecret);
        Debug.Print($"The original data is: {originalData}");

        Assert.That(originalData, Is.EqualTo(secret));
    }

    [Test]
    public void ProtectUnprotect_CurrentUser_Successful()
    {
        const string secret = "Test123!";
        const string key = "key";

        _service.CurrentDataProtectionScope = DataProtectionScope.LocalMachine;

        //Encrypt the data.
        var encryptedSecret = _service.Protect(key, secret);
        Debug.Print($"The encrypted byte array is: {encryptedSecret}");


        // Decrypt the data and store in a byte array.
        var originalData = _service.Unprotect(key, encryptedSecret);
        Debug.Print($"The original data is: {originalData}");

        Assert.That(originalData, Is.EqualTo(secret));
    }

    [Test]
    public void ProtectUnprotect_LocalMachine_Successful()
    {
        const string secret = "Test123!";
        const string key = "key";

        _service.CurrentDataProtectionScope = DataProtectionScope.LocalMachine;

        //Encrypt the data.
        var encryptedSecret = _service.Protect(key, secret);
        Debug.Print($"The encrypted byte array is: {encryptedSecret}");


        // Decrypt the data and store in a byte array.
        var originalData = _service.Unprotect(key, encryptedSecret);
        Debug.Print($"The original data is: {originalData}");

        Assert.That(originalData, Is.EqualTo(secret));
    }
}