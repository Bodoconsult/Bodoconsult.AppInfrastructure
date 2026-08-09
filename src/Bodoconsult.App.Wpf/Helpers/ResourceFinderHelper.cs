//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Bodoconsult.App.Wpf.Models;

//namespace Bodoconsult.App.Wpf.Helpers;

///// <summary>
///// Load resources in a cache and search resourekeys
///// </summary>
//public static class ResourceFinderHelper
//{

//    private class CacheObject
//    {

//        public string Key;

//        public SharedResourceDictionary ResourceDictionary;

//#pragma warning disable 414
//        public long Ticks;
//#pragma warning restore 414

//    }


//    private static IList<CacheObject> _cachedResources;

//    /// <summary>
//    /// Clear the cache
//    /// </summary>
//    public static void ClearCache()
//    {
//        _cachedResources?.Clear();
//    }


//    /// <summary>
//    /// Find a resource lkey and return object
//    /// </summary>
//    /// <param name="path">resource path</param>
//    /// <param name="resourceKey">resource key</param>
//    /// <returns></returns>
//    public static object FindResource(string path, string resourceKey)
//    {

//        try
//        {
//            var rd = CheckCache(path);

//            return rd[resourceKey];
//        }
//        catch (Exception ex)
//        {       
//            throw new Exception($"ResourcePath: {path}", ex);
//        }
//    }


//    /// <summary>
//    /// Find a resource key and return object of type T
//    /// </summary>
//    /// <typeparam name="T">Type to convert the resource into</typeparam>
//    /// <param name="path">resource path</param>
//    /// <param name="resourceKey">resource key</param>
//    /// <returns></returns>
//    public static T FindResource<T>(string path, string resourceKey)
//    {

//        try
//        {
//            var rd = CheckCache(path);

//            return (T)rd[resourceKey];
//        }
//        catch (Exception ex)
//        {
//            throw new Exception($"ResourcePath: {path}", ex);
//        }
//    }

//    /// <summary>
//    /// Number of resource dictionaries currently cached
//    /// </summary>
//    public static int Count => _cachedResources.Count;


//    /// <summary>
//    /// Update a resource file in memory
//    /// </summary>
//    /// <param name="path">resource path</param>
//    /// <param name="resourceKey">resource key</param>
//    /// <param name="value">new value</param>
//    /// <exception cref="Exception"></exception>
//    public static void SetResource(string path, string resourceKey, string value)
//    {
//        try
//        {

//            var rd = CheckCache(path);

//            rd[resourceKey] = value;
//        }
//        catch (Exception ex)
//        {
//            throw new Exception($"ResourcePath: {path}", ex);
//        }
//    }


//    /// <summary>
//    /// Update a resource file in memory
//    /// </summary>
//    /// <typeparam name="T">Type to convert the resource into</typeparam>
//    /// <param name="path">resource path</param>
//    /// <param name="resourceKey">resource key</param>
//    /// <param name="value">new value</param>
//    /// <exception cref="Exception"></exception>
//    public static void SetResource<T>(string path, string resourceKey, T value)
//    {
//        try
//        {
//            var rd = CheckCache(path);
//            rd[resourceKey] = value;
//        }
//        catch (Exception ex)
//        {
//            throw new Exception($"ResourcePath: {path}", ex);
//        }
//    }


//    /// <summary>
//    /// Check in the cache is a resource dictionary is loaded already. If not load it
//    /// </summary>
//    /// <param name="path">Path to the resource dictionary</param>
//    /// <returns>Loaded resource dictionary</returns>
//    private static SharedResourceDictionary CheckCache(string path)
//    {
//        _cachedResources ??= new List<CacheObject>();

//        var cache = _cachedResources.FirstOrDefault(x => x.Key == path.ToLowerInvariant());

//        SharedResourceDictionary rd;

//        if (cache is null)
//        {
//            rd = new SharedResourceDictionary
//            {
//                Source = new Uri(path, UriKind.RelativeOrAbsolute)
//            };

//            _cachedResources.Add(new CacheObject
//            {
//                Key = path.ToLowerInvariant(),
//                ResourceDictionary = rd,
//                Ticks = DateTime.Now.Ticks
//            });
//        }
//        else
//        {
//            rd = cache.ResourceDictionary;
//        }

//        return rd;
//    }
//}