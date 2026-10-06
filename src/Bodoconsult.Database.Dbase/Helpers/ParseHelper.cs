// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Globalization;

namespace Bodoconsult.Database.Dbase.Helpers
{
    /// <summary>
    /// Helper class for parsing DBF file field values to correct .NET types
    /// </summary>
    public static class ParseHelper
    {

        private const string Format = "yyyyMMdd";

        #region Public methods

        /// <summary>
        /// Parse to a nullable bool
        /// </summary>
        /// <param name="columnValue">Column value</param>
        /// <param name="returnNull">Should null be returned or false? Default: false</param>
        /// <returns>Parsed value</returns>
        public static bool? ParseBool(string columnValue, bool returnNull = false)
        {
            if (string.IsNullOrWhiteSpace(columnValue))
            {
                return returnNull ? (bool?)null : false;
            }

            if (bool.TryParse(columnValue.ToLower(), out var result))
            {
                return result;
            }

            return returnNull ? (bool?)null : false;
        }

        /// <summary>
        /// Parse to a nullable double
        /// </summary>
        /// <param name="columnValue">Column value</param>
        /// <param name="returnNull">Should null be returned or false? Default: false</param>
        /// <returns>Parsed value</returns>
        public static double? ParseDouble(string columnValue, bool returnNull = false)
        {
            if (string.IsNullOrWhiteSpace(columnValue))
            {
                return returnNull ? (double?)null : 0;
            }

            if (double.TryParse(columnValue.ToLower(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return returnNull ? (double?)null : 0;
        }

        /// <summary>
        /// Parse to a nullable float
        /// </summary>
        /// <param name="columnValue">Column value</param>
        /// <param name="returnNull">Should null be returned or false? Default: false</param>
        /// <returns>Parsed value</returns>
        public static float? ParseFloat(string columnValue, bool returnNull = false)
        {
            if (string.IsNullOrWhiteSpace(columnValue))
            {
                return returnNull ? (float?)null : 0;
            }

            return float.TryParse(columnValue.ToLower(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : returnNull ? (float?)null : 0;
        }

        /// <summary>
        /// Parse to a nullable int
        /// </summary>
        /// <param name="columnValue">Column value</param>
        /// <param name="returnNull">Should null be returned or false? Default: false</param>
        /// <returns>Parsed value</returns>
        public static int? ParseInt(string columnValue, bool returnNull = false)
        {
            if (string.IsNullOrWhiteSpace(columnValue))
            {
                return returnNull ? (int?)null : 0;
            }

            return int.TryParse(columnValue.ToLower(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : returnNull ? (int?)null : 0;
        }

        /// <summary>
        /// Parse to a nullable datetime
        /// </summary>
        /// <param name="datestring">Date string</param>
        /// <returns>Parsed value</returns>
        public static DateTime? ParseNullableDate(string datestring)
        {
            if (string.IsNullOrEmpty(datestring))
            {
                return null;
            }

            var success = DateTime.TryParseExact(datestring.Trim(' '), Format, CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal, out var result);

            return success ? (DateTime?)result : null;
        }

        /// <summary>
        /// Parse to a nullable DateTime
        /// </summary>
        /// <param name="timestring">Time string</param>
        /// <returns>Parsed value</returns>
        private static DateTime? ParseNullableTime(string timestring)
        {
            //const string format = "HH:mm:ss";
            //var success = DateTime.TryParseExact(timestring, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var result);

            if (string.IsNullOrEmpty(timestring))
            {
                return null;
            }

            var success = DateTime.TryParse(timestring.Trim(' '), out var result);

            return success ?
                (DateTime?)result :
                null;
        }

        /// <summary>
        /// Parse to a nullable DateTime
        /// </summary>
        /// <param name="dateString">Date string</param>
        /// <param name="timeString">Time string</param>
        /// <returns>Parsed value</returns>
        public static DateTime? ParseNullableDateTime(string dateString, string timeString)
        {

            if (string.IsNullOrEmpty(dateString) ||
                string.IsNullOrEmpty(timeString))
            {
                return null;
            }

            var date = ParseNullableDate(dateString);
            var time = ParseNullableTime(timeString);

            return date?.Date + time?.TimeOfDay;
        }

        /// <summary>
        /// Parse to a string with removed blanks at the end
        /// </summary>
        /// <param name="columnValue">Column value</param>
        /// <returns>Parsed string</returns>
        public static string ProcessString(string columnValue)
        {

            // return string.IsNullOrWhiteSpace(columnValue) ? string.Empty : columnValue.TrimEnd(' ');
            return string.IsNullOrWhiteSpace(columnValue) ?
                null :
                columnValue.TrimEnd(' ');
        }

        #endregion Protected Methods

    }
}
