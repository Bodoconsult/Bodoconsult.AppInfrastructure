// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Bodoconsult.Test.Interfaces;

namespace Bodoconsult.Test;

/// <summary>
/// Main class to handle output for unit test results
/// </summary>
public class TestDocuHelperHandler : ITestDocuHelperHandler
{
    /// <summary>
    /// default ctor
    /// </summary>
    public TestDocuHelperHandler()
    {
        Helpers = new List<ITestDocuHelper>();
        DefaultNumberFormatDouble = "N3";
        DefaultNumberFormatInteger = "#,##0";
        IgnoredNamespaces = new List<string> { "Bodoconsult.Test" };
        IgnoredTypes = new List<string>();
    }


    /// <summary>
    /// Name of the company run the tests
    /// </summary>
    public string Company { get; set; }

    /// <summary>
    /// Path to a logo image file (JPEG or PNG)
    /// </summary>
    public string LogoPath { get; set; }

    /// <summary>
    /// Namespaces ignored for getting class and method names via reflection
    /// </summary>
    public IList<string> IgnoredNamespaces { get; set; }

    /// <summary>
    ///Types ignored for getting class and method names via reflection
    /// </summary>
    public IList<string> IgnoredTypes { get; set; }


    internal string DirPath;

    internal int ImageCounter;

    internal IList<ITestDocuHelper> Helpers { get; set; }

    #region Formatting properties

    /// <summary>
    /// Default numberformat for double numbers
    /// </summary>
    public string DefaultNumberFormatDouble { get; set; }

    /// <summary>
    /// Default numberformat for integer numbers
    /// </summary>
    public string DefaultNumberFormatInteger { get; set; }

    #endregion

    /// <summary>
    /// Add a test helper
    /// </summary>
    /// <param name="helper"></param>
    public void Add(ITestDocuHelper helper)
    {
        Helpers.Add(helper);
    }

    /// <summary>
    /// Target path
    /// </summary>
    public string TargetPath
    {
        get => _targetPath;
        set
        {
            _targetPath = value;
            DirPath = new FileInfo(_targetPath).DirectoryName;
        }
    }

    /// <summary>
    /// Initialize helper class
    /// </summary>
    public void HelperInitialize()
    {
        foreach (var helper in Helpers)
        {
            helper.LogoPath = LogoPath;
            helper.Company = Company;
            helper.HelperInitialize();
        }
    }

    /// <summary>
    /// Finalize helper class
    /// </summary>
    public void HelperFinalize()
    {
        foreach (var helper in Helpers)
        {
            helper.HelperFinalize();
        }
    }

    /// <inheritdoc />
    public void PrintIt(string inputString, params object[] array)
    {
        foreach (var helper in Helpers)
        {
            helper.PrintIt(inputString, array);
        }
    }

    /// <inheritdoc />
    public void PrintHeader(string message)
    {
        foreach (var helper in Helpers)
        {
            helper.PrintHeader(message);
        }
    }

    /// <inheritdoc />
    public void PrintHeader(string message, params object[] array)
    {
        var s = string.Format(message, array);

        foreach (var helper in Helpers)
        {
            helper.PrintHeader(s);
        }
    }

    /// <inheritdoc />
    public void PrintDivider()
    {
        foreach (var helper in Helpers)
        {
            helper.PrintDivider();
        }
    }

    /// <inheritdoc />
    public void PrintDivider1()
    {
        foreach (var helper in Helpers)
        {
            helper.PrintDivider1();
        }
    }

    /// <inheritdoc />
    public void PrintShortDivider()
    {
        foreach (var helper in Helpers)
        {
            helper.PrintShortDivider();
        }
    }

    /// <inheritdoc />
    public void PrintRow()
    {
        foreach (var helper in Helpers)
        {
            helper.PrintRow();
        }
    }

    /// <inheritdoc />
    public void PrintMethodHeader(string message)
    {
        var method = GetMethodBase();
        foreach (var helper in Helpers)
        {
            helper.PrintMethodHeader(message, method.DeclaringType.Name, method.Name);
        }
    }

    /// <inheritdoc />
    public void PrintMethodHeader(string message, params object[] array)
    {
        var method = GetMethodBase();
        message = string.Format(message, array);

        foreach (var helper in Helpers)
        {
            helper.PrintMethodHeader(message, method.DeclaringType.Name, method.Name);
        }
    }

    /// <inheritdoc />
    public void PrintMethodHeader2(string message)
    {
        foreach (var helper in Helpers)
        {
            helper.PrintMethodHeader2(message);
        }
    }

    /// <inheritdoc />
    public void PrintMethodHeader2(string message, params object[] array)
    {

        var s = string.Format(message, array);

        foreach (var helper in Helpers)
        {
            helper.PrintMethodHeader2(s);
        }
    }


    /// <inheritdoc />
    public void PrintMethodHeader3(string message)
    {
        foreach (var helper in Helpers)
        {
            helper.PrintMethodHeader3(message);
        }
    }

