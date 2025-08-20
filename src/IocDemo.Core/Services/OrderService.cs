using IocDemo.Core.Contracts;
using Microsoft.Extensions.Logging;

namespace IocDemo.Core.Services;

/// <summary>
/// Order processing service
/// Demonstrates constructor injection and service composition
/// </summary>
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMessageSender _messageSender;
    private readonly ILogger<OrderService> _logger;

    /// <summary>
    /// Initializes a new instance of the OrderService class
    /// Constructor injection: Dependencies are clear and immutable
    /// </summary>
    /// <param name="repository">Repository for order persistence</param>
    /// <param name="messageSender">Service for sending notifications</param>
    /// <param name="logger">Logger instance for logging operations</param>
    public OrderService(
        IOrderRepository repository, 
        IMessageSender messageSender,
        ILogger<OrderService> logger)
    {
        _repository = repository;
        _messageSender = messageSender;
        _logger = logger;
    }

    /// <summary>
    /// Places an order and sends notification
    /// </summary>
    /// <param name="orderId">The order identifier</param>
    /// <returns>Result of the order placement operation</returns>
    public string PlaceOrder(string orderId)
    {
        _logger.LogInformation("Processing order: {OrderId}", orderId);
        
        try
        {
            // 1. Save order
            var saveResult = _repository.Save(orderId);
            
            // 2. Send notification
            var notifyResult = _messageSender.Send($"Order {orderId} processed");
            
            _logger.LogInformation("Order processed successfully: {OrderId}", orderId);
            return $"{saveResult}\n{notifyResult}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process order: {OrderId}", orderId);
            return $"❌ Failed to process order {orderId}: {ex.Message}";
        }
    }

    /// <summary>
    /// Gets a summary of all orders
    /// </summary>
    /// <returns>Summary string containing order information</returns>
    public string GetOrderSummary()
    {
        _logger.LogDebug("Retrieving order summary");
        
        var orders = _repository.GetAllOrders();
        var summary = $"Currently {orders.Length} orders: [{string.Join(", ", orders)}]";
        
        _logger.LogDebug("Order summary generated: {Summary}", summary);
        return summary;
    }

    /// <summary>
    /// Gets the type of message sender being used
    /// </summary>
    /// <returns>Message sender type</returns>
    public string GetMessageSenderType()
    {
        return _messageSender.SenderType;
    }
}
