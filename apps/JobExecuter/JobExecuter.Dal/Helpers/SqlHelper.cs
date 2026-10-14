// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Data.SqlClient;
using Bodoconsult.Database.Interfaces;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Helepr class to handle SQL related tasks
/// </summary>
internal class SqlHelper
{
    /// <summary>
    /// Get status messages from database stored procs
    /// </summary>
    private SqlStatus Status { get; set; }

    private readonly SqlConnection _conn;

    /// <summary>
    /// default ctor
    /// </summary>
    /// <param name="connectionString">Current connection string</param>
    /// <param name="status">SQL status</param>
    public SqlHelper(string connectionString, SqlStatus status)
    {
        Status = status;
        _conn = new SqlConnection(connectionString);
        _conn.Open();
        if (status !=null)
        {
            _conn.InfoMessage += SendInfo;
        }
    }

    private void SendInfo(object sender, SqlInfoMessageEventArgs e)
    {
        Status(e.ToString());
    }

    /// <summary>
    /// Execute SQL statement with not returning values
    /// </summary>
    /// <param name="sql">SQL statement to execute</param>
    public void Exec(string sql)
    {
        var cmd = new SqlCommand(sql, _conn);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Execute SQL statement and return one value
    /// </summary>
    /// <param name="sql">SQL to execute</param>
    /// <returns>Result of the executed SQL statement as string</returns>
    public string ExecWithResult(string sql)
    {
        var cmd = new SqlCommand(sql, _conn);
        return cmd.ExecuteScalar()?.ToString();
    }

    /// <summary>
    /// Close the connection
    /// </summary>
    public void Close()
    {
        _conn.Close();
        _conn.Dispose();
    }
}