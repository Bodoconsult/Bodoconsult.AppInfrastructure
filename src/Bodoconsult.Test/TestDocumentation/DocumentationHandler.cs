// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Bodoconsult.Test.Helpers;

namespace Bodoconsult.Test.TestDocumentation;

/// <summary>
/// Create a test documenation
/// </summary>
public class DocumentationHandler : IHtmlCreator
{

    /// <summary>
    /// All test classes
    /// </summary>
    public IList<DocuTestClass> TestClasses { get; set; } = new List<DocuTestClass>();


    /// <summary>
    /// Methods without start
    /// </summary>
    public IList<DocuTestClassMethod> MethodsWithoutStart { get; set; } = new List<DocuTestClassMethod>();


    /// <summary>
    /// Assembly name
    /// </summary>
    public string AssemblyName { get; set; }

    /// <summary>
    /// Assembly description
    /// </summary>
    public string AssemblyDescription { get; set; }


    /// <summary>
    /// The target file name
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Number of test included in the documentation
    /// </summary>
    public int NumberOfTests { get; set; }

    /// <summary>
    /// Number of successful test included in the documentation
    /// </summary>
    public int NumberOfTestsSuccess { get; set; }

    /// <summary>
    /// Marks a certain method as started
    /// </summary>
    /// <param name="type">Current type</param>
    /// <param name="description">Description</param>
    /// <param name="priority">Test priority</param>
    /// <returns>Documentation string</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public string StartMethod(Type type, string description = null, TestPriority priority = TestPriority.Normal)
    {

        var st = new StackTrace();

        //for (int i = 0; i < st.FrameCount; i++)
        //{
        //    // Note that high up the call stack, there is only
        //    // one stack frame.
        //    StackFrame sfa = st.GetFrame(i);
        //    Debug.WriteLine("");
        //    Debug.WriteLine("High up the call stack, Method: {0}",
        //        sfa.GetMethod());

        //    Console.WriteLine("High up the call stack, Line Number: {0}",
        //        sfa.GetFileLineNumber());
        //}



        var sf = st.GetFrame(1);

        if (sf is null)
        {
            return string.Empty;
        }

        var m = sf.GetMethod();

        if (m is null)
        {
            return string.Empty;
        }

        var className = type.Name;


        var testClass = TestClasses.FirstOrDefault(x => x.Name == className);

        if (testClass is null)
        {
            testClass = new DocuTestClass(this, type);

            TestClasses.Add(testClass);
        }

        var method = m.Name;

        var testMethod = testClass.TestMethods.FirstOrDefault(x => x.Name == method);

        if (testMethod is null)
        {
            testMethod = new DocuTestClassMethod
            {
                Name = method,
                Parent = testClass
            };
            testClass.TestMethods.Add(testMethod);
        }

        if (!string.IsNullOrEmpty(description))
        {
            testMethod.Description = description;
        }
        testMethod.TestPriority = priority;


        if (testClass.TestPriority != TestPriority.Ignore
            && testMethod.TestPriority != TestPriority.Ignore
            && testMethod.TestPriority != TestPriority.IgnoreIfError)
        {
            NumberOfTests++;
        }

        return method;

    }

    /// <summary>
    /// Method was successful
    /// </summary>
    /// <param name="type">Type to test</param>
    /// <returns>Documentation</returns>
    public string MethodSuccess(Type type)
    {
        var st = new StackTrace();
        var sf = st.GetFrame(1);

        if (sf is null)
        {
            return string.Empty;
        }

        var m = sf.GetMethod();

        if (m is null)
        {
            return string.Empty;
        }

        var className = type.Name;


        var testClass = TestClasses.FirstOrDefault(x => x.Name == className);

        if (testClass is null)
        {
            testClass = new DocuTestClass(this, type);

            TestClasses.Add(testClass);
        }

        var method = m.Name;

        var testMethod = testClass.TestMethods.FirstOrDefault(x => x.Name == method);

        if (testMethod is null)
        {
            testMethod = new DocuTestClassMethod
            {
                Name = method,
                Parent = testClass
            };
            testClass.TestMethods.Add(testMethod);

            MethodsWithoutStart.Add(testMethod);
        }

        testMethod.Success = true;

        if (testMethod.TestPriority == TestPriority.IgnoreIfError)
        {
            NumberOfTests++;
        }

        if (testClass.TestPriority != TestPriority.Ignore && testMethod.TestPriority != TestPriority.Ignore)
        {
            NumberOfTestsSuccess++;
        }

        return method;
    }

