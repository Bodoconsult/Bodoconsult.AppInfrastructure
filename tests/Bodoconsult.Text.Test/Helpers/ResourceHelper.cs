// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using System.Reflection;

namespace Bodoconsult.Text.Test.Helpers;

public class ResourceHelper
{

    /// <summary>
    /// Get a text from an embedded resource file
    /// </summary>
    /// <param name="resourceName">resource name = plain file name without extension and path</param>
    /// <returns></returns>
    public static string GetTextResource(string resourceName)
    {

        resourceName = $"Bodoconsult.Text.Test.Resources.{resourceName}";

        var ass = Assembly.GetExecutingAssembly();
        var str = ass.GetManifestResourceStream(resourceName);

        if (str is null) return null;

        string s;

        using (var file = new StreamReader(str))
        {
            s = file.ReadToEnd();
        }

        return s;
    }

}