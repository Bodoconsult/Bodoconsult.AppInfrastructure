// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Globalization;

namespace BodoWebReporting.Dal.Helpers;

public static class TextHelper
{
    public static string? Parser(string? input)
    {
        if (input == null)
        {
            return null;
        }

        if (input.Contains("@Quarter"))
        {
            var q = (DateTime.Now.Month - 1)/3 + 1;
            input = input.Replace("@Quarter", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)+q.ToString(CultureInfo.InvariantCulture));

        }

        if (input.Contains("@LQuarter"))
        {
            var d = DateTime.Now.AddMonths(-3);

            var q = (d.Month - 1) / 3 + 1;
            input = input.Replace("@LQuarter", d.Year.ToString(CultureInfo.InvariantCulture) + q.ToString(CultureInfo.InvariantCulture));

        }

        var output = input.Replace("@Year", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture))
            .Replace("@LYear", (DateTime.Now.Year - 1).ToString(CultureInfo.InvariantCulture));
        return output;
    }
}