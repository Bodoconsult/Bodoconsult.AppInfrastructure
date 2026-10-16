// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// DHCP server hardware type
/// </summary>
public enum DhcpServerHardwareType : byte
{
    /// <summary>
    /// Ethernet
    /// </summary>
    Ethernet = 1,
    /// <summary>
    /// ExperimentalEthernet
    /// </summary>
    ExperimentalEthernet = 2,
    /// <summary>
    /// AmateurRadioAx25
    /// </summary>
    AmateurRadioAx25 = 3,
    /// <summary>
    /// ProteonProNetTokenRing
    /// </summary>
    ProteonProNetTokenRing = 4,
    /// <summary>
    /// Chaos
    /// </summary>
    Chaos = 5,
    /// <summary>
    /// Ieee802Networks
    /// </summary>
    Ieee802Networks = 6,
    /// <summary>
    /// Arcnet
    /// </summary>
    Arcnet = 7,
    /// <summary>
    /// Hyperchannel
    /// </summary>
    Hyperchannel = 8,
    /// <summary>
    /// Lanstar
    /// </summary>
    Lanstar = 9,
    /// <summary>
    /// AutonetShortAddress
    /// </summary>
    AutonetShortAddress = 10,
    /// <summary>
    /// LocalTalk
    /// </summary>
    LocalTalk = 11,
    /// <summary>
    /// LocalNet
    /// </summary>
    LocalNet = 12,
    /// <summary>
    /// UltraLink
    /// </summary>
    UltraLink = 13,
    /// <summary>
    /// SMDS
    /// </summary>
    Smds = 14,
    /// <summary>
    /// FrameRelay
    /// </summary>
    FrameRelay = 15,
    /// <summary>
    /// AsynchronousTransmissionMode16
    /// </summary>
    AsynchronousTransmissionMode16 = 16,
    /// <summary>
    /// HDLC 
    /// </summary>
    Hdlc = 17,
    /// <summary>
    /// FibreChannel
    /// </summary>
    FibreChannel = 18,
    /// <summary>
    /// AsynchronousTransmissionMode19
    /// </summary>
    AsynchronousTransmissionMode19 = 19,
    /// <summary>
    /// SerialLine
    /// </summary>
    SerialLine = 20,
    /// <summary>
    /// AsynchronousTransmissionMode21
    /// </summary>
    AsynchronousTransmissionMode21 = 21
}