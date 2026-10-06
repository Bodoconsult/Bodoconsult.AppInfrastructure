// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.IO;
using System.Text;

namespace Bodoconsult.Test;

/// <summary>
/// TestHelperBase class prints to an ASCII file
/// </summary>
public class TestDocuHelperAscii : TestDocuHelperBase
{

    /// <summary>
    /// Default ctor
    /// </summary>
    public override void HelperInitialize()
    {
        // Delete file if existing
        if (File.Exists(TargetPath))
        {
            File.Delete(TargetPath);
        }

    }
    /// <summary>
    /// Finalizer
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
        // Write to file
        using var sw = File.AppendText(TargetPath);
        sw.WriteLine(inputString, array);
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

    /// <summary>
    /// Print a message header level 2
    /// </summary>
    /// <param name="message">message to print</param>
    public override void PrintMethodHeader2(string message)
    {
        PrintRow();
        PrintShortDivider();
        PrintIt(message);
        PrintRow();
    }
    /// <summary>
    /// Print a message header level 3
    /// </summary>
    /// <param name="message">message to print</param>
    public override void PrintMethodHeader3(string message)
    {
        PrintRow();
        PrintShortDivider();
        PrintIt(message);
        PrintRow();
    }


    /// <summary>
    /// Print a class header
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
    /// Print an image
    /// </summary>
    /// <param name="fileNameWithoutPath"></param>
    public override void PrintImage(string fileNameWithoutPath)
    {
        PrintIt($"--> see image {fileNameWithoutPath}");
    }

    /// <summary>
    /// Print a table
    /// </summary>
    /// <param name="data"></param>
    public override void PrintTable(string[,] data)
    {
        var length = new int[data.GetLength(1)];


        for (var x = 0; x < data.GetLength(0); x++)
        {
            for (var y = 0; y < data.GetLength(1); y++)
            {
                if (!string.IsNullOrEmpty(data[x, y]))
                {
                    if (data[x, y].Length > length[y]) length[y] = data[x, y].Length;
                }
            }
        }


        var erg = new StringBuilder();
        for (var x = 0; x < data.GetLength(0); x++)
        {
            for (var y = 0; y < data.GetLength(1); y++)
            {
                erg.Append((string.IsNullOrEmpty(data[x, y]) ? string.Empty : data[x, y]).PadRight(length[y] + 3));
            }

            erg.Append("\r\n");
        }

        PrintIt(erg.ToString());
    }

}