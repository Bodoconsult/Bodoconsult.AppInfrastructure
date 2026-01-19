using System.Windows;
using Bodoconsult.App.Wpf.ReactiveUI.Models;
using PropertyChanged;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    /// <code> 
    /// 
    ///         var data = new InputBoxData();
    /// 
    ///         new InputBox(data).ShowDialog();
    /// 
    ///         if (string.IsNullOrEmpty(data.UserInput))
    ///         {
    ///             WpfStandardDialogUtility.ShowInfo("InputBox", "Cancelled");
    ///         }
    ///         else
    ///         {
    ///             WpfStandardDialogUtility.ShowInfo("InputBox", "User input"+data.UserInput);
    ///         }
    /// 
    /// </code>
    [ImplementPropertyChanged]
    public partial class InputBox
    {

        internal InputBoxData Data;

        public InputBox(InputBoxData model)
        {
            Data = model;
            DataContext = Data;
            
            InitializeComponent();

            model.UserInput = "Hallo";
            Title = model.WindowTitle;

        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Data.UserInput = "";
            Close();
        }


        private void BtnLoginClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
