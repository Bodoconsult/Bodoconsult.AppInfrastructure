//using Bodoconsult.App.Wpf.ReactiveUI.Models;
//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{
//    [ImplementPropertyChanged]
//    public class ChangePasswordViewModel
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



//        public ChangePasswordData ChangePasswordData { get; set; }


//        public bool TryChangePassword(string passwordOld, string passwordNew)
//        {
//            _tryCounter++;

//            if (_tryCounter > 2) return false;

//            var successful = ChangePasswordData.CheckChangePasswordData(passwordOld, passwordNew);

//            return successful;
//        }
//    }
//}