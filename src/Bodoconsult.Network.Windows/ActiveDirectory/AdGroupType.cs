// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.ActiveDirectory;

/// <summary>
/// Typ of AD group
/// </summary>
public enum AdGroupType
{
    /// <summary>
    ///  GlobalDistributionGroup 
    /// </summary>
    GlobalDistributionGroup = 2,
    /// <summary>
    ///  DomainLocalDistributionGroup 
    /// </summary>
    DomainLocalDistributionGroup = 4,
    /// <summary>
    ///  UniversalDistributionGroup
    /// </summary>
    UniversalDistributionGroup = 8,
    /// <summary>
    ///  GlobalSecurityGroup 
    /// </summary>
    GlobalSecurityGroup = -2147483646,
    /// <summary>
    ///  DomainLocalSecurityGroup
    /// </summary>
    DomainLocalSecurityGroup = -2147483644,
    /// <summary>
    ///  UniversalSecurityGroup
    /// </summary>
    UniversalSecurityGroup = -2147483640,
    /// <summary>
    ///  BuiltInSecurityGroup 
    /// </summary>
    BuiltInSecurityGroup = -2147483643
}