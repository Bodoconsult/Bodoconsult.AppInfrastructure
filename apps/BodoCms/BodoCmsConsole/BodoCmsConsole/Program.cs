// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;

namespace BodoCmsConsole;

[SupportedOSPlatform("windows10.0.17763")]
internal class Program
{
    //[STAThread]
    private static int Main()
    {
        var starter = new BodoCmsConsoleStarter([]);
        return starter.Start();
    }
}