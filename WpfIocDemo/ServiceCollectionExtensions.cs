using Microsoft.Extensions.DependencyInjection;
using WpfIocDemo.Contracts;
using WpfIocDemo.Services;
using WpfIocDemo.ViewModels;

namespace WpfIocDemo
{
    /// <summary>
    /// Dependency injection configuration extension methods
    /// Demonstrates how to organize and manage dependency registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register services with different lifetimes
            
            // Singleton: Single instance throughout application lifetime
            // Register EmailSender as the primary message sender here
            services.AddSingleton<IMessageSender, EmailSender>();
            
            // To switch to SMS sender, just modify this line:
            // services.AddSingleton<IMessageSender, SmsSender>();
            
            // Scoped: One instance per scope (similar to Transient in WPF)
            services.AddScoped<IOrderRepository, InMemoryOrderRepository>();
            
            // Transient: New instance created every time
            services.AddTransient<OrderService>();
            
            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            // Register ViewModels
            services.AddTransient<MainViewModel>();
            
            return services;
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            // Register Views (Windows)
            services.AddTransient<MainWindow>();
            
            return services;
        }
    }
}
