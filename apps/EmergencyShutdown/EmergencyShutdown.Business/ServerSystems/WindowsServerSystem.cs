// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.ShellTools;
using Bodoconsult.Network.Windows.Helpers;
using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;

namespace EmergencyShutdown.Business.ServerSystems;

/// <summary>
/// Create a dummy server for tdd testing
/// </summary>
public class WindowsServerSystem : BaseServerSystem
{
    private readonly WindowsServer _server;
    private readonly IShellProcessManager _shellProcessManager;
    private const string Cmd = @"shutdown /s /m \\\\{0} /t {1} /c ""{2}"" /f /d {3}";

    /// <summary>
    /// default ctor
    /// </summary>
    /// <param name="server">Windows server instance</param>
    /// <param name="shutdownConfig">Current shutdown config</param>
    /// <param name="shellProcessManager">Current shell process manager</param>
    public WindowsServerSystem(WindowsServer server, ShutdownConfig shutdownConfig, IShellProcessManager shellProcessManager) : base(server, shutdownConfig)
    {
        _server = server;
        _shellProcessManager = shellProcessManager;
    }

    /// <summary>
    /// Shutdown server
    /// </summary>
    /// <returns>True on success else false</returns>
    public override bool Shutdown()
    {
        string reason;
        var comment = $"Shutdown of {Server.ServerNameOrIp}: ";

        switch (ShutdownConfig.ShutdownReason)
        {
            case ShutdownReason.UnplannedOthers:
                reason = "u:0:0";
                comment += "Unplanned others";
                break;
            case ShutdownReason.PlannendOthers:
                reason = "p:0:0";
                comment += "Planned others";
                break;
            case ShutdownReason.UnplannedPowerFailureEnvironment:
                reason = "u:6:12";
                comment += "Unplanned due to power failure environment";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //var cmd = Cmd.Replace("{0}", Server.ServerNameOrIp, StringComparison.OrdinalIgnoreCase)
        //    .Replace("{1}", Server.Timeout.ToString(), StringComparison.OrdinalIgnoreCase)
        //    .Replace("{2}", comment, StringComparison.OrdinalIgnoreCase)
        //    .Replace("{3}", reason, StringComparison.OrdinalIgnoreCase);


        //Debug.Print(cmd);

        //var shellCmd = new ShellCommandShellProcessParameter
        //{
        //    Command = cmd,
        //    AdCredentials = new AdCredentials
        //    {
        //        Name = "ShellCommand",
        //        Username = _server.UserName,
        //        Password = _server.Password,
        //        Domain = _server.Domain
        //    }
        //};

        //try
        //{
        //    _shellProcessManager.RunShellCommand(shellCmd);
        //}
        //catch (Exception e)
        //{
        //    Debug.Print(e.ToString());
        //    return false;
        //}


        try
        {
            return ServerHelper.RemoteShutdown(_server.UserName, _server.Password, _server.Domain, _server.ServerNameOrIp);
        }
        catch (Exception e)
        {
            Debug.Print(e.ToString());
            return false;
        }
    }

}