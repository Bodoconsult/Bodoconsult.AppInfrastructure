// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp.Options;

namespace Bodoconsult.Network.Windows.Interfaces;

public interface IDhcpServerOptionElement : IEquatable<IDhcpServerOptionElement>
{
    DhcpServerOptionElementType Type { get; }
    object Value { get; }
    string ValueFormatted { get; }
}