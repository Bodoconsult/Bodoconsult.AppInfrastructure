// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Bodoconsult.Test.TestDocumentation;

/// <summary>
/// Represents the docu for test class
/// </summary>
public class DocuTestClass : IHtmlCreator
{
    /// <summary>
    /// Current documentation handler. Public only for testing
    /// </summary>
    public DocumentationHandler DocuHandler { get; }


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="docuHandler">Current documentation handler</param>
    /// <param name="type">Curent type</param>
    public DocuTestClass(DocumentationHandler docuHandler, Type type)
    {
        DocuHandler = docuHandler;
        Type = type;
    }

    /// <summary>
    /// Current type of the test class
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Class name
    /// </summary>
    public string Name => Type.Name;

    /// <summary>
    /// Test methods contained in the test class
    /// </summary>
    public IList<DocuTestClassMethod> TestMethods { get; set; } = new List<DocuTestClassMethod>();


    /// <summary>
    /// The priority of a test class as importance for the whole.
    /// </summary>
    public TestPriority TestPriority { get; set; }


    /// <summary>
    /// Test class description: should address documentation receivers, not software developer
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Returns a HTML string for the class
    /// </summary>
    /// <returns>HMTL string representing the class</returns>
    public StringBuilder ToHtmlString()
    {
        var result = new StringBuilder();

        result.Append($"<h2 id=\"{Name.Replace(".", "_")}\">Test class {Name}</h2>\r\n");

        if (!string.IsNullOrEmpty(Description)) result.AppendLine($"<p class=\"top bottom\">{Description}</p>");

        if (TestPriority != TestPriority.Normal)
        {
            result.AppendLine($"<p>Test priority: {TestPriority}</p>");
        }

        var numberOfTests = TestMethods.Count;
        var numberOfTestsSuccess = TestMethods.Count(x => x.Success);
        var numberOfTestsFailedHigh = TestMethods.Count(x => x.Success == false && x.TestPriority == TestPriority.High);

        result.AppendLine($"<dl>\r\n<dt>Total number of tests:</dt><dd>{numberOfTests}</dd>");
        result.AppendLine($"<dt>Successful tests:</dt><dd>{numberOfTestsSuccess}</dd>");
        result.AppendLine($"<dt>Failed tests:</dt><dd>{numberOfTests - numberOfTestsSuccess}</dd>");
        result.AppendLine($"<dt>Failed tests with high priority:</dt><dd>{numberOfTestsFailedHigh}</dd>");
        result.AppendLine("</dl>");



        // Failed tests
        result.Append("<h3>Failed tests</h3>\r\n");

        var data = TestMethods.Where(x => x.TestPriority != TestPriority.Ignore &&
                                          x.TestPriority != TestPriority.IgnoreIfError &&
                                          x.Success == false).OrderBy(x => x.Name).ToArray();

        if (data.Any())
        {
            foreach (var x in data)
            {
                result.Append(x.ToHtmlString());
            }
        }
        else
        {
            result.AppendLine("<p>- none -</p>");
        }


        // Successful tests
        result.Append("<h3>Successful tests</h3>\r\n");
        data = TestMethods.Where(x => x.TestPriority != TestPriority.Ignore &&
                                      x.Success).OrderBy(x => x.Name).ToArray();

        if (data.Any())
        {
            foreach (var x in data)
            {
                result.Append(x.ToHtmlString());
            }
        }
        else
        {
            result.AppendLine("<p>- none -</p>");
        }

        return result;
    }

    /// <summary>
    /// Mark a method as started
    /// </summary>
    /// <param name="description">Description</param>
    /// <param name="priority">Test priority</param>
    /// <returns></returns>
    public DocuTestClassMethod StartMethod(string description = null, TestPriority priority = TestPriority.Normal)
    {
        var st = new StackTrace();

        var sf = st.GetFrame(1);

        var m = sf.GetMethod();

        var method = m.Name;

        var testMethod = TestMethods.FirstOrDefault(x => x.Name == method);

        if (testMethod is null)
        {
            testMethod = new DocuTestClassMethod
            {
                Name = method,
                Parent = this
            };
            TestMethods.Add(testMethod);
        }

        if (!string.IsNullOrEmpty(description))
        {
            testMethod.Description = description;
        }
        testMethod.TestPriority = priority;


        if (TestPriority != TestPriority.Ignore
            && testMethod.TestPriority != TestPriority.Ignore
            && testMethod.TestPriority != TestPriority.IgnoreIfError)
        {
            DocuHandler.NumberOfTests++;
        }

        return testMethod;
    }
}