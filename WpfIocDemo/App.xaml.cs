using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace WpfIocDemo;

/// <summary>
/// Demonstrates the use of IoC container in WPF applications
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        // Create and configure Host
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Register application services
                services.AddApplicationServices();
                
                // Register ViewModels
                services.AddViewModels();
                
                // Register Views
                services.AddViews();
            })
            .Build();

        // Get main window from container and show it
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Release resources
        _host?.Dispose();
        base.OnExit(e);
    }
}

