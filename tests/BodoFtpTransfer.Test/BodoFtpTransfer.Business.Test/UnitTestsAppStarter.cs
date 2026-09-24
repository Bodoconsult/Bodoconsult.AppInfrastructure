using System.Diagnostics;
using BodoFtpTransferCore.Business.App;
using NUnit.Framework;

namespace BodoFtpTransferCore.Business.Test;

[TestFixture]
public class UnitTestsAppStarter
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void TestStart()
    {
        AppStarter.Start(0, SetMessage);
    }


    public static void SetMessage(string message)
    {
        Debug.Print(message);
    }
}