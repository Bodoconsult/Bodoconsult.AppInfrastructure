using System;
using System.Collections.Generic;
using Bodoconsult.Core.Database;
using Bodoconsult.Core.Database.Sqlite;
using Microsoft.Data.Sqlite;

namespace BodoFtpTransferCore.Business.Test.MetaDataSamples
{
    public class FilesService
    {

        private readonly IConnManager _db;

        public FilesService(string connectionString)
        {
            _db = new SqliteConnManager(connectionString);
        }

        /// <summary>
        /// Insert a data row into table Files from entity class Files object
        /// </summary>
        public void AddNew(FtpFiles item)
        {

            const string sql = "INSERT INTO \"FtpFiles\"(\"Path\", \"HashCode\", \"PathRemote\", \"Source\", \"HashCodeNew\", \"Type\", \"Size\", \"SizeRemote\") " +
                               "VALUES (@Path, @HashCode, @PathRemote, @Source, @HashCodeNew, @Type, @Size, @SizeRemote)";

            var cmd = new SqliteCommand(sql);

            SqliteParameter p;

            // Parameter @ID
            //p = new SqliteParameter("@ID", SqliteType.Integer) { Value = item.ID };
            //cmd.Parameters.Add(p);

            // Parameter @Path
            p = new SqliteParameter("@Path", SqliteType.Text) { Value = item.Path };
            cmd.Parameters.Add(p);

            // Parameter @HashCode
            p = new SqliteParameter("@HashCode", SqliteType.Text) { Value = item.HashCode };
            cmd.Parameters.Add(p);

            // Parameter @PathRemote
            p = new SqliteParameter("@PathRemote", SqliteType.Text) { Value = item.PathRemote };
            cmd.Parameters.Add(p);

            // Parameter @Source
            p = new SqliteParameter("@Source", SqliteType.Text) { Value = item.Source };
            cmd.Parameters.Add(p);

            // Parameter @HashCodeNew
            p = new SqliteParameter("@HashCodeNew", SqliteType.Text) { Value = item.HashCodeNew };
            cmd.Parameters.Add(p);

            // Parameter @Type
            p = new SqliteParameter("@Type", SqliteType.Text) { Value = item.Type };
            cmd.Parameters.Add(p);

            // Parameter @Size
            p = new SqliteParameter("@Size", SqliteType.Integer) { Value = item.Size };
            cmd.Parameters.Add(p);

            // Parameter @SizeRemote
            p = new SqliteParameter("@SizeRemote", SqliteType.Integer) { Value = item.SizeRemote };
            cmd.Parameters.Add(p);

            _db.Exec(cmd);

        }

        /// <summary>
        /// Update a data row in table Files from an entity class Files object
        /// </summary>
        public void Update(FtpFiles item)
        {

            const string sql = "UPDATE \"FtpFiles\" SET \"Path\"=@Path, \"HashCode\"=@HashCode, \"PathRemote\"=@PathRemote, \"Source\"=@Source, \"HashCodeNew\"=@HashCodeNew, \"Type\"=@Type, \"Size\"=@Size, \"SizeRemote\"=@SizeRemote WHERE \"ID\"=@ID; ";

            var cmd = new SqliteCommand(sql);

            SqliteParameter p;

            // Parameter @ID
            p = new SqliteParameter("@ID", SqliteType.Integer) { Value = item.ID };
            cmd.Parameters.Add(p);

            // Parameter @Path
            p = new SqliteParameter("@Path", SqliteType.Text) { Value = item.Path };
            cmd.Parameters.Add(p);

            // Parameter @HashCode
            p = new SqliteParameter("@HashCode", SqliteType.Text) { Value = item.HashCode };
            cmd.Parameters.Add(p);

            // Parameter @PathRemote
            p = new SqliteParameter("@PathRemote", SqliteType.Text) { Value = item.PathRemote };
            cmd.Parameters.Add(p);

            // Parameter @Source
            p = new SqliteParameter("@Source", SqliteType.Text) { Value = item.Source };
            cmd.Parameters.Add(p);

            // Parameter @HashCodeNew
            p = new SqliteParameter("@HashCodeNew", SqliteType.Text) { Value = item.HashCodeNew };
            cmd.Parameters.Add(p);

            // Parameter @Type
            p = new SqliteParameter("@Type", SqliteType.Text) { Value = item.Type };
            cmd.Parameters.Add(p);

            // Parameter @Size
            p = new SqliteParameter("@Size", SqliteType.Integer) { Value = item.Size };
            cmd.Parameters.Add(p);

            // Parameter @SizeRemote
            p = new SqliteParameter("@SizeRemote", SqliteType.Integer) { Value = item.SizeRemote };
            cmd.Parameters.Add(p);

            _db.Exec(cmd);

        }

        /// <summary>
        /// Delete a row from table Files 
        /// </summary>
        public void Delete(System.Int64 ID)
        {

            var cmd = new SqliteCommand("DELETE FROM \"FtpFiles\" WHERE \"ID\" = @PK");

            var p = new SqliteParameter("@PK", SqliteType.Integer) { Value = ID };
            cmd.Parameters.Add(p);

            _db.Exec(cmd);

        }

        /// <summary>
        /// Get all rows in table Files
        /// </summary>
        public IList<FtpFiles> GetAll()
        {

            var result = new List<FtpFiles>();

            var reader = _db.GetDataReader("SELECT * FROM \"FtpFiles\"");

            while (reader.Read())
            {
                var dto = DataHelper.MapFromDbToFiles(reader);
                result.Add(dto);

            }

            reader.Dispose();

            return result;
        }

        /// <summary>
        /// Get all rows in table Files
        /// </summary>
        public FtpFiles GetById(System.Int64 pkID)
        {

            FtpFiles dto = null;

            var reader = _db.GetDataReader($"SELECT * FROM \"FtpFiles\" WHERE \"ID\"={pkID};");

            while (reader.Read())
            {
                dto = DataHelper.MapFromDbToFiles(reader);
                break;

            }

            reader.Dispose();

            return dto;
        }

        /// <summary>
        /// Count all rows in table Files 
        /// </summary>
        public int Count()
        {

            var result = _db.ExecWithResult("SELECT COUNT(*) FROM \"FtpFiles\"");

            return Convert.ToInt32(result);
        }


    }
}