// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

// <c> Based on http://www.nullify.net/Article/304 by Simon Soanes.
//
// (c) 2008 Simon Soanes, All Rights Reserved.  No warranties express or implied.
// DO NOT redistribute this source code publically without a link to the origin and this copyright
// notice fully intact, also please send any modifications back to me at simon@nullifynetwork.com
// Including in your software is fine although attribution would be nice. </c>
// 

namespace Bodoconsult.Network.Windows.Dns;

/// <summary>
/// Different types of DNS zone in MS DNS Server
/// </summary>
/// <remarks>For creation of new zones the list is different</remarks>
public enum NewZoneType
{
    /// <summary>
    /// Primary
    /// </summary>
    Primary,
    /// <summary>
    /// Secondary
    /// </summary>
    Secondary,
    /// <remarks>Server 2003+ only</remarks>
    Stub,
    /// <remarks>Server 2003+ only</remarks>
    Forwarder
}