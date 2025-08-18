using WpfIocDemo.Contracts;

namespace WpfIocDemo.Services
{
    /// <summary>
    /// Email sender implementation
    /// </summary>
    public class EmailSender : IMessageSender
    {
        public string Send(string message)
        {
            return $"✉️ Email sent: {message} (via EmailSender)";
        }
    }
}
