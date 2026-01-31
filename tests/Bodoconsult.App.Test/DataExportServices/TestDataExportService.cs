// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using System.Text;
using Bodoconsult.App.DataExportServices;

namespace Bodoconsult.App.Test.DataExportServices;

/// <summary>
/// Data export service for TestData instances
/// </summary>
public class TestDataExportService : DataExportServiceBase<TestData>
{
    private readonly CultureInfo _cultureInfo = new("en-us");

    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Thrown if type T is NOT string, ReadOnlyMemory&lt;byte&gt; or byte[]</exception>
    public override ReadOnlyMemory<byte> ToMemory(TestData data)
    {
        var sb = new StringBuilder();

        sb.Append($"{data.Text};");
        sb.Append($"{data.Date:O};");
        sb.Append($"{data.IsValid.ToString(_cultureInfo)};");
        sb.Append($"{data.Number.ToString("N", _cultureInfo)}{Environment.NewLine}");

        var b = Encoding.UTF8.GetBytes(sb.ToString());
        return b.AsMemory();
    }
}