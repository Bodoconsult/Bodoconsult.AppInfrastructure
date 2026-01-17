using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for PrintDocument.xaml
    /// </summary>
    public partial class FlowDocumentViewerControl
    {
        public FlowDocumentViewerControl(FlowDocumentViewerControlViewModel model)
        {
            DataContext = model;
            InitializeComponent();

            model.FlowDocument = FlowDoc;
        }
    }
}
