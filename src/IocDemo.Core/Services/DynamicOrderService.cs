using IocDemo.Core.Contracts;
using Microsoft.Extensions.Logging;

namespace IocDemo.Core.Services;

/// <summary>
/// Enhanced order service that supports dynamic message sender switching
/// </summary>
public class DynamicOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMessageSenderFactory _messageSenderFactory;
    private readonly ILogger<DynamicOrderService> _logger;

    public DynamicOrderService(
        IOrderRepository orderRepository, 
        IMessageSenderFactory messageSenderFactory,
        ILogger<DynamicOrderService> logger)
    {
        _orderRepository = orderRepository;
        _messageSenderFactory = messageSenderFactory;
        _logger = logger;
    }

    /// <summary>
    /// Process order using the default message sender
    /// </summary>
    public bool ProcessOrder(string orderId)
    {
        var sender = _messageSenderFactory.GetDefaultSender();
        return ProcessOrderWithSender(orderId, sender);
    }

    /// <summary>
    /// Process order using a specific message sender type
    /// </summary>
    public bool ProcessOrder(string orderId, MessageSenderType senderType)
    {
        var sender = _messageSenderFactory.CreateSender(senderType);
        return ProcessOrderWithSender(orderId, sender);
    }

    /// <summary>
    /// Switch the default message sender type
    /// </summary>
    public void SwitchDefaultSender(MessageSenderType newType)
    {
        _messageSenderFactory.SetDefaultSender(newType);
        _logger.LogInformation("Default message sender switched to: {SenderType}", newType);
    }

    private bool ProcessOrderWithSender(string orderId, IMessageSender sender)
    {
        try
        {
            _logger.LogInformation("Processing order: {OrderId}", orderId);

            // Save order
            var result = _orderRepository.Save(orderId);
            if (string.IsNullOrEmpty(result))
            {
                _logger.LogError("Failed to save order: {OrderId}", orderId);
                return false;
            }

            // Send notification using the specified sender
            var message = $"Order {orderId} processed";
            sender.Send(message);

            _logger.LogInformation("Order processed successfully: {OrderId} using {SenderType}", 
                orderId, sender.GetType().Name);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing order: {OrderId}", orderId);
            return false;
        }
    }

    /// <summary>
    /// Get order summary
    /// </summary>
    public string GetOrderSummary()
    {
        _logger.LogDebug("Retrieving order summary");
        
        var orders = _orderRepository.GetAllOrders();
        var summary = $"Currently {orders.Length} orders: [{string.Join(", ", orders)}]";
        
        _logger.LogDebug("Order summary generated: {Summary}", summary);
        return summary;
    }
}
