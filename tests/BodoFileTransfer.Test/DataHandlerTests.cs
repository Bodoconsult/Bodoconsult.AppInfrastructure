// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFileTransfer.Business.DataHandling;
using BodoFileTransfer.Test.App;
using NUnit.Framework;

namespace BodoFileTransfer.Test;

[TestFixture]
public class DataHandlerTests: BaseTestDataHandler
{

    [SetUp]
    public void Setup()
    {
        DataHandler = new DataHandler(Globals.Instance);
        DataHandler.DeleteFile(TestFilePath);
    }




}