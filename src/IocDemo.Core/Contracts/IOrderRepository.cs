namespace IocDemo.Core.Contracts;

/// <summary>
/// Contract for order repository operations
/// Demonstrates separation of concerns and testability
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Saves an order to the storage
    /// </summary>
    /// <param name="orderId">The order identifier</param>
    /// <returns>Result of the save operation</returns>
    string Save(string orderId);
    
    /// <summary>
    /// Retrieves all stored orders
    /// </summary>
    /// <returns>Array of order identifiers</returns>
    string[] GetAllOrders();
    
    /// <summary>
    /// Gets the total count of orders
    /// </summary>
    int OrderCount { get; }
}
