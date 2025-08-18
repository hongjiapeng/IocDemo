using WpfIocDemo.Contracts;

namespace WpfIocDemo.Services
{
    /// <summary>
    /// 订单服务 - 演示构造函数注入
    /// </summary>
    public class OrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMessageSender _messageSender;

        // 构造函数注入：依赖关系清晰，不可变
        public OrderService(IOrderRepository repository, IMessageSender messageSender)
        {
            _repository = repository;
            _messageSender = messageSender;
        }

        public string PlaceOrder(string orderId)
        {
            // 1. 保存订单
            var saveResult = _repository.Save(orderId);
            
            // 2. 发送通知
            var notifyResult = _messageSender.Send($"订单 {orderId} 处理完成");
            
            return $"{saveResult}\n{notifyResult}";
        }

        public string GetOrderSummary()
        {
            var orders = _repository.GetAllOrders();
            return $"当前共有 {orders.Length} 个订单: [{string.Join(", ", orders)}]";
        }
    }
}
