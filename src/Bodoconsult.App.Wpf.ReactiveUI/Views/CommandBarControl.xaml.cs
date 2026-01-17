using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for CommandBar.xaml
    /// </summary>
    public partial class CommandBarControl
    {
        public CommandBarControl(CommandBarViewModel model)
        {
            DataContext = model;

            InitializeComponent();

           
        }
    }
}
