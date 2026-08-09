//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using Avalonia.Controls;
//using Avalonia.Markup.Xaml;
//using Avalonia.Markup.Xaml.Styling;
//using Bodoconsult.I18N.ResourceProviders;
//using System.Collections;
//using System.Reflection;
//using System.Resources;

//namespace Bodoconsult.App.Avalonia.I18N;

///// <summary>
///// Provider for embedded WPF resources
///// </summary>
//public class AvaloniaEmbeddedResourceProvider : BaseResourceProvider
//{
//    private readonly string _resourceFolder;

//    private readonly Assembly _assembly;


//    /// <summary>
//    /// Default ctor
//    /// </summary>
//    /// <param name="assembly">Assembly to load the locales from</param>
//    /// <param name="resourceFolder">Folder relative to app path the locales are stored in. Locales file must be named Culture.XX.xaml with XX being the language identifier</param>
//    public AvaloniaEmbeddedResourceProvider(Assembly assembly, string resourceFolder)
//    {
//        _assembly = assembly;
//        _resourceFolder = resourceFolder;

//        if (_resourceFolder.StartsWith("/"))
//        {
//            _resourceFolder = _resourceFolder[1..];
//        }

//        if (_resourceFolder.EndsWith("/"))
//        {
//            _resourceFolder = _resourceFolder[..^1];
//        }

//    }

//    /// <summary>
//    /// Register all available resource items
//    /// </summary>
//    public override void RegisterLocaleItems()
//    {
//        var assName = _assembly.GetName().Name;

//        var lResourceContainerName = $"{assName}";
//        var lResourceManager = new ResourceManager(lResourceContainerName, _assembly);

//        var lResourceSet = lResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true);

//        if (lResourceSet is null)
//        {
//            return;
//        }

//        foreach (DictionaryEntry lEesource in lResourceSet)
//        {
//            if (lEesource.ToString() is null)
//            {
//                continue;
//            }

//            var key = lEesource.Key.ToString().Split(',')[0]
//                .ToLowerInvariant()
//                .Replace($"{_resourceFolder}/", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace(".axaml", string.Empty);

//            if (!key.StartsWith("culture."))
//            {
//                continue;
//            }

//            var path = $"avares://{assName}/{_resourceFolder}/{key}.axaml";
//            var kvp = new KeyValuePair<string, string>(key.Replace("culture.", string.Empty), path);

//            ResourceItems.Add(kvp);
//        }
//    }

//    /// <summary>
//    /// Load key value pairs for string translations in a translation dictionary.
//    /// If a key is already contained in the translation dictionary it should not be added again.
//    /// </summary>
//    /// <param name="language">Requested language</param>
//    /// <param name="translations">Central translation dictionary to store the key value pairs in.
//    /// </param>
//    public override void LoadResourceItem(string language, IDictionary<string, string> translations)
//    {
//        // Check if language exists
//        var success = ResourceItems.TryGetValue(language, out var path);

//        if (!success)
//        {
//            return;
//        }

//        var rd = new ResourceDictionary();
//        rd.MergedDictionaries.Add(new ResourceInclude(new Uri(path, UriKind.RelativeOrAbsolute)));

//        foreach (var key in rd.Keys)
//        {
//            //if (key is null)
//            //{
//            //    continue;
//            //}
//            var value = rd[key];
//            if (value is null)
//            {
//                continue;
//            }

//            var kvp = new KeyValuePair<string, string>(key.ToString(), value.ToString());
//            translations.Add(kvp);
//        }
//    }
//}