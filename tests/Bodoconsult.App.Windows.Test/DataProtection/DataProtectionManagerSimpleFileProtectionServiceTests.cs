// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataProtection.FileProtection;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.DataProtection;

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