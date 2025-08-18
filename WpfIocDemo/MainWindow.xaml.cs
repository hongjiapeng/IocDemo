using System.Windows;
using WpfIocDemo.ViewModels;

namespace WpfIocDemo;

/// <summary>
/// Main window - Demonstrates the application of dependency injection in WPF
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        
        // Set DataContext to the injected ViewModel
        DataContext = viewModel;
    }
}