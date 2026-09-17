using BodoFileTransferCore.Business.App;
using BodoFileTransferCore.Business.DataHandling;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test;

[TestFixture]
public class UnitTestDataHandler: BaseTestDataHandler
{

    [SetUp]
    public void Setup()
    {
        DataHandler = new DataHandler(GlobalValues.CurrentAppSettings);

        DataHandler.DeleteFile(TestFilePath);

    }




}