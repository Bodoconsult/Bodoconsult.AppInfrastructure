// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Security.Cryptography;
using Bodoconsult.App.DataProtection.FileProtection;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.DataProtection;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
internal class DataProtectionManagerMachineScopeSimpleFileProtectionServiceTests : BaseDataProtectionManagerTests
{
    public DataProtectionManagerMachineScopeSimpleFileProtectionServiceTests()
    {
        FileProtectionService = new SimpleFileProtectionService();
        Extension = "dat";
        CurrentDataProtectionScope = DataProtectionScope.LocalMachine;
    }
}