using WpfIocDemo.Contracts;

namespace WpfIocDemo.Services
{
    /// <summary>
    /// SMS sender implementation
    /// </summary>
    public class SmsSender : IMessageSender
    {
        public string Send(string message)
        {
            return $"📱 SMS sent: {message} (via SmsSender)";
        }
    }
}
