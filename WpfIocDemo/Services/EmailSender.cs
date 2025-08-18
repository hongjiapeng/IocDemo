using WpfIocDemo.Contracts;

namespace WpfIocDemo.Services
{
    /// <summary>
    /// 邮件发送器实现
    /// </summary>
    public class EmailSender : IMessageSender
    {
        public string Send(string message)
        {
            return $"✉️ 邮件发送: {message} (通过 EmailSender)";
        }
    }
}
