// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Data;

namespace Bodoconsult.Text.Test.Helpers;

public class DataHelper
{
    /// <summary>
    /// Get a test <see cref="DataTable"/>
    /// </summary>
    /// <returns></returns>
    public static DataTable GetDataTable()
    {
        var dt = new DataTable();
        dt.Columns.Add("ShareId", typeof(int));
        dt.Columns.Add("ShareName", typeof(string));
        dt.Columns.Add("WKN", typeof(string));
        dt.Columns.Add("ISIN", typeof(string));
        dt.Columns.Add("Domestic", typeof(bool));
        dt.Rows.Add(1, "Testfirma AG", "900900", "DE123456789", true);
        dt.Rows.Add(2, "Blubb AG", "123456", "AT123456789", false);
        dt.Rows.Add(3, "Blabb AG", "234567", "GB123456789", false);
        dt.Rows.Add(4, "Lustig AG", "345678", "DE234567891", true);
        dt.Rows.Add(5, "Unsinn AG", "456789", "DE345678912", true);
        return dt;
    }

    public static DataTable GetDefinitionListAsDataTable()
    {
        var dt = new DataTable();
        dt.Columns.Add("Key");
        dt.Columns.Add("Value");

        var item = dt.NewRow();
        item["Key"] = "Content";
        item["Value"] = TestDataHelper.MassText;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["Key"] = "Rights";
        item["Value"] = TestDataHelper.MassText;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["Key"] = "Key";
        item["Value"] = "500";
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["Key"] = "Values";
        item["Value"] = TestDataHelper.MassText;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["Key"] = "Blubbs";
        item["Value"] = TestDataHelper.MassText;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["Key"] = "Blabbs";
        item["Value"] = TestDataHelper.MassText;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["Key"] = "Blobbs";
        item["Value"] = TestDataHelper.MassText;
        dt.Rows.Add(item);

        return dt;
    }

    public static DataTable GetSmallDataTable()
    {
        var dt = new DataTable();
        dt.Columns.Add("Key");
        dt.Columns.Add("Value1").DataType = typeof(double);
        dt.Columns.Add("Value2").DataType = typeof(DateTime);
        dt.Columns.Add("Value3").DataType = typeof(bool);

        var item = dt.NewRow();
        item["Key"] = "Content";
        item["Value1"] = 5.97;
        item["Value2"] = DateTime.Now;
        item["Value3"] = true;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["Key"] = "Website";
        item["Value1"] = 65.97;
        item["Value2"] = DateTime.Now.AddDays(-25);
        item["Value3"] = false;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["Key"] = "Marketing";
        item["Value1"] = 45.97;
        item["Value2"] = DateTime.Now;
        item["Value3"] = true;
        dt.Rows.Add(item);

        return dt;
    }

    public static DataTable GetSmallDataTableWithCssInfo()
    {
        var dt = new DataTable();
        dt.Columns.Add("CssStyle");
        dt.Columns.Add("Key");
        dt.Columns.Add("Value1").DataType = typeof(double);
        dt.Columns.Add("Value2").DataType = typeof(DateTime);
        dt.Columns.Add("Value3").DataType = typeof(bool);

        var item = dt.NewRow();
        item["CssStyle"] = "wr_cell_h1";
        item["Key"] = "Content";
        item["Value1"] = 5.97;
        item["Value2"] = DateTime.Now;
        item["Value3"] = true;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["CssStyle"] = "";
        item["Key"] = "Website";
        item["Value1"] = 65.97;
        item["Value2"] = DateTime.Now.AddDays(-25);
        item["Value3"] = false;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["CssStyle"] = "wr_cell_h1";
        item["Key"] = "Marketing";
        item["Value1"] = 45.97;
        item["Value2"] = DateTime.Now;
        item["Value3"] = true;
        dt.Rows.Add(item);

        item = dt.NewRow();
        item["CssStyle"] = "blubb"; // Not existing
        item["Key"] = "Sales";
        item["Value1"] = 45.97;
        item["Value2"] = DateTime.Now;
        item["Value3"] = true;
        dt.Rows.Add(item);

        return dt;
    }
}