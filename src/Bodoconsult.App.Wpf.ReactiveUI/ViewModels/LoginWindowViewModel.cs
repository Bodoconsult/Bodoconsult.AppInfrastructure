//using Bodoconsult.App.Wpf.ReactiveUI.Models;
//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{
//    [ImplementPropertyChanged]
//    public class LoginWindowViewModel
//    {

//        private int _tryCounter;



//        ///// <summary>
//        ///// Load login data to initialize the <see cref="LoginWindow"/>.
//        ///// </summary>
//        ///// <param name="data"></param>
//        //private void LoadData(LoginData data)
//        //{
//        //    LoginData = data;
//        //}



//        public LoginData LoginData { get; set; }


//        public bool TryLogin(string userName, string password)
//        {
//            _tryCounter++;

//            if (_tryCounter > 2) return false;

//            var logonSuccessful = LoginData.CheckLoginData(userName, password);

//            return logonSuccessful;
//        }
//    }
//}
