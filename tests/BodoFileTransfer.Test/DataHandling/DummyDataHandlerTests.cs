// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFileTransfer.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Test.DataHandling;

[TestFixture]
public class DummyDataHandlerTests: BaseTestDataHandler
{

    [SetUp]
    public void Setup()
    {
        DataHandler = TestHelper.GetDummyDataHandler();
    }
}