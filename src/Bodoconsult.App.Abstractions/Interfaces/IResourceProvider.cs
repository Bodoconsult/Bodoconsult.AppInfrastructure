//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


//namespace Bodoconsult.App.Abstractions.Interfaces;

///// <summary>
///// Base interface for I18N localization file resource providers
///// </summary>
//public interface IResourceProvider
//{

//    /// <summary>
//    /// All available resource items
//    /// </summary>
//    IDictionary<string, string> ResourceItems { get; }


//    /// <summary>
//    /// Register all available resource items
//    /// </summary>
//    void RegisterLocaleItems();


//    /// <summary>
//    /// Load key value pairs for string translations in a translation dictionary.
//    /// If a key is already contained in the translation dictionary it should not be added again.
//    /// </summary>
//    /// <param name="language">Requested language</param>
//    /// <param name="translations">Central translation dictionary to store the key value pairs in.
//    /// </param>
//    void LoadResourceItem(string language, IDictionary<string, string> translations);


//}