using IocDemo.Core.Contracts;
using IocDemo.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IocDemo.Core;

/// <summary>
/// Dependency injection configuration extension methods
/// Demonstrates how to organize and manage dependency registration
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all core application services
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="useEmailSender">If true, uses EmailSender; otherwise uses SmsSender</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddIocDemoCore(this IServiceCollection services, bool useEmailSender = true)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/iocdemo-.txt", 
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        // Add Serilog to the logging pipeline
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });

        // Register services with different lifetimes
        
        // Message sender - can be easily switched
        if (useEmailSender)
        {
            services.AddSingleton<IMessageSender, EmailSender>();
        }
        else
        {
            services.AddSingleton<IMessageSender, SmsSender>();
        }
        
        // Repository - Scoped for potential future database contexts
        services.AddScoped<IOrderRepository, InMemoryOrderRepository>();
        
        // Business service - Transient for stateless operations
        services.AddTransient<OrderService>();
        
        return services;
    }

    /// <summary>
    /// Registers core services with SMS sender
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddIocDemoCoreWithSms(this IServiceCollection services)
    {
        return services.AddIocDemoCore(useEmailSender: false);
    }
}
