using System.IO;
using Bodoconsult.Core.Database;
using Bodoconsult.Core.Database.MetaData;
using Bodoconsult.Core.Database.Sqlite;
using Bodoconsult.Core.Database.Sqlite.MetaData;
using BodoFtpTransferCore.Business.Test.Helpers;
using NUnit.Framework;

namespace BodoFtpTransferCore.Business.Test
{
    [TestFixture]
    public class UnitTestsMetaData
    {
        private IConnManager _db;
        
        private string _databaseFileName;

        private string _conn;

        [SetUp]
        public void Setup()
        {
            _databaseFileName = Path.Combine(TestHelper.OutputPath, TestHelper.DatabaseFileName);

            if (File.Exists(_databaseFileName)) File.Delete(_databaseFileName);

            _conn = $"Data Source={_databaseFileName};";

            _db = new SqliteConnManager(_conn);
            _db.TestConnection();
        }




        [Test]
        public void TestCreateDatabase()
        {
            // Act / Assert
            Assert.IsTrue(File.Exists(_databaseFileName));
        }


        [Test]
        public void TestCreateDatabaseWithTable()
        {
            // Arrange
            Assert.IsTrue(File.Exists(_databaseFileName));

            var sql = TestHelper.GetTextResource("BodoFtpTransferCore.Business.Test.Resources.Database.sql");

            // Act
            _db.Exec(sql);

            // Assert
            var sqlResult = "SELECT * FROM \"FtpFiles\";";

            var result = _db.GetDataReader(sqlResult);

            Assert.IsFalse(result.IsClosed);
        }

        [Test]
        public void GetMetaData()
        {

            
            var sql = TestHelper.GetTextResource("BodoFtpTransferCore.Business.Test.Resources.Database.sql");

            // Act
            _db.Exec(sql);
            
            

            var sqlResult = "SELECT * FROM \"FtpFiles\";";

            IMetaDataService mds = new SqliteMetaDataService();


            mds.GetMetaData(_conn, "Files", sqlResult, "ID");


            mds.ExportAll(TestHelper.OutputPath);
        }
    }
}