//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{
//    [ImplementPropertyChanged]
//    public class MainWindowViewModel
//    {
//        //internal IEventAggregator EventAggregator;

//        public MainWindowViewModel(IEventAggregator eventAggregator)
//        {
//            eventAggregator.GetEvent<StatusChangedEvent>().Subscribe(ShowMessage);
//        }

//        private void ShowMessage(string message)
//        {
//            Message = message;
//        }

//        /// <summary>
//        /// Message to show in the statusbar
//        /// </summary>
//        public string Message { get; set; }

//    }
//}
