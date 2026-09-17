using System.Windows;
using BodoFileTransferCore.Ui.ViewModels;

namespace BodoFileTransferCore.Ui;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IMainWindowViewModel _viewModel;

    public MainWindow(IMainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        DataContext = _viewModel;
    }

}