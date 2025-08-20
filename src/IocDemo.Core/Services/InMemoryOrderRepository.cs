using IocDemo.Core.Contracts;
using Microsoft.Extensions.Logging;

namespace IocDemo.Core.Services;

/// <summary>
/// In-memory order repository implementation
/// Demonstrates simple storage implementation for demo purposes
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<string> _orders = new();
    private readonly ILogger<InMemoryOrderRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the InMemoryOrderRepository class
    /// </summary>
    /// <param name="logger">Logger instance for logging operations</param>
    public InMemoryOrderRepository(ILogger<InMemoryOrderRepository> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets the total count of orders
    /// </summary>
    public int OrderCount => _orders.Count;

    /// <summary>
    /// Saves an order to in-memory storage
    /// </summary>
    /// <param name="orderId">The order identifier</param>
    /// <returns>Result of the save operation</returns>
    public string Save(string orderId)
    {
        _logger.LogInformation("Saving order: {OrderId}", orderId);
        
        _orders.Add(orderId);
        var result = $"💾 Order {orderId} saved to repository";
        
        _logger.LogInformation("Order saved successfully. Total orders: {Count}", _orders.Count);
        return result;
    }

    /// <summary>
    /// Retrieves all stored orders
    /// </summary>
    /// <returns>Array of order identifiers</returns>
    public string[] GetAllOrders()
    {
        _logger.LogDebug("Retrieving all orders. Count: {Count}", _orders.Count);
        return _orders.ToArray();
    }
}
