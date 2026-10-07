// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections;
using System.Globalization;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Tools for console apps
/// </summary>
public static class ConsoleHelper
{
    /// <summary>
    /// Read a secret from console
    /// </summary>
    /// <returns>password string</returns>
    public static string ReadSecret(string message)
    {
        Console.WriteLine(message);
        return ReadSecret();
    }

    /// <summary>
    /// Read a secret from console in a secure manner not showing the secret
    /// </summary>
    /// <returns>String with the secret read hidden from console</returns>
    public static string ReadSecret()
    {
        var passbits = new Queue();

        for (var cki = Console.ReadKey(true); cki.Key != ConsoleKey.Enter; cki = Console.ReadKey(true))
        {
            if (cki.Key == ConsoleKey.Backspace)
            {
                if (passbits.Count <= 0)
                {
                    continue;
                }

                Console.SetCursorPosition((Console.CursorLeft - 1), Console.CursorTop);
                Console.Write(" ");
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                passbits.Dequeue();
            }
            else
            {
                Console.Write("*");
                passbits.Enqueue(cki.KeyChar.ToString(CultureInfo.InvariantCulture));
            }
        }
        Console.WriteLine();

        var pass = passbits.Cast<object>()
            .Select(x => x.ToString())
            .ToArray();

        return string.Join(string.Empty, pass);
    }
}