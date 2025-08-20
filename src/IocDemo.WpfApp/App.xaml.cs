using IocDemo.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Windows;

namespace IocDemo.WpfApp;

/// <summary>
/// Demonstrates the use of IoC container in WPF applications
/// Shows how to integrate with Microsoft.Extensions.Hosting
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        // Create and configure Host with Serilog
        _host = Host.CreateDefaultBuilder()
            .UseSerilog() // Use Serilog as the logging provider
            .ConfigureServices((context, services) =>
            {
                // Register core services
                services.AddIocDemoCore()
                        .AddEmailSender(); // Use email sender as default
                
                // Register ViewModels
                services.AddTransient<MainViewModel>();
                
                // Register Views
                services.AddTransient<MainWindow>();
            })
            .Build();

        // Get main window from container and show it
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        Log.Information("IoC Demo WPF Application started");

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Information("IoC Demo WPF Application exiting");
        
        // Dispose Serilog
        Log.CloseAndFlush();
        
        // Release resources
        _host?.Dispose();
        base.OnExit(e);
    }
}
