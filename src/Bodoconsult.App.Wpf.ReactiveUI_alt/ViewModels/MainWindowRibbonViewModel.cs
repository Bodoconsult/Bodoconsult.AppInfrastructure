//using System.Windows.Controls.Ribbon;
//using Bodoconsult.App.Wpf.ReactiveUI.Models;
//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{
//    [ImplementPropertyChanged]
//    public class MainWindowRibbonViewModel: ViewModelBase
//    {

//        public MainWindowRibbonViewModel(IEventAggregator eventAggregator)
//        {
//            eventAggregator.GetEvent<StatusChangedEvent>().Subscribe(ShowMessage);
//            QuickAccessToolbarHeight = 30;
//        }

//        /// <summary>
//        /// The current ribbon of the MainWindowControl
//        /// </summary>
//        public Ribbon CurrentRibbon { get; set; }


//        //private IRibbonService _ribbonService;

//        /// <summary>
//        /// Used for PRISM navigation
//        /// </summary>
//        /// <param name="navigationContext"></param>
//        public override void OnNavigatedTo(NavigationContext navigationContext)
//        {
//            ShowMessage("Main menu is loading...");

//            var data = (DataContainer<IRibbonService>)navigationContext.Parameters["Data"];

//            var ribbonService = data.Data;
//            ribbonService.CurrentRibbon = CurrentRibbon;
//            ribbonService.BuildRibbon();
            
//            base.OnNavigatedTo(navigationContext);

//            ShowMessage("Main menu loaded!");
//        }


//        private void ShowMessage(string message)
//        {
//            Message = message;
//        }

//        /// <summary>
//        /// Message to show in the statusbar
//        /// </summary>
//        public string Message { get; set; }

        
//        public double QuickAccessToolbarHeight { get; set; }
//    }
//}
