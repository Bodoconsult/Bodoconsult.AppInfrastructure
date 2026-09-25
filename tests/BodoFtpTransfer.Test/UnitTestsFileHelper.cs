using System.IO;
using BodoFtpTransferCore.Business.Helpers;
using BodoFtpTransferCore.Business.Test.Helpers;
using NUnit.Framework;

namespace BodoFtpTransferCore.Business.Test
{
    [TestFixture]
    public class UnitTestsFileHelper
    {
        //[SetUp]
        //public void Setup()
        //{
        //}

        [Test]
        public void TestGetHashCode()
        {

            // Arrange
            var fileName = Path.Combine(TestHelper.TestDataPath, "Lungau_2012_077.JPG");

            // Act
            var hash = FileHelper.GetHashCode(fileName);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(hash));
        }
    }
}