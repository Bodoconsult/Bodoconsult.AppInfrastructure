// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Microsoft.Win32;
using PdfSharp.Fonts;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// An implementation of <see cref="IFontResolver"/> working on MS Windows operating systems
/// </summary>
[SupportedOSPlatform("windows")]
public class WindowsFontResolver : IFontResolver
{
    /// <summary>
    /// All installed fonts
    /// </summary>
    public readonly Dictionary<string, string> InstalledFonts = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    public WindowsFontResolver()
    {
        var localMachineKey = Registry.LocalMachine;
        var localMachineKeySub = localMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", false);

        if (localMachineKeySub is null)
        {
            localMachineKey.Close();
            return;
        }

        var mynames = localMachineKeySub.GetValueNames();

        foreach (var name in mynames)
        {
            var value = localMachineKeySub.GetValue(name);

            if (value is null)
            {
                continue;
            }

            var myvalue = value.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(myvalue) || myvalue.Length < 5)
            {
                continue;
            }


            if (!string.Equals( myvalue[^4..], ".TTF", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(myvalue.Substring(1, 2),  @":", StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            var val = name[..^11].ToUpperInvariant();
            InstalledFonts[val] = myvalue;
        }

        localMachineKey.Close();
        localMachineKeySub.Close();

    }

    /// <summary>
    /// Converts specified information about a required typeface into a specific font.
    /// </summary>
    /// <param name="familyName">Name of the font family.</param>
    /// <param name="bold">Set to <c>true</c> when a bold font face is required.</param>
    /// <param name="italic">Set to <c>true</c> when an italic font face is required.</param>
    /// <returns>Information about the physical font, or null if the request cannot be satisfied.</returns>
    public FontResolverInfo ResolveTypeface(string familyName, bool bold, bool italic)
    {
        // Ignore case of font names.
        var name = familyName.ToUpperInvariant();

        var fonts = InstalledFonts.Keys.Where(x => x.StartsWith(name));


        if (!fonts.Any())
        {
            return new FontResolverInfo("ARIAL");
        }


        bool success;
        string key;

        if (bold)
        {
            if (italic)
            {
                key = $"{name} BOLD ITALIC";
                success = InstalledFonts.TryGetValue(key, out _);
                if (success)
                {
                    return new FontResolverInfo(key);
                }
            }
            else
            {
                key = $"{name} BOLD";
                success = InstalledFonts.TryGetValue(key, out _);
                if (success)
                {
                    return new FontResolverInfo(key);
                }
            }
        }

        if (italic)
        {
            key = $"{name} ITALIC";
            success = InstalledFonts.TryGetValue(key, out _);
            if (success)
            {
                return new FontResolverInfo(key);
            }
        }

        key = $"{name}";
        success = InstalledFonts.TryGetValue(key, out _);
        return success ? new FontResolverInfo(key) : new FontResolverInfo("ARIAL");
    }

    /// <summary>
    /// Gets the bytes of a physical font with specified face name.
    /// </summary>
    /// <param name="faceName">A face name previously retrieved by ResolveTypeface.</param>
    public byte[] GetFont(string faceName)
    {
        var fName = faceName.ToUpperInvariant();

        if (!InstalledFonts.TryGetValue(fName, out var font))
        {
            throw new ArgumentException($"Font {faceName} not installed");
        }

        var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), font);

        var data = File.ReadAllBytes(fileName);

        return data;
    }
}