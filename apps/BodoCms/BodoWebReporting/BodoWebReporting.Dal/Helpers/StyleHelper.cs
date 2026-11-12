//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


//using Bodoconsult.Pdf.Interfaces;
//using BodoWebReporting.Dal.Styles;

//namespace BodoWebReporting.Dal.Helpers;

//public static class StyleHelper
//{
//    /// <summary>
//    /// Generate a stylesheet
//    /// </summary>
//    /// <param name="landscape">Orientation is landscape?</param>
//    public static IStyleSet CreateStyleSet(bool landscape)
//    {
//        var s = new BodoWebReportingStyleSet();
//        s.CreatePageSetup(landscape);
//        s.CalculateMeasures();
//        s.InitializeStyles();
//        return s;
//    }

//    //private static bool isStart;

//    public static string? FileName { get; set;  }

//    //private static object? _instance;

//    public static void Start()
//    {

//        ////isStart = true;

//        //var compiler = new Compiler();
//        //var runner = new Runner();


//        //var provider = CodeDomProvider.CreateProvider("CSharp");
//        //var parameters = new CompilerParameters { GenerateInMemory = true };

//        //var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

//        //parameters.CompilerOptions = $"/optimize /lib:\"{dir}\"";

//        //parameters.ReferencedAssemblies.Add("System.dll");
//        //parameters.ReferencedAssemblies.Add("Bodoconsult.Pdf.dll");
//        //parameters.ReferencedAssemblies.Add("MigraDoc.DocumentObjectModel-gdi.dll");
//        //parameters.ReferencedAssemblies.Add("BodoWebReporting.Base.dll");

//        //// BodoWebReporting

//        ////MigraDoc.DocumentObjectModel.dll

//        //var result = provider.CompileAssemblyFromFile(parameters, FileName);
//        //if (result.Errors.HasErrors)
//        //{
//        //    return;
//        //}
//        //_instance = result.CompiledAssembly.CreateInstance("BodoWebReporting.Base.CodeImport");

//    }


//    /// <summary>
//    /// Code aus Datei dynamisch ausführen
//    /// </summary>
//    public static void  RunCode()
//    {
//        //if (!isStart) return;

//        //if (_instance == null)
//        //{
//        //    return;
//        //}

//        //var parametersArray = new object[] { };

//        //var info = _instance.GetType().GetMethod("Start");
//        //if (info != null) info.Invoke(_instance, parametersArray);

//        //isStart = false;
//    }
//}