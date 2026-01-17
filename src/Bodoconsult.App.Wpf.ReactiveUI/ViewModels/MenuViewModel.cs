//using System.Collections.ObjectModel;
//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{
//    [ImplementPropertyChanged]
//    public class MenuViewModel
//    {
//        //readonly MenuService _menuService = new MenuService();

//        public MenuViewModel(IEventAggregator eventAggregator)
//        {
//            MainMenu = new ObservableCollection<BaseMenuItem>();


//            eventAggregator.GetEvent<MainWindowMenuBarReadyToLoadEvent>().Publish(this);
//        }



//        public ObservableCollection<BaseMenuItem> MainMenu { get; set; }


//        public void AddMenuItem(BaseMenuItem menuItem)
//        {
//            MainMenu.Add(menuItem);
//        }
//    }



//}
