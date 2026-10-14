// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Model;
using JobExecuter.Dal.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.HelperTests;

[TestFixture]
public class SqlDocuDatabaseHelperTests
{
   
    [Test]
    public void TestDocu()
    {
        var result = new StructuredText();
        IList<Exception> errors = new List<Exception>();
        var dbName="MediaDb";
        var docu = new SqlDocuDatabaseHelper(result, dbName, ".\\sqlexpress", errors, SetStatus);
        docu.AddGeneralDatabaseInfos();
        docu.AddSecurityRoles();
        docu.AddSecurityLogins();
        docu.AddSecurityLoginsRoles();

        var t = new PlainTextFormatter
        {
            StructuredText = result,
            Title = "DB documentation MediaDb"
        };


        var s = t.GetFormattedText();
        Debug.Print(s);

        Assert.That(!string.IsNullOrEmpty(s));
            
        Assert.That(errors.Count==0);
    }

    private void SetStatus(string message)
    {
        Debug.Print(message);
    }
}