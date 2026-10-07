// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataProtection.FileProtection;
using NUnit.Framework;
using System.Security.Cryptography;

namespace Bodoconsult.App.Windows.Test.DataProtection;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
internal class DataProtectionManagerUserScopeSimpleFileProtectionServiceTests : BaseDataProtectionManagerTests
{
    public DataProtectionManagerUserScopeSimpleFileProtectionServiceTests()
    {
        FileProtectionService = new SimpleFileProtectionService();
        Extension = "dat";
        CurrentDataProtectionScope = DataProtectionScope.CurrentUser;
    }
}