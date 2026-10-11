// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Windows.ConsoleTools;

/// <summary>
/// Basic implementation of a console app starter implementing app password and SMTP or O365 mail acccount 
/// </summary>
public class BasicConsoleStarter : BaseConsoleStarter
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appName">App name</param>
    /// <param name="args">Args the app was started with</param>
    public BasicConsoleStarter(string appName, string[] args): base(appName, args)
    { }
}