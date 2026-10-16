// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;

namespace BodoFtpTransfer;

[SupportedOSPlatform("windows10.0.17763")]
internal class Program
{
    private static int Main(string[] args)
    {
        //try
        //{
            var starter = new BodoFtpTransferConsoleStarter(args);
            starter.LoadWebCredentialsConfig();
            return starter.Start();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//#if DEBUG
//            Console.ReadLine();
//#endif
//        }
    }

    ///// <summary>
    ///// Status-Nachricht setzen
    ///// </summary>
    ///// <param name="msg">Anzuzeigende Nachricht</param>
    //private static void SetMessage(string msg)
    //{
    //    Console.WriteLine(msg);
    //}
}