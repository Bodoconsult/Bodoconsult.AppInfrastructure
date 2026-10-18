// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Dhcp.Options;

/// <summary>
/// Option ID type attribute
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class DhcpServerOptionIdTypeAttribute : Attribute
{
    /// <summary>
    /// Option ID type
    /// </summary>
    public DhcpServerOptionIdTypes Type { get; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="type">Option ID type</param>
    public DhcpServerOptionIdTypeAttribute(DhcpServerOptionIdTypes type)
    {
        Type = type;
    }
}