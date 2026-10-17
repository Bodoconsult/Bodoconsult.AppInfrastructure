// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Packat message types
/// </summary>
public enum DhcpServerPacketMessageTypes : byte
{
    /// <summary>
    /// Unknown
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// Discover
    /// </summary>
    Discover = 1,
    /// <summary>
    /// Offer
    /// </summary>
    Offer = 2,
    /// <summary>
    /// Request
    /// </summary>
    Request = 3,
    /// <summary>
    /// Decline
    /// </summary>
    Decline = 4,
    /// <summary>
    /// Acknowledgment
    /// </summary>
    Acknowledgment = 5,
    /// <summary>
    /// NegativeAcknowledgment
    /// </summary>
    NegativeAcknowledgment = 6,
    /// <summary>
    /// Release
    /// </summary>
    Release = 7,
    /// <summary>
    /// Inform
    /// </summary>
    Inform = 8,
    /// <summary>
    /// ForceRenew
    /// </summary>
    ForceRenew = 9,
    /// <summary>
    /// LeaseQuery
    /// </summary>
    LeaseQuery = 10,
    /// <summary>
    /// LeaseUnassigned
    /// </summary>
    LeaseUnassigned = 11,
    /// <summary>
    /// LeaseUknown
    /// </summary>
    LeaseUnknown = 12,
    /// <summary>
    /// LeaseActive
    /// </summary>
    LeaseActive = 13,
    /// <summary>
    /// BulkLeaseQuery
    /// </summary>
    BulkLeaseQuery = 14,
    /// <summary>
    /// LeaseQueryDone
    /// </summary>
    LeaseQueryDone = 15,
    /// <summary>
    /// ActiveLeaseQuery
    /// </summary>
    ActiveLeaseQuery = 16,
    /// <summary>
    /// LeaseQueryStatus
    /// </summary>
    LeaseQueryStatus = 17,
    /// <summary>
    /// Tls 
    /// </summary>
    Tls = 18
}