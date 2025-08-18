using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace WpfIocDemo;

/// <summary>
/// 演示在 WPF 应用程序中使用 IoC 容器
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        // 创建和配置 Host
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // 注册应用程序服务
                services.AddApplicationServices();
                
                // 注册 ViewModels
                services.AddViewModels();
                
                // 注册 Views
                services.AddViews();
            })
            .Build();

        // 从容器中获取主窗口并显示
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // 释放资源
        _host?.Dispose();
        base.OnExit(e);
    }
}

