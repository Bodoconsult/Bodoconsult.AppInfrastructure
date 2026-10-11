// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using System.IO;
using Bodoconsult.Test.Test.Helpers;
using Bodoconsult.Test.Test.Models;
using NUnit.Framework;

namespace Bodoconsult.Test.Test;

[TestFixture]
#pragma warning disable 1591
public class TestHelperAsciiTests
{
    private readonly string _path = Path.Combine(TestHelper.TempPath, "ProtocolUnitTesting.txt");

    [Test]
    public void DiverseMethods_ValidInput_AsciiOutputCreated()
    {

        var x = new DerivedClass
        {
            Field2 = "Field2Value",
            Property2 = "Property2Value",
            Field1 = "Field1Value",
            Property1 = "PropertyValue1"
        };
        
        var y = new BaseClass
        {
            Field1 = "Field1Value",
            Property1 = "PropertyValue1"
        };

        var list = new List<BaseClass> { y, x };

        var handler = new HandlerClass { Data = list };

        var helper = new TestDocuHelperHandler();
        helper.AddDebugWindowOutput();
        helper.AddAsciiOutput(_path);
        helper.PrintHeader("Test header");


        //helper.PrintObjectData(x, "Derived class object");

        //helper.PrintListData(list, "List of objects");


        helper.PrintObjectData(handler, "Handler class object");

        helper.PrintClassHeader("Hallo", "UnitTestTestHelperAscii");
        helper.PrintMethodHeader("Method header 1");
        helper.PrintMethodHeader2("Method header 2");

        helper.PrintClassHeader("2 Print data table", "UnitTestTestHelperAscii");
        helper.PrintTable(TestData.GetDataTable());

        helper.PrintClassHeader("2 Print double array", "UnitTestTestHelperAscii");
        helper.PrintTable(TestData.GetDoubleArray());

        // Assert
        Assert.That(File.Exists(_path));
    }
}