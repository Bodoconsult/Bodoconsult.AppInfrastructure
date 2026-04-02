//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using Avalonia;
//using Avalonia.Markup.Xaml;

//namespace Bodoconsult.App.Avalonia.ReactiveUI.Helper;

///// <summary>
///// Get styles from Bodoconsult.Avalonia.Base assembly
///// </summary>
//public class AvaloniaBaseResourceExtension : MarkupExtension
//{
//    /// <summary>
//    /// Resource key which we want to extract
//    /// </summary>
//    public string ResourceKey { get; set; } = string.Empty;
//    /// <summary>
//    /// Overriding base function which will return key from RD
//    /// </summary>
//    /// <param name="serviceProvider">Not used</param>
//    /// <returns>Object from RD</returns>
//    public override object ProvideValue(IServiceProvider serviceProvider)
//    {
//        return Application.Styles, ResourceKey);
//    }
//}