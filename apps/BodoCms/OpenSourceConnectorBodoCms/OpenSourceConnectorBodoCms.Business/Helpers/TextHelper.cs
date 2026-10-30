// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Reflection;

namespace OpenSourceConnectorBodoCms.Business.Helpers;

public class TextHelper
{

    /// <summary>
    /// Get a text from a embedded resource file
    /// </summary>
    /// <param name="resourceName">resource name = file name</param>
    /// <returns></returns>
    public static string GetTextResource(string resourceName)
    {
        var ass = Assembly.GetExecutingAssembly();
        var str = ass.GetManifestResourceStream($"OpenSourceConnectorBodoCms.Business.Resources.{resourceName}.txt");

        if (str == null) return null;

        string s;

        using (var file = new StreamReader(str))
        {
            s = file.ReadToEnd();
        }

        return s;
    }

}