using System.Windows;
using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    /// <code> /// <summary>
    ///     /// Boostrapper.cs
    ///     /// </summary>
    ///     protected override void InitializeShell()
    ///     {
    ///             base.InitializeShell();
    ///     
    ///             var main = (Window)Shell;
    ///             Application.Current.MainWindow = main;
    ///             main.WindowState = WindowState.Maximized;
    ///             main.Show();   
    ///     
    ///             var data = new LoginData();
    ///             data.CheckLoginData += CheckLoginData;
    ///             data.UserName = "robert";
    ///             data.CancelButtonLabel = "Abbrechen";
    ///             data.LoginButtonLabel = "Anmelden";
    ///             data.PasswordLabel = "Paßwort:";
    ///             data.UserNameLabel = "Benutzername: ";
    ///             data.UserNameTooltip = "Bitte Benutzernamen eingeben.";
    ///             data.PasswordTooltip = "Bitte Paßwort eingeben.";
    ///             data.TitleLabel = "Anmelden an XY-App";
    ///     
    ///             var viewModel = new LoginWindowViewModel { LoginData = data };
    ///     
    ///             var form = new LoginWindow(viewModel) { Owner = main };
    ///             form.ShowDialog();
    ///     
    ///             if (form.DialogResult == false)
    ///            {
    ///                form.Close();
    ///               main.Close();
    ///            }
    ///            form.Close();
    ///         }
    /// 
    ///         /// <summary>
    ///         /// Method checking the provided login data
    ///         ///</summary>
    ///         ///<param>User name
    ///         ///<name>userName</name>
    ///     ////</param>
    ///         ///<param>Password
    ///         ///<name>password</name>
    ///     ///</param>
    ///         ///<returns>Returns true if login was successfully, otherwise false</returns>
    ///         private static bool CheckLoginData(string userName, string password)
    ///         {
    ///             return userName == "robert" && password == "test";
    ///         }
    /// 
    /// </code>
    public partial class LoginWindow
    {

        private readonly LoginWindowViewModel _model;

        public LoginWindow(LoginWindowViewModel model)
        {

            DataContext = model;
            _model = model;
            InitializeComponent();
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }


        private void BtnLoginClick(object sender, RoutedEventArgs e)
        {
            DialogResult = _model.TryLogin(TxtUserName.Text, TxtPassword.Password);
            Close();
        }
    }
}
