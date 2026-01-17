//using Bodoconsult.App.Wpf.ReactiveUI.Views;


//namespace Bodoconsult.App.Wpf.ReactiveUI.Models
//{
//    /// <summary>
//    /// Data to use for a <see cref="InputBox"/>
//    /// </summary>
//    /// <code> 
//    /// 
//    ///         var data = new InputBoxData();
//    /// 
//    ///         new InputBox(data).ShowDialog();
//    /// 
//    ///         if (string.IsNullOrEmpty(data.UserInput))
//    ///         {
//    ///             WpfStandardDialogUtility.ShowInfo("InputBox", "Cancelled");
//    ///         }
//    ///         else
//    ///         {
//    ///             WpfStandardDialogUtility.ShowInfo("InputBox", "User input"+data.UserInput);
//    ///         }
//    /// 
//    /// </code>

//    public class InputBoxData
//    {
//        /// <summary>
//        /// Default ctor
//        /// </summary>
//        public InputBoxData()
//        {
//            OkButtonText = translateDelegate.Invoke("Wpf.Base.InputBoxOkButtonText");
//            EscButtonText = translateDelegate.Invoke("Wpf.Base.InputBoxEscButtonText");
//            WindowTitle = translateDelegate.Invoke("Wpf.Base.InputBoxWindowTitle");
//            UserInstruction = translateDelegate.Invoke("Wpf.Base.InputBoxUserInstruction");
//        }

//        /// <summary>
//        /// Text for the o.k.-button
//        /// </summary>
//        public string OkButtonText { get; set; }

//        /// <summary>
//        /// Text for the escape button
//        /// </summary>
//        public string EscButtonText { get; set; }

//        /// <summary>
//        /// Window title to show
//        /// </summary>
//        public string WindowTitle { get; set; }

//        /// <summary>
//        /// Instruction text for the use. Should explain what to insert in the inputbox
//        /// </summary>
//        public string UserInstruction { get; set; }

//        /// <summary>
//        /// User input. May be checked after dialog is closed. Use String.IsNullOrEmpty to check if dialog was escpaed by the user.
//        /// </summary>
//        public string UserInput { get; set; }

//    }
//}
