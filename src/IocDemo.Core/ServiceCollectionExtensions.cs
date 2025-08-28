using IocDemo.Core.Contracts;
using IocDemo.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IocDemo.Core;

/// <summary>
/// Dependency injection configuration extension methods
/// Demonstrates how to organize and manage dependency registration
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers core application services (without message sender)
    /// Call AddEmailSender() or AddSmsSender() or AddMessageSender&lt;T&gt;() to register a message sender
    /// Note: Logging configuration should be done at the application level
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddIocDemoCore(this IServiceCollection services)
    {
        // Register services with different lifetimes
        
        // Repository - Scoped for potential future database contexts
        services.AddScoped<IOrderRepository, InMemoryOrderRepository>();
        
        // Business service - Transient for stateless operations
        services.AddTransient<OrderService>();
        
        // Dynamic message sender support
        services.AddSingleton<IMessageSenderFactory, MessageSenderFactory>();
        services.AddTransient<DynamicOrderService>();
        
        return services;
    }

    /// <summary>
    /// Registers EmailSender as the message sender implementation
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        return services.AddSingleton<IMessageSender, EmailSender>();
    }

    /// <summary>
    /// Registers SmsSender as the message sender implementation
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddSmsSender(this IServiceCollection services)
    {
        return services.AddSingleton<IMessageSender, SmsSender>();
    }

    /// <summary>
    /// Registers a custom message sender implementation
    /// </summary>
    /// <typeparam name="T">The message sender implementation type</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddMessageSender<T>(this IServiceCollection services) 
        where T : class, IMessageSender
    {
        return services.AddSingleton<IMessageSender, T>();
    }

    /// <summary>
    /// Convenience method: Registers core services with EmailSender
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddIocDemoCoreWithEmail(this IServiceCollection services)
    {
        return services.AddIocDemoCore().AddEmailSender();
    }

    /// <summary>
    /// Convenience method: Registers core services with SmsSender
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddIocDemoCoreWithSms(this IServiceCollection services)
    {
        return services.AddIocDemoCore().AddSmsSender();
    }
}
