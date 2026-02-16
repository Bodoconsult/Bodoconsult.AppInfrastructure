// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Wpf.Models;

namespace Bodoconsult.App.Wpf.Services;

/// <summary>
/// Load resources in a cache and search resourekeys
/// </summary>
public static class ResourceFinderService
{

    /// <summary>
    ///  Default ctor loading pack scheme
    /// </summary>
    static ResourceFinderService()
    {
        var s = System.IO.Packaging.PackUriHelper.UriSchemePack;
        Debug.Print(s);
    }

    private class CacheObject
    {

        public string Key;

        public SharedResourceDictionary ResourceDictionary;

#pragma warning disable 414
        public long Ticks;
#pragma warning restore 414

    }

    private static readonly IList<CacheObject> CachedResources = new List<CacheObject>();


    /// <summary>
    /// Find a resource key and return object
    /// </summary>
    /// <param name="path">resource path</param>
    /// <param name="resourceKey">resource key</param>
    /// <returns>Object found or null</returns>
    public static object FindResource(string path, string resourceKey)
    {

        try
        {
            var cache = CachedResources.FirstOrDefault(x => x.Key == path.ToLower());

            SharedResourceDictionary rd;

            if (cache == null)
            {
                rd = new SharedResourceDictionary
                {
                    Source = new Uri(path, UriKind.RelativeOrAbsolute)
                };

                CachedResources.Add(new CacheObject
                {
                    Key = path.ToLower(),
                    ResourceDictionary = rd,
                    Ticks = DateTime.Now.Ticks
                });

            }
            else
            {
                rd = cache.ResourceDictionary;
            }

            return rd[resourceKey];
        }
        catch (Exception ex)
        {
            throw new Exception($"ResourcePath: {path}: {resourceKey}", ex);
        }
    }

    /// <summary>
    /// Find a resource key and return object of type T
    /// </summary>
    /// <typeparam name="T">Type to convert the resource into</typeparam>
    /// <param name="path">resource path</param>
    /// <param name="resourceKey">resource key</param>
    /// <returns>Object found or null</returns>
    public static T FindResource<T>(string path, string resourceKey)
    {

        try
        {
            var cache = CachedResources.FirstOrDefault(x => x.Key == path.ToLower());


            SharedResourceDictionary rd;

            if (cache == null)
            {
                rd = new SharedResourceDictionary
                {
                    Source = new Uri(path, UriKind.RelativeOrAbsolute)
                };

                CachedResources.Add(new CacheObject
                {
                    Key = path.ToLower(),
                    ResourceDictionary = rd,
                    Ticks = DateTime.Now.Ticks
                });

            }
            else
            {
                cache.Ticks = DateTime.Now.Ticks;
                rd = cache.ResourceDictionary;
            }

            return (T)rd[resourceKey];
        }
        catch (Exception ex)
        {
            throw new Exception($"ResourcePath: {path}", ex);
        }
    }

    /// <summary>
    /// Number of resource dictionaries currently cached
    /// </summary>
    public static int Count => CachedResources.Count;


    /// <summary>
    /// Update a resource file in memory
    /// </summary>
    /// <param name="path">resource path</param>
    /// <param name="resourceKey">resource key</param>
    /// <param name="value">new value</param>
    /// <exception cref="Exception"></exception>
    public static void SetResource(string path, string resourceKey, string value)
    {
        try
        {
            var cache = CachedResources.FirstOrDefault(x => x.Key == path.ToLower());

            SharedResourceDictionary rd;

            if (cache == null)
            {
                rd = new SharedResourceDictionary
                {
                    Source = new Uri(path, UriKind.RelativeOrAbsolute)
                };

                CachedResources.Add(new CacheObject
                {
                    Key = path.ToLower(),
                    ResourceDictionary = rd,
                    Ticks = DateTime.Now.Ticks
                });

            }
            else
            {
                cache.Ticks = DateTime.Now.Ticks;
                rd = cache.ResourceDictionary;
            }

            rd[resourceKey] = value;
        }
        catch (Exception ex)
        {
            throw new Exception($"ResourcePath: {path}", ex);
        }
    }


    /// <summary>
    /// Update a resource file in memory
    /// </summary>
    /// <typeparam name="T">Type to convert the resource into</typeparam>
    /// <param name="path">resource path</param>
    /// <param name="resourceKey">resource key</param>
    /// <param name="value">new value</param>
    /// <exception cref="Exception"></exception>
    public static void SetResource<T>(string path, string resourceKey, T value)
    {
        try
        {
            var cache = CachedResources.FirstOrDefault(x => x.Key == path.ToLower());

            SharedResourceDictionary rd;

            if (cache == null)
            {
                rd = new SharedResourceDictionary
                {
                    Source = new Uri(path, UriKind.RelativeOrAbsolute)
                };

                CachedResources.Add(new CacheObject
                {
                    Key = path.ToLower(),
                    ResourceDictionary = rd,
                    Ticks = DateTime.Now.Ticks
                });

            }
            else
            {
                cache.Ticks = DateTime.Now.Ticks;
                rd = cache.ResourceDictionary;
            }

            rd[resourceKey] = value;
        }
        catch (Exception ex)
        {
            throw new Exception($"ResourcePath: {path}", ex);
        }
    }
}
