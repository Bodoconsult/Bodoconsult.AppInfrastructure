//using System.Collections.Generic;
//using System.Windows.Input;
//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.Models
//{

//    /// <summary>
//    /// Represents a menu item for a menubar
//    /// </summary>
//    [ImplementPropertyChanged]
//    public class BaseMenuItem
//    {
        

//        public BaseMenuItem(string header, ICommand command)
//        {
//            Header = header;
//            Command = command;
//        }

//        public BaseMenuItem()
//        {

//        }

//        public string Header { get; set; }

//        private List<BaseMenuItem> _items;
//        /// <summary>
//        /// Contains submenu items of the current menu item
//        /// </summary>
//        public List<BaseMenuItem> Items
//        {
//            get { return _items ?? (_items = new List<BaseMenuItem>()); }
//            set { _items = value; }
//        }

//        public ICommand Command { get; set; }
//        public string CommandName { get; set; }
//        public object Icon { get; set; }
//        public bool IsCheckable { get; set; }
//        public bool IsChecked { get; set; }

//        public bool Visible { get; set; }
//        public bool IsSeparator { get; set; }
//        public string InputGestureText { get; set; }
//        public string ToolTip { get; set; }
//        public int MenuHierarchyId { get; set; }
//        public int ParentMenuHierarchyId { get; set; }
//        public string IconPath { get; set; }
//        public bool IsAdminOnly { get; set; }
//        public object Context { get; set; }
//        //public BaseMenuItem Parent { get; set; }
//        public int IntSequence { get; set; }
//        public int IntKeyIndex { get; set; }
//    }
//}
