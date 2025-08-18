namespace WpfIocDemo.Contracts
{
    /// <summary>
    /// Order repository interface
    /// </summary>
    public interface IOrderRepository
    {
        string Save(string orderId);
        string[] GetAllOrders();
    }
}
