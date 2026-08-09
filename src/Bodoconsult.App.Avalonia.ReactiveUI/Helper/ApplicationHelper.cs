//using System.Linq;
//using System.Windows;
//using System.Windows.Input;
//using Bodoconsult.App.Avalonia.ReactiveUI.Views;

//namespace Bodoconsult.App.Avalonia.ReactiveUI.Helper
//{
//    /// <summary>
//    /// Helper class for Avalonia application with useful methods related to PRISM
//    /// </summary>
//    public class ApplicationHelper
//    {

//        private static IUnityContainer _unityContainer;

//        private static IRegionManager _regionManager;

//        private static IEventAggregator _eventAggregator;

//        /// <summary>
//        /// Get the current Unity container for the application
//        /// </summary>
//        /// <returns></returns>
//        public static IUnityContainer GetContainer()
//        {
//            return _unityContainer ?? (_unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>());
//        }

//        /// <summary>
//        /// Get the current PRISM region manager for the application
//        /// </summary>
//        /// <returns></returns>
//        public static IRegionManager GetRegionManager()
//        {
//            return _regionManager ?? (_regionManager = ServiceLocator.Current.GetInstance<IRegionManager>());
//        }



//        /// <summary>
//        /// Get the current PRISM event aggregator for the application
//        /// </summary>
//        /// <returns></returns>
//        public static IEventAggregator GetEventAggregator()
//        {
//            if (!ServiceLocator.IsLocationProviderSet) return null;

//            return _eventAggregator ?? (_eventAggregator=ServiceLocator.Current.GetInstance<IEventAggregator>());
//        }

//        /// <summary>
//        /// Navigate to a view in a region
//        /// </summary>
//        /// <typeparam name="T">View type</typeparam>
//        /// <param name="regionName">Region name the view has to be loaded in</param>
//        public static void Navigate<T>(string regionName)
//        {
//            var regionManager = GetRegionManager();
//            var viewName = typeof(T).Name;
//            if (regionManager.Regions[regionName].Views.All(x => x.GetType().Name != viewName))
//                regionManager.RegisterViewWithRegion(regionName, () => GetContainer().Resolve<T>());
 
//            regionManager.RequestNavigate(regionName, (typeof (T)).Name);
//        }


//        /// <summary>
//        /// Navigate to a view in the region <see cref="BaseRegionNames.MainWindowDocumentRegion"/>
//        /// </summary>
//        /// <typeparam name="T">View type</typeparam>
//        public static void Navigate<T>()
//        {
//            var regionManager = GetRegionManager();
//            var viewName = typeof (T).Name;
//            if (regionManager.Regions[BaseRegionNames.MainWindowDocumentRegion].Views.All(x => x.GetType().Name != viewName)) 
//                regionManager.RegisterViewWithRegion(BaseRegionNames.MainWindowDocumentRegion, () => GetContainer().Resolve<T>());
//            regionManager.RequestNavigate(BaseRegionNames.MainWindowDocumentRegion, viewName);
//        }




//        private static ICommand _goBackCommand;

//        /// <summary>
//        /// Command for GoBack
//        /// </summary>	
//        public static ICommand GoBackCommand
//        {
//            get
//            {
//                return _goBackCommand ??
//                       (_goBackCommand = new DelegateCommand(GoBack, () => true));
//            }
//        }

//        /// <summary>
//        /// Go back in region <see cref="BaseRegionNames.MainWindowDocumentRegion"/>
//        /// </summary>
//        public static void GoBack()
//        {
//            var navigationService = GetRegionManager().Regions[BaseRegionNames.MainWindowDocumentRegion].NavigationService;
//            if (navigationService.Journal.CanGoBack)
//            {
//                navigationService.Journal.GoBack();
//            }
//        }

//        /// <summary>
//        /// Go back in a region
//        /// </summary>
//        public static void GoBack(string regionName)
//        {
//            var navigationService = GetRegionManager().Regions[regionName].NavigationService;
//            if (navigationService.Journal.CanGoBack)
//            {
//                navigationService.Journal.GoBack();
//            }
//        }


//        /// <summary>
//        /// Publish a event
//        /// </summary>
//        /// <typeparam name="TEvent">event to publish</typeparam>
//        /// <typeparam name="TInputData">input data type for the published event</typeparam>
//        /// <param name="inputdata">input data of type TInputData</param>
//        public static void PublishEvent<TEvent, TInputData>(TInputData inputdata) where TEvent: PubSubEvent<TInputData>, new()
//        {
//            GetEventAggregator().GetEvent<TEvent>().Publish(inputdata);
//        }


//        /// <summary>
//        /// Send a message to the status bar of main window
//        /// </summary>
//        /// <param name="message"></param>
//        public static void Status(string message)
//        {
//            GetEventAggregator().GetEvent<StatusChangedEvent>().Publish(message);
//        }



//        /// <summary>
//        /// Navigate to a view in the region <see cref="BaseRegionNames.MainWindowDocumentRegion"/> and then publish the event of type TEvent with input data of type TInputData
//        /// </summary>
//        /// <typeparam name="TView">View type</typeparam>
//        /// <typeparam name="TInputData">Input data type</typeparam>
//        /// <param name="inputdata">input data of type TInputData</param>
//        public static void Navigate<TView, TInputData>(TInputData inputdata) 
//        {
//            var regionManager = GetRegionManager();
//            var viewName = typeof (TView).Name;

