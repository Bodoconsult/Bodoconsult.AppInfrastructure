// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoFileTransfer.Business.Helpers;

namespace BodoFileTransfer.Test.App;

public class MyDebugAppBuilder : BaseDebugAppBuilder
{
    public MyDebugAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    {
        AppHelper.LoadPasswordData();
    }
}