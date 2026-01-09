// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.Extensions;
using Bodoconsult.Text.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Pdf.Test;

[TestFixture]
public class DataTableExtensionsTests
{

    [Test]
    public void ToPdfTable_ValidDataTable_PdfTableCreated()
    {
        // Arrange 
        var dt = DataHelper.GetSmallDataTable();

        // Act  
        var result = dt.ToPdfTable();

        // Assert
        Assert.That(result, Is.Not.Null);

        var count = dt.Columns.Count;

        Assert.That(result.Columns.Count, Is.EqualTo(count));
        Assert.That(result.Rows.Count, Is.EqualTo(dt.Rows.Count));

        foreach (var row in result.Rows)
        {
            Assert.That(row.Cells.Count, Is.EqualTo(count));
        }
    }

    [Test]
    public void ToPdfTable_ValidDataTableWithCssInfo_PdfTableCreated()
    {
        // Arrange 
        var dt = DataHelper.GetSmallDataTableWithCssInfo();

        // Act  
        var result = dt.ToPdfTableWithCssInfo(DataTableExtensions.BodoconsultCssColors);

        // Assert
        Assert.That(result, Is.Not.Null);

        var count = dt.Columns.Count - 1;

        Assert.That(result.Columns.Count, Is.EqualTo(count));
        Assert.That(result.Rows.Count, Is.EqualTo(dt.Rows.Count));

        foreach (var row in result.Rows)
        {
            Assert.That(row.Cells.Count, Is.EqualTo(count));
        }
    }
}