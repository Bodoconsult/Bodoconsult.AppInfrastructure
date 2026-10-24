// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;

namespace EmergencyShutdown;

[SupportedOSPlatform("windows10.0.17763")]
internal class Program
{
    private static int Main(string[] args)
    {
        var starter = new EmergencyShutdownConsoleStarter(args);
        return starter.Start();
    }
}