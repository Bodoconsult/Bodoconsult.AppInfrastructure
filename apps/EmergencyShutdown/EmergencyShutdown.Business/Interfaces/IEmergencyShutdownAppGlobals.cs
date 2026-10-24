// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace EmergencyShutdown.Business.Interfaces;

/// <summary>
/// App globals for emergency shutdown
/// </summary>
public interface IEmergencyShutdownAppGlobals : IAppGlobals
{
    /// <summary>
    /// Current data protection service
    /// </summary>
    IDataProtectionService? DataProtectionService { get; set; }

    /// <summary>
    /// Current workload or null
    /// </summary>
    Workload? Workload { get; set; }

    /// <summary>
    /// Current shutdown config
    /// </summary>
    ShutdownConfig ShutdownConfig { get; set; }
}