    /// <inheritdoc />
    public void PrintMethodHeader3(string message, params object[] array)
    {

        var s = string.Format(message, array);

        foreach (var helper in Helpers)
        {
            helper.PrintMethodHeader3(s);
        }
    }


    /// <inheritdoc />
    public void PrintClassHeader(string message)
    {

        var method = GetMethodBase();

        foreach (var helper in Helpers)
        {
            helper.PrintClassHeader(message, method.DeclaringType.Name);
        }
    }

    /// <inheritdoc />
    public void PrintClassHeader(string message, params object[] array)
    {

        var method = GetMethodBase();
        var s = string.Format(message, array);

        foreach (var helper in Helpers)
        {
            helper.PrintClassHeader(s, method.DeclaringType.Name);
        }
    }

    /// <inheritdoc />
    public void PrintObjectData(object data, string message)
    {
        // Header
        foreach (var helper in Helpers)
        {
            helper.PrintMethodHeader2(message);
        }

        PrintObjectDataRaw(data);
    }

    private void PrintObjectDataRaw(object data)
    {
        // Fields
        var type = data.GetType(); // Get type pointer
        var fields = type.GetFields(); // Obtain all fields
        var props = type.GetProperties(); // Obtain all properties

        PrintIt("Type: {0}", type.Name);

        var result = new string[fields.Length + props.Length + 1, 4];

        var count = 0;

        result[count, 0] = "Field/Property name";
        result[count, 1] = "Value";
        result[count, 2] = "Type";
        result[count, 3] = "Derived from";

        count++;

        foreach (var field in fields) // Loop through fields
        {
            var name = field.Name; // Get string name
            var temp = field.GetValue(data); // Get value

            result[count, 0] = name;
            result[count, 1] = temp is null ? "null" : temp.ToString();
            result[count, 2] = "Field";

            if (field.DeclaringType != type)
            {
                result[count, 3] = field.DeclaringType.Name;
            }

            // Debug.Print("{0}: {1}", name, temp);
            count++;
        }

        // Properties
        foreach (var prop in props)
        {
            var name = prop.Name; // Get string name
            var temp = prop.GetValue(data); // Get value

            if (prop.PropertyType != typeof(string) && prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                if (temp is null)
                {
                    result[count, 1] = "null";
                }
                else
                {
                    var listType = prop.PropertyType;

                    var generic = string.Empty;

                    if (listType.GenericTypeArguments.Length > 0)
                    {
                        generic = listType.GenericTypeArguments.Aggregate(generic, (current, typeGeneric) => current + (typeGeneric.Name + ","));

                        generic = generic[..^1];
                    }

                    var dict = temp as ICollection;
                    result[count, 1] =  string.Format("{1}<{2}> Count: {0}", dict.Count, listType.Name, generic);
                }
            }
            else
            {
                result[count, 1] = temp is null ? "null" : temp.ToString();
            }

            result[count, 0] = name;
            result[count, 2] = "Property";

            if (prop.DeclaringType != type)
            {
                result[count, 3] = prop.DeclaringType.Name;
            }

            //Debug.Print("{0}: {1}", name, temp);
            count++;
        }

        PrintIt("");

        foreach (var helper in Helpers)
        {
            helper.PrintTable(result);
        }
    }

    /// <summary>
    /// Print objects in a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="message"></param>
    public void PrintListData<T>(IList<T> data, string message)
    {
        // Header
        foreach (var helper in Helpers)
        {
            helper.PrintMethodHeader2(message);
        }

        for (var i = 0; i < data.Count; i++)
        {

            var itemMessage = $"Item {i}";

            // Header
            foreach (var helper in Helpers)
            {
                helper.PrintMethodHeader3(itemMessage);
            }

            PrintObjectDataRaw(data[i]);

        }

    }


    internal Stopwatch Watch;
    private string _targetPath;

    /// <inheritdoc />
    public void StartWatch()
    {
        Watch = new Stopwatch();
        Watch.Restart();
    }

    /// <inheritdoc />
    public void StopWatch()
    {

        var message = $"Time elapsed: {Watch.Elapsed:hh\\:mm\\:ss}";

        foreach (var helper in Helpers)
        {
            helper.PrintIt(message);
        }
    }

    /// <inheritdoc />
    public string PrintImage()
    {
        var erg = GetNextImageNameIntern();

        foreach (var helper in Helpers)
        {
            helper.PrintImage(erg);
        }

        ImageCounter++;
        return Path.Combine(DirPath, erg);
    }

    /// <summary>
    /// Get the path for the next image
    /// </summary>
    /// <returns></returns>
    internal string GetNextImageNameIntern()
    {
        return $"image{ImageCounter}.png";

    }


    /// <summary>
    /// Get the path for the next image
    /// </summary>
    /// <returns></returns>
    public string GetNextImageName()
    {
        return Path.Combine(DirPath, $"image{ImageCounter}.png");

    }

