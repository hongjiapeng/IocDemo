using WpfIocDemo.Contracts;

namespace WpfIocDemo.Services
{
    /// <summary>
    /// 内存订单仓储实现
    /// </summary>
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<string> _orders = new();

        public string Save(string orderId)
        {
            _orders.Add(orderId);
            return $"📦 订单 {orderId} 已保存到内存 (共 {_orders.Count} 个订单)";
        }

        public string[] GetAllOrders()
        {
            return _orders.ToArray();
        }
    }
}
