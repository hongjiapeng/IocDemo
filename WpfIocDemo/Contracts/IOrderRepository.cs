namespace WpfIocDemo.Contracts
{
    /// <summary>
    /// 订单仓储接口
    /// </summary>
    public interface IOrderRepository
    {
        string Save(string orderId);
        string[] GetAllOrders();
    }
}
