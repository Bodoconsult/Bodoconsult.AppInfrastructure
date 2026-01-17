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
    ///         var data = new ChangePasswordData
    ///         {
    ///               TitleLabel = "Paßwort ändern",
    ///               CancelButtonLabel = "Abbrechen",
    ///               ChangePasswordButtonLabel = "Paßwort ändern",
    ///               PasswordLabel = "Altes Paßwort:",
    ///               PasswordTooltip = "Bitte Paßwort eingeben.",
    ///               PasswordRepeatLabel = "Altes Paßwort wiederholen:",
    ///               PasswordRepeatTooltip = "Bitte altes Paßwort eingeben",
    ///               NewPasswordLabel = "Neues Paßwort:",
    ///               NewPasswordTooltip = "Bitte neues Paßwort eingeben"
    ///         };
    ///
    ///         data.CheckChangePasswordData += ChangePassword;
    ///
    ///         var viewModel = new ChangePasswordViewModel { ChangePasswordData = data };
    ///
    ///         var form = new ChangePasswordWindow(viewModel) { Owner = main };
    ///         form.ShowDialog();
    ///
    ///         var erg = form.DialogResult;
    ///
    ///         if (erg == false)
    ///         {
    ///             // Any reaction if password change was not succesful
    ///             form.Close();
    ///             main.Close();
    ///         }
    ///         form.ShowDialog();
    ///     
    ///             if (form.DialogResult == false)
    ///            {
    ///                form.Close();
    ///               main.Close();
    ///            }
    ///            form.Close();
    ///         }
    /// </code>
    public partial class ChangePasswordWindow
    {
        /// <summary>
        /// Message shown on the dialog window
        /// </summary>
        public string Message { get; set; }

        private readonly ChangePasswordViewModel _model;

        public ChangePasswordWindow(ChangePasswordViewModel model)
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

            if (TxtPasswordNew.Password != TxtNewPasswordRepeat.Password)
            {
                return;
            }

            DialogResult = _model.TryChangePassword(TxtPassword.Password, TxtPasswordNew.Password);
            Close();
        }
    }
}
