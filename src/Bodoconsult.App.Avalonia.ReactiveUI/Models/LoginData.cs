//using System.Windows.Media;
//using PropertyChanged;

//namespace Bodoconsult.App.Avalonia.ReactiveUI.Models
//{
//    /// <summary>
//    /// Contains all data needed to fill and handle a LoginWindow
//    /// </summary>
//    [ImplementPropertyChanged]
//    public class LoginData
//    {

//        public LoginData()
//        {
//            TitleLabel = translateDelegate.Invoke("Avalonia.Base.LoginDialogTitle");
//            UserNameLabel = translateDelegate.Invoke("Avalonia.Base.UsernameLabelText");
//            PasswordLabel = translateDelegate.Invoke("Avalonia.Base.PasswordLabelText");
//            PasswordTooltip = translateDelegate.Invoke("Avalonia.Base.PasswordTooltipText");
//            UserNameTooltip = translateDelegate.Invoke("Avalonia.Base.UsernameTooltipText");
//            CancelButtonLabel = translateDelegate.Invoke("Avalonia.Base.CancelButtonText");
//            LoginButtonLabel = translateDelegate.Invoke("Avalonia.Base.LoginButtonText");
//            Background = ResourceFinder.FindResource<Brush>("HighlightBrush");
//        }


//        #region Tooltip properties

//        public string PasswordTooltip { get; set; }
//        public string UserNameTooltip { get; set; }

//        #endregion

//        #region Label properties

//        public string TitleLabel { get; set; }
//        public string PasswordLabel { get; set; }
//        public string UserNameLabel { get; set; }
//        public string CancelButtonLabel { get; set; }
//        public string LoginButtonLabel { get; set; }

//        #endregion

//        #region Data properties

//        public string UserName { get; set; }

//        #endregion


//        #region Delegate variables

//        public LoginCheckLoginData CheckLoginData;

//        #endregion

//        #region Layout properties

//        public Brush Background { get; set; }

//        #endregion

//    }
//}
