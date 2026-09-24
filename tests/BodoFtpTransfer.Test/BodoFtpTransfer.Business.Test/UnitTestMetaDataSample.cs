using System.IO;
using System.Linq;
using Bodoconsult.Core.Database.Sqlite;
using BodoFtpTransferCore.Business.Test.Helpers;
using BodoFtpTransferCore.Business.Test.MetaDataSamples;
using NUnit.Framework;

namespace BodoFtpTransferCore.Business.Test
{
    [TestFixture]
    public class UnitTestMetaDataSample
    {

        private FilesService _db;

        [SetUp]
        public void Setup()
        {
            var databaseFileName = Path.Combine(TestHelper.OutputPath, TestHelper.DatabaseFileName);

            if (File.Exists(databaseFileName)) File.Delete(databaseFileName);

            var conn = $"Data Source={databaseFileName};";

            var dbs = new SqliteConnManager(conn);
            dbs.TestConnection();

            var sql = TestHelper.GetTextResource("BodoFtpTransferCore.Business.Test.Resources.Database.sql");

            dbs.Exec(sql);

            _db = new FilesService(conn);

        }

        [Test]
        public void TestGetAll()
        {

            // Arrange
            AddNew();

            // Act
            var result = _db.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());

        }

        [Test]
        public void TestGetById()
        {

            var file = AddNew();

            // Act
            var result = _db.GetById(file.ID);

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(file.ID, result.ID);
        }

        [Test]
        public void TestCount()
        {
            // Arrange
            AddNew();

            // Act
            var result = _db.Count();

            // Assert
            Assert.IsTrue(result > 0);

        }


        private FtpFiles AddNew()
        {
            int id = _db.Count() + 1;

            var customer = TestDataHelper.NewFiles();
            customer.ID = id;

            // Act
            _db.AddNew(customer);

            return customer;
        }


        //[Test]
        //public void TestUpdate()
        //{
        //    const int id = 28;

        //    // Act
        //    var customer = _db.GetById(id);

        //    var newFax = customer.Fax == "" ? "08106-43425" : "";

        //    customer.Fax = newFax;

        //    // Act
        //    _db.Update(customer);

        //    // Assert
        //    var result = _db.GetById(id);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(newFax, result.Fax);

        //}


        [Test]
        public void TestAddNew()
        {
            var customer = AddNew();

            // Act
            _db.AddNew(customer);

            // Assert
            var result = _db.GetById(customer.ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(customer.Path, result.Path);

        }

        [Test]
        public void TestDelete()
        {
            // Assert
            var file = AddNew();
            // Act
            _db.Delete(file.ID);

            // Assert
            Assert.IsTrue(_db.Count()==0);

        }
    }
}