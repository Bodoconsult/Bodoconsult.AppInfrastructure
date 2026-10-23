// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace EmergencyShutdown.Business.Interfaces;

/// <summary>
/// Current configuration for the shutdown
/// </summary>
public class ShutdownConfig
{
    /// <summary>
    /// The reason for the shutdown
    /// </summary>
    public ShutdownReason ShutdownReason { get; set; } = ShutdownReason.UnplannedPowerFailureEnvironment;
}