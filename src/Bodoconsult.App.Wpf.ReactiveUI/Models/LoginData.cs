//using System.Windows.Media;
//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.Models
//{
//    /// <summary>
//    /// Contains all data needed to fill and handle a LoginWindow
//    /// </summary>
//    [ImplementPropertyChanged]
//    public class LoginData
//    {

//        public LoginData()
//        {
//            TitleLabel = translateDelegate.Invoke("Wpf.Base.LoginDialogTitle");
//            UserNameLabel = translateDelegate.Invoke("Wpf.Base.UsernameLabelText");
//            PasswordLabel = translateDelegate.Invoke("Wpf.Base.PasswordLabelText");
//            PasswordTooltip = translateDelegate.Invoke("Wpf.Base.PasswordTooltipText");
//            UserNameTooltip = translateDelegate.Invoke("Wpf.Base.UsernameTooltipText");
//            CancelButtonLabel = translateDelegate.Invoke("Wpf.Base.CancelButtonText");
//            LoginButtonLabel = translateDelegate.Invoke("Wpf.Base.LoginButtonText");
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
