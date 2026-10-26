// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Management;

namespace Bodoconsult.Network.Windows.Helpers;

/// <summary>
/// Windows server helper class providing features for server management
/// </summary>
public static class ServerHelper
{
    // Source - https://stackoverflow.com/a/67195162
    // Posted by Hossein Bazrafkan, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-10-26, License - CC BY-SA 4.0

    /// <summary>
    /// Shutdown a remote Windows computer
    /// </summary>
    /// <param name="userName">Username</param>
    /// <param name="password">Password</param>
    /// <param name="domain">Domainname or null</param>
    /// <param name="ip">IP or name of the remote computer</param>
    /// <returns>True on success else false</returns>
    public static bool RemoteShutdown(string userName, string password, string domain, string ip)
    {
        try
        {
            var op = new ConnectionOptions
            {
                Username = userName,
                Password = password
            };

            if (!string.IsNullOrEmpty(domain))
            {
                op.Authority = $"ntlmdomain:{domain}";
            }

            // Make a connection to a remote computer.  
            var scope = new ManagementScope($"\\\\{ip}\\root\\cimv2", op);
            scope.Connect();
            //Query system for Operating System information  
            var oq = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            var query = new ManagementObjectSearcher(scope, oq);
            var queryCollection = query.Get();
            foreach (ManagementObject obj in queryCollection)
            {
                //obj.InvokeMethod("ShutDown", null); //shutdown  
                // Obtain in-parameters for the method
                var inParams =
                    obj.GetMethodParameters("Win32Shutdown");

                // Add the input parameters.
                inParams["Flags"] = 1;

                // Execute the method and obtain the return values.
                var outParams = obj.InvokeMethod("Win32Shutdown", inParams, null);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.Print(e.ToString());
            return false;
        }
    }

    // Source - https://stackoverflow.com/a/67195162
    // Posted by Hossein Bazrafkan, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-10-26, License - CC BY-SA 4.0

    
    /// <summary>
    /// Reboot a remote Windows computer
    /// </summary>
    /// <param name="userName">Username</param>
    /// <param name="password">Password</param>
    /// <param name="domain">Domainname or null</param>
    /// <param name="ip">IP or name of the remote computer</param>
    /// <returns>True on success else false</returns>
    public static bool RemoteReboot(string userName, string password, string domain, string ip)
    {
        try
        {
            var op = new ConnectionOptions
            {
                Username = userName,
                Password = password
            };

            if (!string.IsNullOrEmpty(domain))
            {
                op.Authority = $"ntlmdomain:{domain}";
            }

            // Make a connection to a remote computer.  
            var scope = new ManagementScope($"\\\\{ip}\\root\\cimv2", op);
            scope.Connect();
            //Query system for Operating System information  
            var oq = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            var query = new ManagementObjectSearcher(scope, oq);
            var queryCollection = query.Get();
            foreach (ManagementObject obj in queryCollection)
            {
                //obj.InvokeMethod("Reboot", null); //shutdown  

                // Obtain in-parameters for the method
                var inParams =
                    obj.GetMethodParameters("Win32Shutdown");

                // Add the input parameters.
                inParams["Flags"] = 2;

                // Execute the method and obtain the return values.
                var outParams = obj.InvokeMethod("Win32Shutdown", inParams, null);
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


}