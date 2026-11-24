// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Data;
using Microsoft.Data.SqlClient;

namespace Bodoconsult.App.WinForms;

/// <summary>
/// Datenbindung an eine SQL-Datenbank für ein DataGridView
/// </summary>
public class DataGridBinding
{
    private SqlDataAdapter _da;
    private SqlCommandBuilder _cmd;
    private DataTable _dt = new();

    /// <summary>
    /// Set SqlDataAdapter for data binding
    /// </summary>
    /// <param name="da">Data adapter</param>
    public void SetAdapter(SqlDataAdapter da)
    {
        _da = da;
        _cmd = new SqlCommandBuilder(_da);
        _da.Fill(_dt);
    }
    /// <summary>
    /// Fetch DataTable to show it in the DataGridView
    /// </summary>
    /// <returns>DataTable</returns>
    public DataTable GetTable()
    {
        return _dt;
    }

    /// <summary>
    /// Set a <see cref="DataTable"/>
    /// </summary>
    /// <param name="dt"></param>
    public void SetTable(ref DataTable dt)
    {
        _dt = dt;
    }

    /// <summary>
    /// Is the data tbale changed?
    /// </summary>
    public bool IsChanged => _dt.GetChanges() != null;

    /// <summary>
    /// Änderungen speichern
    /// </summary>
    public void Save()
    {
        //try
        //{
        var d = _dt.GetChanges();
        if (d == null) return;
        _da.Update(d);
        _dt.AcceptChanges();
        //}
        //catch 
        //{

        //}
    }

    /// <summary>
    /// Check if there are unsaved changes and save them
    /// </summary>
    public void CheckSave()
    {
        if (!IsChanged)
        {
            return;
        }

        if (MessageBox.Show("Wollen Sie die geändeten Daten?",
                "Speichern", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
            DialogResult.No) { return; }

        Save();
    }

}