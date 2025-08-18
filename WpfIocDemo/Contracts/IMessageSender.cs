namespace WpfIocDemo.Contracts
{
    /// <summary>
    /// Message sender interface - Demonstrates dependency inversion principle
    /// </summary>
    public interface IMessageSender
    {
        string Send(string message);
    }
}
