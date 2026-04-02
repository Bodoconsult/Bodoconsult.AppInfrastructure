// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bodoconsult.App.Wpf.Interfaces;

namespace Bodoconsult.App.Wpf.Services;

/// <summary>
/// Base implementation of <see cref="IRibbonService"/> to create a ribbon
/// </summary>
public class RibbonBaseService : IRibbonService
{
    private const string QuickAccessName = "QuickAccess";
    private const string ApplicationMenuName = "ApplicationMenu";

    /// <summary>
    /// Group item
    /// </summary>
    private struct GroupItem
    {
        /// <summary>
        /// Tab name
        /// </summary>
        public string TabName;

        /// <summary>
        /// Group name
        /// </summary>
        public string GroupName;
    }

    private readonly Dictionary<int, RibbonItem> _items =new();
    private readonly List<string> _tabs = [];
    private readonly List<GroupItem> _tabGroups = [];

    ///// <summary>
    ///// Default ctor
    ///// </summary>
    //public RibbonBaseService()
    //{
    //    _tabs = new List<string>();
    //    _tabGroups = new List<GroupItem>();
    //    _items = new Dictionary<int, RibbonItem>();
    //}

    /// <summary>
    /// The current ribbon control to build
    /// </summary>
    public Ribbon CurrentRibbon { get; set; }


    /// <summary>
    /// Add a ribbon item to the quick access bar
    /// </summary>
    /// <param name="ribbonItem"></param>
    public void AddQuickAccessItem(RibbonItem ribbonItem)
    {
        ribbonItem.TabName = QuickAccessName;
        _items.Add(_items.Count, ribbonItem);
    }
    /// <summary>
    /// Add a ribbon item to the application menu
    /// </summary>
    /// <param name="ribbonItem"></param>
    public void AddApplicationMenuItem(RibbonItem ribbonItem)
    {
        ribbonItem.TabName = ApplicationMenuName;
        _items.Add(_items.Count, ribbonItem);
    }

    /// <summary>
    /// Add a ribbon item to a tab
    /// </summary>
    /// <param name="ribbonItem"></param>
    public void AddTabItem(RibbonItem ribbonItem)
    {
        if (!_tabs.Contains(ribbonItem.TabName))
        {
            _tabs.Add(ribbonItem.TabName);
        }

        if (!_tabGroups.Any(x => x.TabName == ribbonItem.TabName && ribbonItem.TabGroupName == x.GroupName))
        {
            _tabGroups.Add(new GroupItem
            {
                GroupName = ribbonItem.TabGroupName,
                TabName = ribbonItem.TabName
            });
        }

        _items.Add(_items.Count, ribbonItem);
    }

    /// <summary>
    /// Override this method to add your own items to the ribbon. Ribbon starts completely empty otherwise.
    /// </summary>
    public virtual void DefineRibbonItems()
    {

        // QuickAccess

        var item = new RibbonItem
        {
            TabName = "QuickAccessBar",
            TabGroupName = "Gruppe1",
            Header = "Speichern",
            SmallImagePath = "pack://application:,,,/Bodoconsult.Wpf.Base;component/Resources/Styling/BitmapGraphics/Assets/save.png"
        };

        AddQuickAccessItem(item);

        item = new RibbonItem
        {
            TabName = "QuickAccessBar",
            TabGroupName = "Gruppe1",
            Header = "Öffnen",
            SmallImagePath = "pack://application:,,,/Bodoconsult.Wpf.Base;component/Resources/Styling/BitmapGraphics/Assets/aligncenter.png"
        };

        AddQuickAccessItem(item);


        // Application menu
        item = new RibbonItem
        {
            TabName = "QuickAccessBar",
            TabGroupName = "Gruppe1",
            Header = "Speichern",
            SmallImagePath = "pack://application:,,,/Bodoconsult.Wpf.Base;component/Resources/Styling/BitmapGraphics/Assets/save.png"
        };

        AddApplicationMenuItem(item);

        item = new RibbonItem
        {
            TabName = "QuickAccessBar",
            TabGroupName = "Gruppe1",
            Header = "Öffnen",
            SmallImagePath = "pack://application:,,,/Bodoconsult.Wpf.Base;component/Resources/Styling/BitmapGraphics/Assets/aligncenter.png"
        };

        AddApplicationMenuItem(item);

        item = new RibbonItem
        {
            TabName = "QuickAccessBar",
            TabGroupName = "Gruppe1",
            Header = "Excel-Periodendaten laden",
            SmallImagePath = "pack://application:,,,/Bodoconsult.Simulation.Wpf;component/Resources/Styling/BitmapGraphics/Assets/IconExcel.png"
        };

        AddApplicationMenuItem(item);

 
        // tab Start
        item = new RibbonItem
        {
            TabName = "Start",
            TabGroupName = "Gruppe1",
            Header = "Projektdaten bearbeiten",
            LargeImagePath = "pack://application:,,,/Bodoconsult.Wpf.Base;component/Resources/Styling/BitmapGraphics/Assets/save.png"
        };


        AddTabItem(item);


    }

