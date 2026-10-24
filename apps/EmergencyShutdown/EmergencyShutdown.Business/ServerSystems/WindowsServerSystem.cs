// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;

namespace EmergencyShutdown.Business.ServerSystems
{
    /// <summary>
    /// Create a dummy server for tdd testing
    /// </summary>
    public class WindowsServerSystem : BaseServerSystem
    {
        private WindowsServer _server;

        private const string Cmd = @"shutdown /s /m \\{0} /t {1} /c ""{2}"" /f /d {3}";

        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="server">Windows server instance</param>
        /// <param name="shutdownConfig">Current shutdown config</param>
        public WindowsServerSystem(WindowsServer server, ShutdownConfig shutdownConfig) : base(server, shutdownConfig)
        {
            _server = server;
        }

        /// <summary>
        /// Shutdown server
        /// </summary>
        /// <returns>True on success else false</returns>
        public override bool Shutdown()
        {
            var reason = string.Empty;
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

            var cmd = Cmd.Replace("{0}", Server.ServerNameOrIp, StringComparison.OrdinalIgnoreCase)
                .Replace("{1}", Server.Timeout.ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("{2}", comment, StringComparison.OrdinalIgnoreCase)
                .Replace("{3}", reason, StringComparison.OrdinalIgnoreCase);

            throw new NotImplementedException();
        }
    }
}
