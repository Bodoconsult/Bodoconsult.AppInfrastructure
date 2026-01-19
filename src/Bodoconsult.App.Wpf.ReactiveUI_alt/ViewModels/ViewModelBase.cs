//using System.ComponentModel;
//using System.Diagnostics.CodeAnalysis;
//using System.Windows;
//using System.Windows.Input;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{
//    public class ViewModelBase : INavigationAware, INotifyPropertyChanged
//    {

//        internal IRegionNavigationService NavigationService;

//        public virtual void OnNavigatedTo(NavigationContext navigationContext)
//        {
//            NavigationService = navigationContext.NavigationService;
//        }

//        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
//        {
//            return true;
//        }

//        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
//        {

//        }

//        /// <summary>
//        /// Print Command
//        /// </summary>
//        private ICommand _goBackCommand;

//        public ICommand GoBackCommand
//        {
//            get
//            {
//                return _goBackCommand ??
//                       (_goBackCommand = new DelegateCommand(GoBack, () => true));
//            }
//        }

//        /// <summary>
//        /// Go back to last view
//        /// </summary>
//        public void GoBack()
//        {
//            if (NavigationService == null) return;

//            if (NavigationService.Journal.CanGoBack)
//            {
//                NavigationService.Journal.GoBack();
//            }

//        }

//        /// <summary>
//        /// Is it possible to go back to the view shown before the current
//        /// </summary>
//        /// <param name="commandArg"></param>
//        /// <returns></returns>
//        public bool CanGoBack(object commandArg)
//        {
//            return NavigationService != null && NavigationService.Journal.CanGoBack;
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void OnPropertyChanged(string propertyName = null)
//        {
//            var x = PropertyChanged;

//            if (x != null)
//            {
//                //DispatcherHelper.CheckBeginInvokeOnUI(() =>
//                //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName)));
//                x(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }


//        private static bool? _isInDesignMode;

//        /// <summary>
//        /// Gets a value indicating whether the control is in design mode (running in Blend
//        /// or Visual Studio).
//        /// </summary>
//        public static bool IsInDesignModeStatic
//        {
//            get
//            {
//                if (!_isInDesignMode.HasValue)
//                {
//#if SILVERLIGHT
//            _isInDesignMode = DesignerProperties.IsInDesignTool;
//#else
//                    var prop = DesignerProperties.IsInDesignModeProperty;
//                    _isInDesignMode
//                        = (bool)DependencyPropertyDescriptor
//                        .FromProperty(prop, typeof(FrameworkElement))
//                        .Metadata.DefaultValue;
//#endif
//                }

//                return _isInDesignMode.Value;
//            }
//        }

//        /// <summary>
//        /// Gets a value indicating whether the control is in design mode (running under Blend
//        /// or Visual Studio).
//        /// </summary>
//        [SuppressMessage(
//            "Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "Non static member needed for data binding")]
//        public bool IsInDesignMode
//        {
//            get
//            {
//                return IsInDesignModeStatic;
//            }
//        }


//    }
//}