    /// <summary>
    /// Create a report and write it to <see cref="FileName"/>
    /// </summary>
    /// <returns><see cref="FileName"/></returns>
    public string CreateReport()
    {
        if (File.Exists(FileName))
        {
            File.Delete(FileName);
        }

        var master = TestHelper.GetTextResource("HtmlMaster");

        var title = new StringBuilder();

        title.AppendLine($"<h1>Test assembly {AssemblyName}</h1>");

        // general data
        title.AppendLine(string.IsNullOrEmpty(AssemblyDescription) ? string.Empty : $"<p class=\"top bottom\">{AssemblyDescription}</p>");
        title.AppendLine($"<dl>\r\n<dt>Date created:</dt><dd>{DateTime.Now:D}</dd>");
        title.AppendLine($"<dt>Computer:</dt><dd>{Environment.MachineName}</dd>");
        title.AppendLine($"<dt>OS: </dt><dd>{Environment.OSVersion}</dd>");
        title.AppendLine($"<dt>64 bit:</dt><dd>{Environment.Is64BitOperatingSystem}</dd>\r\n</dl>");

        var content = new StringBuilder();

        content.Append(TocToHtmlString());

        content.Append(ToHtmlString());

        foreach (var testClass in TestClasses.Where(x => x.TestPriority != TestPriority.Ignore).OrderBy(x => x.Name))
        {
            content.Append(testClass.ToHtmlString());
        }


        var s = string.Format(master, content, title);

        File.WriteAllText(FileName, s);

        return FileName;
    }

    /// <summary>
    /// Add a class to the documentation
    /// </summary>
    /// <param name="type">Type to test</param>
    /// <param name="description">Description</param>
    /// <param name="priority">Priority for testing</param>
    /// <returns></returns>
    public DocuTestClass AddClass(Type type, string description, TestPriority priority = TestPriority.Normal)
    {

        var className = type.Name;

        var testClass = TestClasses.FirstOrDefault(x => x.Name == className);

        if (testClass is null)
        {
            testClass = new DocuTestClass(this, type);

            TestClasses.Add(testClass);
        }

        testClass.Description = description;
        testClass.TestPriority = priority;

        return testClass;
    }

    /// <summary>
    /// Create an HTML string
    /// </summary>
    /// <returns>StringBuilder with HTML</returns>
    public StringBuilder ToHtmlString()
    {
        var content = new StringBuilder();

        // find all failures
        var methods = new List<DocuTestClassMethod>();

        foreach (var testClass in TestClasses.Where(x => x.TestPriority != TestPriority.Ignore &&
                                                         x.TestMethods.Any(y => y.Success == false)))
        {
            foreach (var testMethod in testClass.TestMethods.Where(x => x.TestPriority != TestPriority.Ignore &&
                                                                        x.TestPriority != TestPriority.IgnoreIfError &&
                                                                        x.Success == false))
            {
                methods.Add(testMethod);
            }

        }

        var numberOfTestsFailedHigh = methods.Count(x => x.TestPriority == TestPriority.High);

        // 
        content.AppendLine("<h2 id=\"Overview\">Overview</h2>");
        content.AppendLine($"<dl>\r\n<dt>Total number of tests:</dt><dd>{NumberOfTests}</dd>");
        content.AppendLine($"<dt>Successful tests:</dt><dd>{NumberOfTestsSuccess}</dd>");
        content.AppendLine($"<dt>Failed tests:</dt><dd>{NumberOfTests - NumberOfTestsSuccess}</dd>");

        content.AppendLine($"<dt>Failed tests with high priority:</dt><dd>{numberOfTestsFailedHigh}</dd>");

        //content.AppendLine($"<dt>OS: </dt><dd>{Environment.OSVersion}</dd>");
        //content.AppendLine($"<dt>64 bit:</dt><dd>{Environment.Is64BitOperatingSystem}</dd>");

        content.AppendLine("</dl>");




        if (MethodsWithoutStart.Any())
        {

            content.AppendLine("<h3>Tests without start</h3>");

            // Print methods without start
            foreach (var m in MethodsWithoutStart)
            {
                content.Append(m.ToHtmlStringLong());
            }
        }



        content.AppendLine("<h3>Failed tests</h3>");

        if (methods.Count == 0)
        {
            content.AppendLine("<p>- none -</p>");
        }
        else
        {
            foreach (var method in methods.OrderByDescending(x => x.Parent.TestPriority)
                         .ThenByDescending(x => x.TestPriority))
            {
                content.Append(method.ToHtmlStringLong());
            }
        }


        return content;
    }

    /// <summary>
    /// Create an HTML string for the TOC
    /// </summary>
    /// <returns></returns>
    public StringBuilder TocToHtmlString()
    {
        var result = new StringBuilder();

        result.AppendLine("<h2>Table of content</h2>");

        result.AppendLine("<p><a href=\"#Overview\">Overview</a></p>");

        foreach (var testClass in TestClasses.Where(x => x.TestPriority != TestPriority.Ignore).OrderBy(x => x.Name))
        {
            result.AppendLine($"<p><a href=\"#{testClass.Name.Replace(".", "_")}\">{testClass.Name}</a></p>");
        }

        return result;
    }
}