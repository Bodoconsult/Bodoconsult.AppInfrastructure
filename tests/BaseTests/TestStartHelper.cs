// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BaseTests.Apps;
using Bodoconsult.App.Windows.DataProtection;

namespace BaseTests;

/// <summary>
/// Helper class for test assembly starts
/// </summary>
public static class TestStartHelper
{
    private static TestCredentialManager<TestCredentials> cm;

    /// <summary>
    /// Default ctor
    /// </summary>
    static TestStartHelper()
    {
        cm = new TestCredentialManager<TestCredentials>("Bodoconsult.Finance.Tests");
        cm.LoadCredentials();
    }

    /// <summary>
    /// Load test credentials
    /// </summary>
    public static void LoadCredentials()
    {
        var globals = TestAppGlobals.Instance;
        globals.TestCredentials = cm.Credentials;
    }

    /// <summary>
    /// Current credentials for testing
    /// </summary>
    public static TestCredentials? Credentials => cm.Credentials;
}