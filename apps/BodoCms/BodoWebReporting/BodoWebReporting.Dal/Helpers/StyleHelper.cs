// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Pdf.Stylesets;
using BodoWebReporting.Dal.Styles;
using MigraDoc.DocumentObjectModel;

namespace BodoWebReporting.Dal.Helpers;

public static class StyleHelper
{
    public static Color BackColor { get; set; } = Colors.White;
    public static Color TableBorderColor { get; set; } = new(217, 230, 245);

    public static Color AlternateBackColor { get; set; } = new(217, 230, 245);


    public static IStyleSet StyleSet { get; set; } = new BodoWebReportingStyleSet();

    //private static bool isStart;

    public static string? FileName { get; set;  }

    //private static object? _instance;

    public static void Start()
    {

        ////isStart = true;

        //var compiler = new Compiler();
        //var runner = new Runner();


        //var provider = CodeDomProvider.CreateProvider("CSharp");
        //var parameters = new CompilerParameters { GenerateInMemory = true };

        //var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        //parameters.CompilerOptions = $"/optimize /lib:\"{dir}\"";

        //parameters.ReferencedAssemblies.Add("System.dll");
        //parameters.ReferencedAssemblies.Add("Bodoconsult.Pdf.dll");
        //parameters.ReferencedAssemblies.Add("MigraDoc.DocumentObjectModel-gdi.dll");
        //parameters.ReferencedAssemblies.Add("BodoWebReporting.Base.dll");

        //// BodoWebReporting

        ////MigraDoc.DocumentObjectModel.dll

        //var result = provider.CompileAssemblyFromFile(parameters, FileName);
        //if (result.Errors.HasErrors)
        //{
        //    return;
        //}
        //_instance = result.CompiledAssembly.CreateInstance("BodoWebReporting.Base.CodeImport");

    }


    /// <summary>
    /// Code aus Datei dynamisch ausführen
    /// </summary>
    public static void  RunCode()
    {
        //if (!isStart) return;
        StyleSet = new BodoWebReportingStyleSet();

        //if (_instance == null)
        //{
        //    return;
        //}

        //var parametersArray = new object[] { };

        //var info = _instance.GetType().GetMethod("Start");
        //if (info != null) info.Invoke(_instance, parametersArray);

        //isStart = false;
    }
}