//            if (regionManager.Regions[BaseRegionNames.MainWindowDocumentRegion].Views.All(x => x.GetType().Name != viewName)) 
//                regionManager.RegisterViewWithRegion(BaseRegionNames.MainWindowDocumentRegion, () => GetContainer().Resolve<TView>());

//            var parameters = new NavigationParameters {{"Data", inputdata}};

//            regionManager.RequestNavigate(BaseRegionNames.MainWindowDocumentRegion, viewName, parameters);

//            //GetEventAggregator().GetEvent<TEvent>().Publish(inputdata);
//        }


//        /// <summary>
//        /// Navigate to a view in the region <see cref="BaseRegionNames.MainWindowDocumentRegion"/> and then publish the event of type TEvent with input data of type TInputData
//        /// </summary>
//        /// <typeparam name="TView">View type</typeparam>
//        /// <typeparam name="TInputData">Input data type</typeparam>
//        /// <param name="regionName">name of the region the view should be loaded in</param>
//        /// <param name="inputdata">input data of type TInputData</param>
//        public static void Navigate<TView, TInputData>(string regionName, TInputData inputdata)
//        {
//            var regionManager = GetRegionManager();

//            var viewName = typeof (TView).Name;

//            if (regionManager.Regions[regionName].Views.All(x => x.GetType().Name != viewName)) regionManager.RegisterViewWithRegion(regionName, () => GetContainer().Resolve<TView>());

//            var parameters = new NavigationParameters { { "Data", inputdata } };

//            regionManager.RequestNavigate(regionName, viewName, parameters);

//            //GetEventAggregator().GetEvent<TEvent>().Publish(inputdata);
//        }



//        public static void RegisterMenuRegions(IUnityContainer container, IRegionManager regionManager)
//        {
//            //regionManager.RegisterViewWithRegion(BaseRegionNames.MainWindowDocumentRegion, () => container.Resolve<CommandBarControl>());

//            Navigate<CommandBarControl>(BaseRegionNames.MainWindowCommandBarRegion);
//            Navigate<MenuControl>(BaseRegionNames.MainWindowMenuBarRegion);
//        }

//        /// <summary>
//        /// Change the theme and the accent of the application
//        /// </summary>
//        /// <param name="themeName">name of the name</param>
//        /// <param name="accent">name of the accent</param>
//        public static void ChangeTheme(string themeName, string accent)
//        {
//            //// get the theme from the current application
//            //var theme = ThemeManager.DetectAppStyle(Application.Current);

//            // now set the Green accent and dark theme
//            ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
//                                        ThemeManager.GetAccent(accent),
//                                        ThemeManager.GetAppTheme(themeName));
//        }


//        #region Globalization

//        /// <summary>
//        /// Change the language for namespace Bodoconsult.Avalonia.Base
//        /// </summary>
//        /// <param name="language"></param>
//        public static void ChangeLanguage(string language)
//        {
//            //var source = "pack://application:,,,/Bodoconsult.App.Avalonia.ReactiveUI;component/Resources/Localization/Culture.de.xaml";

//            //if (language.StartsWith("en"))
//            //{
//            //    source = "pack://application:,,,/Bodoconsult.App.Avalonia.ReactiveUI;component/Resources/Localization/Culture.en.xaml";
//            //}

//            //var dict = new SharedResourceDictionary { Source = new Uri(source, UriKind.RelativeOrAbsolute) };

//            //System.Windows.Application.Current.Resources.MergedDictionaries.Add(dict);

//            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);

//        }

//        /// <summary>
//        /// Current language of the UI in lower case. Only first to letters of language code. Example: de, en, ...
//        /// </summary>
//        public static string CurrentLanguage
//        {
//            get { return System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Substring(0, 2).ToLower(); }
//        }

//        #endregion




//        #region Window titles and status message

//        /// <summary>
//        /// Keeps the clear text title of the current application
//        /// </summary>
//        public static string AppTitle { get; set; }


//        /// <summary>
//        /// Set window title for current application window. Format: AppTitle: message
//        /// </summary>
//        /// <param name="message">message to show in the window title bar. Format: AppTitle: message</param>
//        public static void SetCurrentWindowTitle(string message)
//        {
//            var window = System.Windows.Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.IsActive);

//            if (window is null) return;

//            window.Title = string.Format("{0}: {1}", AppTitle, message);
//        }


//        //public static void SetWindowTitleRegion(IRegionNavigationService navigationService, string viewName, string message)
//        //{
//        //    var window = navigationService.Region.RegionManager;

//        //   window.

//        //    if (window is null) return;

//        //    window.Title = string.Format("{0}: {1}", AppTitle, message);
//        //} 

//        /// <summary>
//        /// Send a status message
//        /// </summary>
//        /// <param name="message"></param>
//        public static void SendStatusMessage(string message)
//        {
//            if (!ServiceLocator.IsLocationProviderSet) return;

//            System.Windows.Application.Current.Dispatcher.Invoke(() =>
//            {
//                GetEventAggregator().GetEvent<StatusChangedEvent>().Publish(message);
//            });
//        }

//        #endregion


//    }
//}
