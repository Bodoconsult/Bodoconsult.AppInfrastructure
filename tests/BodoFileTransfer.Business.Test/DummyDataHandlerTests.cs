// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFileTransfer.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Business.Test;

[TestFixture]
public class DummyDataHandlerTests: BaseTestDataHandler
{

    [SetUp]
    public void Setup()
    {
        DataHandler = TestHelper.GetDummyDataHandler();
    }
}