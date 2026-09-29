// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.App.Abstractions.DataProtection;

namespace Bodoconsult.App.Windows.Test.Model;

internal class EntityWithUidWithSecrets
{
    [DataProtectionKey]
    public Guid Uid { get; set; }

    [DataProtectionSecret]
    public string Secret { get; set; }

    [DataProtectionSecret]
    public string Secret2 { get; set; }
}