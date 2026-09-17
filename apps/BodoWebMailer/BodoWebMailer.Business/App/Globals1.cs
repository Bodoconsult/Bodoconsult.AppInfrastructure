//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Reflection;
//using BodoWebMailer.Business.Helpers;

//namespace BodoWebMailer.Business.App;

///// <summary>
///// Global values for the application and the database schema
///// </summary>
//public static class Globals
//{



//    #region Public constants





//    #endregion


//    #region Public properties

//    /// <summary>
//    /// Tolerated difference value for numeric values being equal
//    /// </summary>
//    public static decimal ToleranceValueComparisons { get; set; } = new decimal(0.00000000000001);



//    #endregion



//    #region Public methods

//    /// <summary>
//    /// Check if two objects have the same content
//    /// </summary>
//    /// <param name="original">original value in the database</param>
//    /// <param name="current">current value in the entity</param>
//    /// <returns></returns>
//    public static bool CheckIfValuesAreEqual(object original, object current)
//    {

//        if (original == current)
//        {
//            return true;
//        }

//        if (original == null)
//        {
//            return current == null;
//        }

//        if (original.Equals(current))
//        {
//            return true;
//        }

//        if (current == null)
//        {
//            return false;
//        }


//#pragma warning disable CA1062

//        switch (original.GetType().Name.ToUpperInvariant())
//        {
//            case "BYTE[]":
//                return ByteArrayCompare((byte[])original, (byte[])current);
//            case "SINGLE":
//            case "DOUBLE":
//            case "DECIMAL":
//            case "FLOAT":
//                var diff = Math.Abs(Convert.ToDecimal(original, CultureInfo.InvariantCulture) - Convert.ToDecimal(current, CultureInfo.InvariantCulture));
//                return diff < ToleranceValueComparisons;
//            default:

//                break;
//        }

//#pragma warning restore CA1062

//        return false;
//    }

//    /// <summary>
//    /// Compare two byte arrays
//    /// </summary>
//    /// <param name="a1">Byte array 1 to check</param>
//    /// <param name="a2">Byte array 2 to check</param>
//    /// <returns>true if the arrays are equal</returns>
//    public static bool ByteArrayCompare(IReadOnlyList<byte> a1, IReadOnlyList<byte> a2)
//    {
//        if (a1 == null && a2 == null) return true;

//        if (a1 == null) return false;
//        if (a2 == null) return false;

//        if (a1.Count != a2.Count)
//            return false;

//        // Fastest way to compare arrays: iterate it
//        for (var i = 0; i < a1.Count; i++)
//            if (a1[i] != a2[i])
//                return false;

//        return true;
//    }




//    /// <summary>
//    /// Current app settings
//    /// </summary>
//    public static AppSettings CurrentAppSettings { get; private set; }



//    /// <summary>
//    /// Load app settings from app directory
//    /// </summary>
//    public static void LoadAppSettings()
//    {

//        var path = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

//        var fileName = Path.Combine(path, "appSettings.json");

//        CurrentAppSettings = JsonHelper.LoadJsonFile<AppSettings>(fileName);
//    }


//    #endregion


//}