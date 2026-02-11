// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Test.DataExportServices;

public class TestData
{
    public string Text { get; set; } = "Some text";

    public DateTime Date { get; set; } = DateTime.Now;

    public bool IsValid { get; set; }

    public double Number { get; set; } = 12345.67;

}