// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace EmergencyShutdown.Business.Interfaces;

/// <summary>
/// Enum for the reasons for the shutdown
/// </summary>
public enum ShutdownReason
{
    /// <summary>
    /// Unplanned, other reasons
    /// </summary>
    UnplannedOthers,

    /// <summary>
    /// Planned, other reasons
    /// </summary>
    PlannendOthers,

    /// <summary>
    /// Unplanned, ower failure environment
    /// </summary>
    UnplannedPowerFailureEnvironment
}