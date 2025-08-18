using WpfIocDemo.Contracts;

namespace WpfIocDemo.Services
{
    /// <summary>
    /// Order service - Demonstrates constructor injection
    /// </summary>
    public class OrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMessageSender _messageSender;

        // Constructor injection: Dependencies are clear and immutable
        public OrderService(IOrderRepository repository, IMessageSender messageSender)
        {
            _repository = repository;
            _messageSender = messageSender;
        }

        public string PlaceOrder(string orderId)
        {
            // 1. Save order
            var saveResult = _repository.Save(orderId);
            
            // 2. Send notification
            var notifyResult = _messageSender.Send($"Order {orderId} processed");
            
            return $"{saveResult}\n{notifyResult}";
        }

        public string GetOrderSummary()
        {
            var orders = _repository.GetAllOrders();
            return $"Currently {orders.Length} orders: [{string.Join(", ", orders)}]";
        }
    }
}
