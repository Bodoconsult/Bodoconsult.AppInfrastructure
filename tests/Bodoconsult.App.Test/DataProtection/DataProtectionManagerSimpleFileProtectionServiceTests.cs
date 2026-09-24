// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataProtection;
using Bodoconsult.App.DataProtection.FileProtection;

namespace Bodoconsult.App.Test.DataProtection;

[NonParallelizable]
[SingleThreaded]
internal class DataProtectionManagerSimpleFileProtectionServiceTests : BaseDataProtectionManagerTests
{
    public DataProtectionManagerSimpleFileProtectionServiceTests()
    {
        FileProtectionService = new SimpleFileProtectionService();
        Extension = "dat";
    }
}