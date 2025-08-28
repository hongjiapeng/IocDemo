using IocDemo.Core;
using IocDemo.Core.Services;
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
        // Configure Serilog at application level
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/iocdemo-.txt", 
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        // Create and configure Host with Serilog
        _host = Host.CreateDefaultBuilder()
            .UseSerilog() // Use Serilog as the logging provider
            .ConfigureServices((context, services) =>
            {
                // Register core services (without logging configuration)
                services.AddIocDemoCore()
                  .AddSmsSender()    // Use SMS sender as default
                  .AddEmailSender(); // Also add EmailSender

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
