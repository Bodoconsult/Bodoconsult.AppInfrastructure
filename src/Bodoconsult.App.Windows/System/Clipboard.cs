// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

/*
MIT License

Copyright (c) 2018 Simon Cropp

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bodoconsult.App.Windows.System;

#nullable enable
#nullable disable warnings

/// <summary>
/// Helper class for clipboard handling
/// </summary>
[SupportedOSPlatform("windows")]
public static class Clipboard
{
    /// <summary>
    /// Place a text in the clipboard
    /// </summary>
    /// <param name="text">Text to place in the clipboard</param>
    /// <param name="cancellation">Current cancellation token</param>
    public static async Task SetTextAsync(string text, CancellationToken cancellation)
    {
        await TryOpenClipboardAsync(cancellation);

        InnerSet(text);
    }

    /// <summary>
    /// Place a text in the clipboard
    /// </summary>
    /// <param name="text">Text to place in the clipboard</param>
    public static void SetText(string text)
    {
        TryOpenClipboard();

        InnerSet(text);
    }

    private static void InnerSet(string text)
    {
        EmptyClipboard();
        IntPtr hGlobal = 0;
        try
        {
            var bytes = (text.Length + 1) * 2;
            hGlobal = Marshal.AllocHGlobal(bytes);

            if (hGlobal == 0)
            {
                ThrowWin32();
            }

            var target = GlobalLock(hGlobal);

            if (target == 0)
            {
                ThrowWin32();
            }

            try
            {
                Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
            }
            finally
            {
                GlobalUnlock(target);
            }

            if (SetClipboardData(CfUnicodeText, hGlobal) == 0)
            {
                ThrowWin32();
            }

            hGlobal = 0;
        }
        finally
        {
            if (hGlobal != 0)
            {
                Marshal.FreeHGlobal(hGlobal);
            }

            CloseClipboard();
        }
    }

    private static async Task TryOpenClipboardAsync(CancellationToken cancellation)
    {
        var num = 10;
        while (true)
        {
            if (OpenClipboard(0))
            {
                break;
            }

            if (--num == 0)
            {
                ThrowWin32();
            }

            await Task.Delay(100, cancellation);
        }
    }

    private static void TryOpenClipboard()
    {
        var num = 10;
        while (true)
        {
            if (OpenClipboard(0))
            {
                break;
            }

            if (--num == 0)
            {
                ThrowWin32();
            }

            Thread.Sleep(100);
        }
    }

    /// <summary>
    /// Get clipboard content
    /// </summary>
    /// <param name="cancellation">Current cancellation token</param>
    /// <returns>Clipboard content</returns>
    public static async Task<string?> GetTextAsync(CancellationToken cancellation)
    {
        if (!IsClipboardFormatAvailable(CfUnicodeText))
        {
            return null;
        }
        await TryOpenClipboardAsync(cancellation);

        return InnerGet();
    }

    /// <summary>
    /// Get clipboard content
    /// </summary>
    /// <returns>Clipboard content</returns>
    public static string? GetText()
    {
        if (!IsClipboardFormatAvailable(CfUnicodeText))
        {
            return null;
        }
        TryOpenClipboard();

        return InnerGet();
    }

    private static string? InnerGet()
    {
        IntPtr handle = 0;

        IntPtr pointer = 0;
        try
        {
            handle = GetClipboardData(CfUnicodeText);
            if (handle == 0)
            {
                return null;
            }

            pointer = GlobalLock(handle);
            if (pointer == 0)
            {
                return null;
            }

            var size = GlobalSize(handle);
            var buff = new byte[size];

            Marshal.Copy(pointer, buff, 0, size);

            return Encoding.Unicode.GetString(buff).TrimEnd('\0');
        }
        finally
        {
            if (pointer != 0)
            {
                GlobalUnlock(handle);
            }

            CloseClipboard();
        }
    }

    private const uint CfUnicodeText = 13;

    private static void ThrowWin32()
    {
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DllImport("User32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsClipboardFormatAvailable(uint format);

    [DllImport("User32.dll", SetLastError = true)]
    private static extern IntPtr GetClipboardData(uint uFormat);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

    [DllImport("user32.dll")]
    private static extern bool EmptyClipboard();

    [DllImport("Kernel32.dll", SetLastError = true)]
    private static extern int GlobalSize(IntPtr hMem);
}