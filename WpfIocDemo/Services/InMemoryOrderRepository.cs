using WpfIocDemo.Contracts;

namespace WpfIocDemo.Services
{
    /// <summary>
    /// In-memory order repository implementation
    /// </summary>
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<string> _orders = new();

        public string Save(string orderId)
        {
            _orders.Add(orderId);
            return $"📦 Order {orderId} saved to memory (total {_orders.Count} orders)";
        }

        public string[] GetAllOrders()
        {
            return _orders.ToArray();
        }
    }
}
