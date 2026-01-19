using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for DummyControl.xaml
    /// </summary>
    public partial class DummyControl
    {
        public DummyControl(DummyViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }
    }
}