    /// <summary>
    /// Add output debug window
    /// </summary>
    public void AddDebugWindowOutput()
    {
        Helpers.Add(new TestDocuHelperDebugWindow());
    }

    /// <summary>
    /// Add HTML output
    /// </summary>
    /// <param name="outputFileName">output filename</param>
    public void AddHtmlOutput(string outputFileName)
    {
        TargetPath = outputFileName;
        var output = new TestDocuHelperHtml { TargetPath = outputFileName };
        Helpers.Add(output);
    }

    /// <summary>
    /// Add ASCII output
    /// </summary>
    /// <param name="outputFileName">output filename</param>
    public void AddAsciiOutput(string outputFileName)
    {

        TargetPath = outputFileName;
        var output = new TestDocuHelperAscii { TargetPath = outputFileName };
        Helpers.Add(output);
    }

    /// <inheritdoc />
    public void PrintTable(string[,] data)
    {
        foreach (var helper in Helpers)
        {
            helper.PrintTable(data);
        }
    }


    /// <inheritdoc />
    public void PrintTable(double[,] data)
    {

        var erg = new string[data.GetLength(0) + 1, data.GetLength(1) + 1];

        for (var column = 1; column <= data.GetLength(1); column++)
        {
            erg[0, column] = $"C{column - 1}";
        }

        for (var row = 1; row <= data.GetLength(0); row++)
        {
            erg[row, 0] = $"R{row - 1}";

            for (var column = 1; column <= data.GetLength(1); column++)
            {
                erg[row, column] = data[row - 1, column - 1].ToString(DefaultNumberFormatDouble);
            }
        }

        foreach (var helper in Helpers)
        {
            helper.PrintTable(erg);
        }
    }

    /// <inheritdoc />
    public void PrintTable(DataTable data)
    {
        var erg = new string[data.Rows.Count + 1, data.Columns.Count];

        var formats = new List<string>();

        for (var column = 0; column < data.Columns.Count; column++)
        {
            var colObj = data.Columns[column];

            erg[0, column] = colObj.ColumnName;
            switch (colObj.DataType.Name.ToLower())
            {
                case "datetime":
                    formats.Add("{0:dd.MM.yyyy}");
                    break;
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    formats.Add($"{{0:{DefaultNumberFormatInteger}}}");
                    break;
                case "double":
                case "single":
                case "decimal":
                    formats.Add($"{{0:{DefaultNumberFormatDouble}}}");
                    break;
                default:
                    formats.Add("{0}");
                    break;
            }
        }

        var row = 1;
        foreach (DataRow rowData in data.Rows)
        {

            for (var column = 0; column < data.Columns.Count; column++)
            {
                erg[row, column] = string.Format(formats[column], rowData[column]);
            }

            row++;
        }

        foreach (var helper in Helpers)
        {
            helper.PrintTable(erg);
        }
    }


    /// <summary>
    /// Print a link to the default
    /// </summary>
    /// <param name="url"></param>
    /// <param name="title"></param>
    public void PrintLink(string url, string title)
    {
        if (string.IsNullOrEmpty(title)) title = url;

        foreach (var helper in Helpers)
        {
            helper.PrintLink(url, title);
        }
    }

    /// <inheritdoc />
    public string CurrentClass
    {
        set
        {
            foreach (var helper in Helpers)
            {
                helper.CurrentClass = value;
            }
        }
    }

    /// <inheritdoc />
    public void SetDefaults()
    {
        foreach (var helper in Helpers)
        {
            helper.SetDefaults();
        }
    }

    /// <inheritdoc />
    public void ClearTargetDirectoryImages()
    {
        // delete images in the target folder
        foreach (var file in new DirectoryInfo(DirPath).EnumerateFiles("*.png"))
        {
            try
            {
                file.Delete();
            }
            catch
            {
                // ignored
            }
        }
    }


    /// <inheritdoc />
    public void PrintVector(double[] vector)
    {
        for (var i = 0; i < vector.Length; i++)
        {
            var value = vector[i];
            PrintIt("V[{0}] = {1}", i, value);
        }


    }

    internal MethodBase GetMethodBase()
    {
        // get calling method name

        var stack = new StackTrace();

        var method = stack.GetFrame(1).GetMethod();

        for (var i = 0; i < stack.FrameCount; i++)
        {
            var sf = stack.GetFrame(i);
            method = sf.GetMethod();
            var typeNameSpace = method.ReflectedType.Namespace;
            var typeName = method.ReflectedType.Name;

            if (IgnoredNamespaces.All(x => x != typeNameSpace) && !typeNameSpace.StartsWith("System."))
            {
                if (IgnoredTypes.All(x => x != typeName))
                {
                    Debug.Print("{0} {1}", typeNameSpace, string.Empty);
                    break;
                }


            }
        }

        return method;
    }

}