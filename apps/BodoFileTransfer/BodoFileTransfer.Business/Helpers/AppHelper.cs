// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataProtection;

namespace BodoFileTransfer.Business.Helpers;

/// <summary>
/// Helper class for basic app features
/// </summary>
public static class AppHelper
{
    /// <summary>
    /// Load password data
    /// </summary>
    public static void LoadPasswordData()
    {
        PasswordHandler.Key1 = "q4IkuZAIKXRzoEyG9u18MaR4MnfHLxLRx0AYAQsRurI=";
        PasswordHandler.Key2 = "PJkMvluqZ/w/dpckGKjWWpoBXL2FDApRgDgHROX5dl4=";
        PasswordHandler.Key3 = "PtkhErs+/b0HVtQcjuIh62x0h3Mc6TIpB21LptW0r3E=";
        PasswordHandler.Salt = "U645P8yGhALfeDoLnoqT1Q==";
    }
}