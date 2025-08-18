using WpfIocDemo.Contracts;

namespace WpfIocDemo.Services
{
    /// <summary>
    /// 短信发送器实现
    /// </summary>
    public class SmsSender : IMessageSender
    {
        public string Send(string message)
        {
            return $"📱 短信发送: {message} (通过 SmsSender)";
        }
    }
}
