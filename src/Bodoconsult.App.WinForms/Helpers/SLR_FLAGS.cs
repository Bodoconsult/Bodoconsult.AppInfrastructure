// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.WinForms.Helpers;

[Flags]
internal enum SLR_FLAGS
{
    SLR_NO_UI = 1,
    SLR_ANY_MATCH = 2,
    SLR_UPDATE = 4,
    SLR_NOUPDATE = 8,
    SLR_NOSEARCH = 16, // 0x00000010
    SLR_NOTRACK = 32, // 0x00000020
    SLR_NOLINKINFO = 64, // 0x00000040
    SLR_INVOKE_MSI = 128, // 0x00000080
}