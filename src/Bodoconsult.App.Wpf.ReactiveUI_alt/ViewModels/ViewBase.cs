//using System.Windows.Input;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{
//    public class ViewModelBase:  INavigationAware
//    {

//        private IRegionNavigationService _navigationService;

//        public void OnNavigatedTo(NavigationContext navigationContext)
//        {
//            _navigationService = navigationContext.NavigationService;
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

//        public void GoBack()
//        {
//            if (_navigationService.Journal.CanGoBack)
//            {
//                _navigationService.Journal.GoBack();
//            }
//        }

//        public bool CanGoBack(object commandArg)
//        {
//            return _navigationService.Journal.CanGoBack;
//        }
//    }
//}
