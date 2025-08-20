using Microsoft.Extensions.Logging;

namespace IocDemo.WpfApp;

/// <summary>
/// Main window for the IoC demo application
/// Demonstrates view construction through dependency injection
/// </summary>
public partial class MainWindow : System.Windows.Window
{
    private readonly ILogger<MainWindow> _logger;

    /// <summary>
    /// Initializes a new instance of the MainWindow class
    /// </summary>
    /// <param name="viewModel">The main view model (injected by DI container)</param>
    /// <param name="logger">Logger instance for logging operations</param>
    public MainWindow(MainViewModel viewModel, ILogger<MainWindow> logger)
    {
        _logger = logger;
        InitializeComponent();
        
        DataContext = viewModel;
        
        _logger.LogInformation("MainWindow initialized with injected dependencies");
    }
}
