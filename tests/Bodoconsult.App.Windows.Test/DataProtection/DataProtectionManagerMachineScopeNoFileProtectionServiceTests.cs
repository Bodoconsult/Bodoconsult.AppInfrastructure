// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Security.Cryptography;
using Bodoconsult.App.DataProtection.FileProtection;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.DataProtection;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
internal class DataProtectionManagerMachineScopeNoFileProtectionServiceTests : BaseDataProtectionManagerTests
{
    public DataProtectionManagerMachineScopeNoFileProtectionServiceTests()
    {
        FileProtectionService = new NoFileProtectionService();
        Extension = "json";
        CurrentDataProtectionScope = DataProtectionScope.LocalMachine;
    }
}