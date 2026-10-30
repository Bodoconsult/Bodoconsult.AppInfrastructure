//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using System.Reflection;
//using OpenSourceConnectorBodoCms.Business.Helpers;
//using OpenSourceConnectorBodoCms.Business.Model;

//namespace OpenSourceConnectorBodoCms.Business;

//public static class GlobalValues
//{
//    /// <summary>
//    /// Current app settings
//    /// </summary>
//    public static AppSettings CurrentAppSettings { get; private set; }

//    /// <summary>
//    /// Load app settings from app directory
//    /// </summary>
//    public static void LoadAppSettings()
//    {

//        var path = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

//        var fileName = Path.Combine(path, "appSettings.json");

//        CurrentAppSettings = JsonHelper.LoadJsonFile<AppSettings>(fileName);
//    }


//}