// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;

namespace BodoWebReporting;

public class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        var starter = new BodoWebReportingConsoleStarter(args);
        return starter.Start();
    }
}