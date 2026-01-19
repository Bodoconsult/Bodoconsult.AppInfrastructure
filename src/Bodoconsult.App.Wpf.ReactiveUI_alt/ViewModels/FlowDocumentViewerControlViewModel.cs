//using System.IO;
//using System.Windows;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using PropertyChanged;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels
//{
//    [ImplementPropertyChanged]
//    public class FlowDocumentViewerControlViewModel: ViewModelBase
//    {
//        //private readonly IViewManager _viewManager;

//        // IViewManager viewManager
//        public FlowDocumentViewerControlViewModel(IEventAggregator eventAggregator)
//        {
//            //_viewManager = viewManager;
//            var eventAggregator1 = eventAggregator;

//            eventAggregator1.GetEvent<FlowDocumentViewerControlPrintEvent>().Subscribe(PrintDocument);
//            eventAggregator1.GetEvent<FlowDocumentViewerControlLoadEvent>().Subscribe(LoadDocument);

//            Background = Brushes.White;
//        }


//        public override void OnNavigatedTo(NavigationContext navigationContext)
//        {

//            var data = (FlowDocument)navigationContext.Parameters["Data"];
//            base.OnNavigatedTo(navigationContext);

//            LoadDocument(data);
//        }


//        private void LoadDocument(FlowDocument document)
//        {

//            var textRange2 = new TextRange(FlowDocument.ContentStart, FlowDocument.ContentEnd);
//            var range = new TextRange(document.ContentStart, document.ContentEnd);
//            var stream = new MemoryStream();
//            System.Windows.Markup.XamlWriter.Save(range, stream);
//            range.Save(stream, DataFormats.XamlPackage);
//            textRange2.Load(stream, DataFormats.XamlPackage);

//        }


//        public FlowDocument FlowDocument { get; set; }

//        /// <summary>
//        /// Background for the viewer
//        /// </summary>
//        public Brush Background { get; set; }


//        /// <summary>
//        /// Print Command
//        /// </summary>
//        private ICommand _printDocumentCommand;

//        public ICommand PrintDocumentCommand
//        {
//            get
//            {
//                return _printDocumentCommand ??
//                       (_printDocumentCommand = new DelegateCommand(PrintDocument, () => true));
//            }
//        }


//        /// <summary>
//        /// Print the document to the printer
//        /// </summary>
//        private void PrintDocument()
//        {
//            WpfDocumentUtility.PrintFlowDocument(FlowDocument);
//        }


//        /// <summary>
//        /// Print the document to the printer
//        /// </summary>
//        private void PrintDocument(string notUsed)
//        {
//            WpfDocumentUtility.PrintFlowDocument(FlowDocument);
//        }
//    }
//}
