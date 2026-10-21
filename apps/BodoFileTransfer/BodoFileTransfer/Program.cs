// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;

namespace BodoFileTransfer;

[SupportedOSPlatform("windows10.0.17763")]
internal class Program
{
    private static int Main(string[] args)
    {
        var starter = new BodoFileTransferConsoleStarter(args);
        return starter.Start();
    }
}