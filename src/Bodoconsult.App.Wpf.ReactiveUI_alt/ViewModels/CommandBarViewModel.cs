//using System.Collections.ObjectModel;
//using Bodoconsult.App.Wpf.ReactiveUI.Models;
//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{

//    /// <summary>
//    /// Contains functionality for a command bar
//    /// </summary>
//    [ImplementPropertyChanged]
//    public class CommandBarViewModel
//    {

//        internal IEventAggregator EventAggregator;

//        public CommandBarViewModel(IEventAggregator eventAggregator)
//        {
//            EventAggregator = eventAggregator;
            
//            EventAggregator.GetEvent<CommandBarAddButtonEvent>().Subscribe(AddButton);
//            EventAggregator.GetEvent<CommandBarClearAllEvent>().Subscribe(ClearAll);

//            Buttons = new ObservableCollection<CommandBarButton>();

//            // Must be the last line in this ctor
//            EventAggregator.GetEvent<MainWindowCommandBarReadyToLoadEvent>().Publish(this);
//        }

//        /// <summary>
//        /// Conatins all buttons added to the command bar
//        /// </summary>
//        public ObservableCollection<CommandBarButton> Buttons { get; internal set; }


//        /// <summary>
//        /// Add a button to the command bar
//        /// </summary>
//        /// <param name="buttonData">data of a new button to be added to the commandbar</param>
//        public void AddButton(CommandBarButton buttonData)
//        {
//            Buttons.Add(buttonData);
//        }


//        /// <summary>
//        /// Clear all buttons
//        /// </summary>
//        /// <param name="obj"></param>
//        private void ClearAll(bool obj)
//        {
//            Buttons.Clear();
//        }
//    }
//}
