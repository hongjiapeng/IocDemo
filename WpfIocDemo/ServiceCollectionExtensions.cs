using Microsoft.Extensions.DependencyInjection;
using WpfIocDemo.Contracts;
using WpfIocDemo.Services;
using WpfIocDemo.ViewModels;

namespace WpfIocDemo
{
    /// <summary>
    /// 依赖注入配置扩展方法
    /// 演示如何组织和管理依赖注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 注册不同生命周期的服务
            
            // Singleton: 应用程序生命周期内单实例
            // 这里注册 EmailSender 为主要的消息发送器
            services.AddSingleton<IMessageSender, EmailSender>();
            
            // 如果要切换到短信发送器，只需修改这一行：
            // services.AddSingleton<IMessageSender, SmsSender>();
            
            // Scoped: 每个作用域一个实例（在 WPF 中类似于 Transient）
            services.AddScoped<IOrderRepository, InMemoryOrderRepository>();
            
            // Transient: 每次请求都创建新实例
            services.AddTransient<OrderService>();
            
            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            // 注册 ViewModels
            services.AddTransient<MainViewModel>();
            
            return services;
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            // 注册 Views (Windows)
            services.AddTransient<MainWindow>();
            
            return services;
        }
    }
}
