using System.Windows;
using WpfIocDemo.ViewModels;

namespace WpfIocDemo;

/// <summary>
/// 主窗口 - 演示依赖注入在 WPF 中的应用
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        
        // 设置 DataContext 为注入的 ViewModel
        DataContext = viewModel;
    }
}