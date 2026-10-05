// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.DataProtection;

namespace Bodoconsult.App.Windows.Test.Model;

internal class EntityWithSecrets
{
    [DataProtectionKey]
    public string Name { get; set; }

    [DataProtectionSecret]
    public string Secret { get; set; }

    [DataProtectionSecret]
    public string Secret2 { get; set; }
}
