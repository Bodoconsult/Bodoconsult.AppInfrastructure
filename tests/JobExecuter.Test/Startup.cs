// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test;

[TestFixture]
public static class Startup
{
    [OneTimeSetUp]
    public static void Configure(TestContext tc)
    {
        if (!Directory.Exists(TestDataHelper.TempPath))
        {
            Directory.CreateDirectory(TestDataHelper.TempPath);
        }
    }
}