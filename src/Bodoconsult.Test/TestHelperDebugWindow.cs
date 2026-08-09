// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Diagnostics;
using System.Text;

namespace Bodoconsult.Test;

/// <summary>
/// TestHelperBase class prints to DEBUG window in VSS
/// </summary>
public class TestHelperDebugWindow : TestHelperBase
{

    /// <summary>
    /// Initialize helper class
    /// </summary>
    public override void HelperInitialize()
    {

    }

    /// <summary>
    /// Finalize helper class
    /// </summary>
    public override void HelperFinalize()
    {

    }

    /// <summary>
    /// Print a message in the debug window and the output file
    /// </summary>
    /// <param name="inputString"></param>
    /// <param name="array"></param>
    public override void PrintIt(string inputString, params object[] array)
    {
        //Debug.Print(inputString, array);
        Trace.WriteLine(string.Format(inputString, array));
    }

    /// <summary>
    /// Print a header for the output of unit testing
    /// </summary>
    public override void PrintHeader(string message)
    {

        PrintIt(Divider);
        PrintIt(Divider);
        PrintIt(Divider);
        PrintIt(message);
        PrintIt("Date: {0}", DateTime.Now);
        PrintIt(Divider);
        PrintIt(Divider);
        PrintIt(Divider);

    }

    /// <summary>
    /// Print divider
    /// </summary>
    public override void PrintDivider()
    {
        PrintIt(Divider);

    }

    /// <summary>
    /// Print divider 1
    /// </summary>
    public override void PrintDivider1()
    {
        PrintIt(Divider1);

    }


    /// <summary>
    /// Print divider
    /// </summary>
    public override void PrintShortDivider()
    {
        PrintIt(ShortDivider);

    }

    /// <summary>
    /// Print an empty row
    /// </summary>
    public override void PrintRow()
    {
        PrintIt("");
    }


    /// <summary>
    /// Print a message header level 1
    /// </summary>
    /// <param name="message">message to print</param>
    /// <param name="typeName">name of the type</param>
    /// <param name="methodName">name of the calling method</param>
    public override void PrintMethodHeader(string message, string typeName, string methodName)
    {
        PrintRow();
        PrintRow();
        PrintDivider1();
        PrintDivider();
        PrintIt(message);
        // get calling method name


        PrintIt($"Unit test class: {typeName}");
        PrintIt($"Unit test method: {methodName}");
        PrintDivider();
        PrintRow();

    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="title"></param>
    public override void PrintMethodHeader2(string title)
    {
        PrintRow();
        PrintShortDivider();
        PrintIt(title);
        PrintRow();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    public override void PrintMethodHeader3(string title)
    {
        PrintRow();
        PrintShortDivider();
        PrintIt(title);
        PrintRow();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="className"></param>
    public override void PrintClassHeader(string message, string className)
    {
        PrintRow();
        PrintRow();
        PrintDivider1();
        PrintDivider();
        PrintDivider();
        PrintIt(message);
        // get calling method name


        PrintIt($"Unit test class: {className}");
        PrintDivider();
        PrintRow();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileNameWithoutPath"></param>
    public override void PrintImage(string fileNameWithoutPath)
    {
        PrintIt($"--> see image {fileNameWithoutPath}");
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public override void PrintTable(string[,] data)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        var length = new int[data.GetLength(1)];


        for (var x = 0; x < data.GetLength(0); x++)
        {
            for (var y = 0; y < data.GetLength(1); y++)
            {
                if (!string.IsNullOrEmpty(data[x, y]))
                {
                    if (data[x, y].Length > length[y])length[y] = data[x, y].Length;
                } 
            }
        }


        var erg = new StringBuilder();
        for (var x = 0; x < data.GetLength(0); x++)
        {
            for (var y = 0; y < data.GetLength(1); y++)
            {
                erg.Append((string.IsNullOrEmpty( data[x, y]) ? string.Empty: data[x, y]).PadRight(length[y]+3));
            }

            erg.Append("\r\n");
        }

        PrintIt(erg.ToString());
    }
}