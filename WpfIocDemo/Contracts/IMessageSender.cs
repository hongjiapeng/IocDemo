namespace WpfIocDemo.Contracts
{
    /// <summary>
    /// 消息发送器接口 - 演示依赖反转原则
    /// </summary>
    public interface IMessageSender
    {
        string Send(string message);
    }
}