    /// <summary>
    /// Build the concrete ribbon. Called normally in the view model of the main menu
    /// </summary>
    public void BuildRibbon()
    {

        BuildQuickAccess();

        BuildApplicationMenu();

        BuildTabs();
    }

    /// <summary>
    /// Build the application menu for the ribbon
    /// </summary>
    private void BuildApplicationMenu()
    {
        var items = _items
            .Where(x => x.Value.TabName == ApplicationMenuName)
            .OrderBy(x => x.Key)
            .Select(x => x.Value).ToList();

        if (items.Count == 0)
        {
            return;
        }

        var appMenu = new RibbonApplicationMenu();

        CurrentRibbon.ApplicationMenu = appMenu;

        foreach (var item in _items
                     .Where(x => x.Value.TabName == ApplicationMenuName)
                     .OrderBy(x => x.Key)
                     .Select(x => x.Value))
        {
            var button = GetApplicationMenuItem(item);
            appMenu.Items.Add(button);
        }
    }



    /// <summary>
    /// Build the quick access toolbar
    /// </summary>
    private void BuildQuickAccess()
    {

        var items = _items
            .Where(x => x.Value.TabName == QuickAccessName)
            .OrderBy(x => x.Key)
            .Select(x => x.Value).ToList();

        if (items.Count == 0)
        {
            return;
        }

        var quickAccess = new RibbonQuickAccessToolBar
        {
            VerticalAlignment = VerticalAlignment.Center
        };

        CurrentRibbon.QuickAccessToolBar = quickAccess;


        foreach (var item in _items
                     .Where(x => x.Value.TabName == QuickAccessName)
                     .OrderBy(x => x.Key)
                     .Select(x => x.Value))
        {
            var button = GetRibbonButton(item);
            quickAccess.Items.Add(button);
        }
    }



    /// <summary>
    /// Builds the tabs for the ribbon
    /// </summary>
    private void BuildTabs()
    {
        foreach (var tabName in _tabs)
        {
            var name = tabName;
            var tab = new RibbonTab
            {
                Header = name
            };

            foreach (var groupItem in _tabGroups.Where(x => x.TabName == name))
            {
                var group = groupItem;

                var ribbonGroup = new RibbonGroup();

                foreach (var item in _items
                             .Where(x => x.Value.TabName == name && x.Value.TabGroupName == group.GroupName)
                             .OrderBy(x => x.Key)
                             .Select(x => x.Value))
                {
                    var button = GetRibbonButton(item);
                    ribbonGroup.Items.Add(button);
                }

                tab.Items.Add(ribbonGroup);
            }

            CurrentRibbon.Items.Add(tab);
        }

        CurrentRibbon.UpdateLayout();
    }


    #region Helpers


    private static RibbonButton GetRibbonButton(RibbonItem item)
    {
        var erg = new RibbonButton
        {
            Label = item.Header,
            ToolTipDescription = item.Header
        };

        if (!string.IsNullOrEmpty(item.SmallImagePath))
        {
            erg.SmallImageSource = GetBitmapImage(item.SmallImagePath, 16, 16);
        }

        if (!string.IsNullOrEmpty(item.LargeImagePath))
        {
            erg.LargeImageSource = GetBitmapImage(item.LargeImagePath, 32, 32);
        }


        if (item.Command != null)
        {
            erg.Command = item.Command;
        } 

        return erg;
    }

    private static TransformedBitmap GetBitmapImage(string path, double width, double height)
    {
 
        var bimg = new BitmapImage();
        bimg.BeginInit();

        var uri = new Uri(path, UriKind.RelativeOrAbsolute);

        bimg.UriSource = uri;
        bimg.CacheOption = BitmapCacheOption.OnLoad;

        bimg.EndInit();

        var bitmap = new TransformedBitmap(bimg,
            new ScaleTransform(
                width / bimg.PixelWidth,
                height / bimg.PixelHeight));

        return bitmap;
    }

    private static RibbonApplicationMenuItem GetApplicationMenuItem(RibbonItem item)
    {
        var erg = new RibbonApplicationMenuItem
        {
            Header = item.Header,
            ToolTipDescription = item.Header
        };


        if (!string.IsNullOrEmpty(item.LargeImagePath))
        {
            erg.ImageSource = GetBitmapImage(item.LargeImagePath, 32, 32);
        }

        if (!string.IsNullOrEmpty(item.SmallImagePath))
        {
            erg.ImageSource = GetBitmapImage(item.SmallImagePath, 16, 16);
        }

        if (item.Command != null)
        {
            erg.Command = item.Command;
        }

        return erg;
    }

    #endregion
}