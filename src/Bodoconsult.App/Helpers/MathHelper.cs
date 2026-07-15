// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Numerics;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Helper class for mathematic issues
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Returns true if the given number is evenly divisible by a power of 2
    /// </summary>
    public static bool IsPowerOfTwo(int x)
    {
        return (x & (x - 1)) == 0 && x > 0;
    }

    /// <summary>
    /// Get the biggest power of 2 smaller or equal than a number
    /// </summary>
    /// <param name="number">Number to calculate the biggest power of 2 smaller or equal than the number</param>
    /// <returns>Biggest power of 2 smaller or equal than the number</returns>
    public static uint LastPowerOf2SmallerThanNumber(uint number)
    {
        var nn = (uint)(0x8000_0000ul >> BitOperations.LeadingZeroCount(number));
        return nn;
    }

    /// <summary>
    /// Check a list of complex numbers to fit requirements of Fast Fourier Transform (FFT) 
    /// </summary>
    /// <param name="spectrum">Current spectrum data</param>
    public static Complex[] CheckListLengthForFft(List<Complex> spectrum)
    {
        var number = (int)LastPowerOf2SmallerThanNumber((uint)spectrum.Count);
        return spectrum.TakeLast(number).ToArray();

        //try
        //{
        //    for (var i = number; i < count; i++)
        //    {
        //        spectrum.Remove(spectrum[0]);
        //    }
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e);
        //    throw;
        //}

    }

}