// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Database.Ef.Extensions;

/// <summary>
/// Extension methods for <see cref="Version"/> instances
/// </summary>
public static class VersionExtensions
{
    /// <summary>
    /// Check if current version is equal or greater than the other version. Check Major and Minor, not Build
    /// </summary>
    /// <param name="currentVersion">Version to check</param>
    /// <param name="otherVersion">Version to compare with</param>
    /// <returns>True if current version is equal or greater than the other version else false</returns>
    public static bool IsEqualOrGreater(this Version currentVersion, Version otherVersion)
    {
        if (otherVersion == null)
        {
            return false;
        }

        if (currentVersion.Major < otherVersion.Major)
        {
            return false;
        }

        return currentVersion.Minor >= otherVersion.Minor;
    }

    /// <summary>
    /// Check if current version is equal or greater than the other version. Check Major and Minor AND Build
    /// </summary>
    /// <param name="currentVersion">Version to check</param>
    /// <param name="otherVersion">Version to compare with</param>
    /// <returns>True if current version is equal or greater than the other version else false</returns>
    public static bool IsEqualOrGreaterBuild(this Version currentVersion, Version otherVersion)
    {
        if (otherVersion == null)
        {
            return false;
        }

        if (currentVersion.Major < otherVersion.Major)
        {
            return false;
        }

        if (currentVersion.Minor < otherVersion.Minor)
        {
            return false;
        }

        return currentVersion.Build >= otherVersion.Build;
    }

}