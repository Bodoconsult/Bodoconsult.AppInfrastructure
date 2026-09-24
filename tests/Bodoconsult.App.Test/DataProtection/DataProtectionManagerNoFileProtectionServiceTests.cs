// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataProtection;
using Bodoconsult.App.DataProtection.FileProtection;

namespace Bodoconsult.App.Test.DataProtection;

[NonParallelizable]
[SingleThreaded]
internal class DataProtectionManagerNoFileProtectionServiceTests : BaseDataProtectionManagerTests
{
    public DataProtectionManagerNoFileProtectionServiceTests()
    {
        FileProtectionService = new NoFileProtectionService();
        Extension = "json";
    }
}
