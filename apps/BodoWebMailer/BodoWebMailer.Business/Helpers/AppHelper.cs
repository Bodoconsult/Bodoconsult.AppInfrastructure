// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.DataProtection;

namespace BodoWebMailer.Business.Helpers;

/// <summary>
/// Helper class for mail handling
/// </summary>
public static class AppHelper
{
    /// <summary>
    /// Load password data
    /// </summary>
    public static void LoadPasswordData()
    {
        PasswordHandler.Key1 = "ILQmkEoIe53umB+pPYZsTvvk6wHrInCnlgJhDu9ktm0=";
        PasswordHandler.Key2 = "MUJ8qRQ+NiNZBfnYdY7XX8BoBKF2Yfavvmm1wvTR5FI=";
        PasswordHandler.Key3 = "8Bs11ZoCiPRjtJ1z47IDCq1wubBIZSiAWeIvDZWE+Pk=";
        PasswordHandler.Salt = "Fj/jlbuTXBJNSXGi4f8Cyg==";
    }
}