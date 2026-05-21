// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Extensions;

/// <summary>
/// Extension methods for Memory&lt;byte&gt; instances
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// Is the instance content the same as the one of the checkInstance
    /// </summary>
    /// <param name="instance">Current instance</param>
    /// <param name="checkInstance">Instance to compare with</param>
    /// <returns>True if both instances have the same byte content else false</returns>
    public static bool IsEqualTo(this byte[] instance,  byte[] checkInstance)
    {
        if (instance.Length != checkInstance.Length)
        {
            return false;
        }

        var mem1 = instance.AsMemory();
        var mem2 = checkInstance.AsMemory();

        for (var i = 0; i < mem1.Length; i++)
        {
            if (mem1.Slice(i, 1).Span[0] != mem2.Slice(i, 1).Span[0])
            {
                return false;
            }
        }

        return true;
    }

}