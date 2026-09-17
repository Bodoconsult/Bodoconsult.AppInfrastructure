using BodoFileTransferCore.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test;

[TestFixture]
public class UnitTestDummyDataHandler: BaseTestDataHandler
{

    [SetUp]
    public void Setup()
    {
        DataHandler = TestHelper.GetDummyDataHandler();
    }
}