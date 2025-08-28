using IocDemo.Core.Services;
using IocDemo.Core.Contracts;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IocDemo.WpfApp;

/// <summary>
/// Main view model for the WPF application
/// Demonstrates dependency injection in the presentation layer
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    private readonly OrderService _orderService;
    private readonly EmailSender _emailSender;
    private readonly SmsSender _smsSender;
    private readonly ILogger<MainViewModel> _logger;
    private string _orderIdInput = "ORDER-001";
    private string _output = "Welcome to IoC Demo! 🎉\n\nReady to process orders...\n\n";
    private string _messageSenderType = "";
    private string _currentSenderType = "Email";

    /// <summary>
    /// Initializes a new instance of the MainViewModel class
    /// </summary>
    /// <param name="orderService">Order service (injected by DI container)</param>
    /// <param name="emailSender">Email sender (injected by DI container)</param>
    /// <param name="smsSender">SMS sender (injected by DI container)</param>
    /// <param name="logger">Logger instance for logging operations</param>
    public MainViewModel(OrderService orderService, EmailSender emailSender, SmsSender smsSender, ILogger<MainViewModel> logger)
    {
        _orderService = orderService;
        _emailSender = emailSender;
        _smsSender = smsSender;
        _logger = logger;
        
        // Initialize commands
        PlaceOrderCommand = new RelayCommand(PlaceOrder, CanPlaceOrder);
        ShowOrdersCommand = new RelayCommand(ShowOrders);
        ClearOutputCommand = new RelayCommand(ClearOutput);
        SwitchToEmailCommand = new RelayCommand(SwitchToEmail);
        SwitchToSmsCommand = new RelayCommand(SwitchToSms);
        ProcessOrderWithEmailCommand = new RelayCommand(ProcessOrderWithEmail, CanPlaceOrder);
        ProcessOrderWithSmsCommand = new RelayCommand(ProcessOrderWithSms, CanPlaceOrder);
        
        // Get message sender type
        _messageSenderType = _orderService.GetMessageSenderType();
        
        _logger.LogInformation("MainViewModel initialized with {MessageSenderType} message sender", _messageSenderType);
    }

    public string OrderIdInput
    {
        get => _orderIdInput;
        set
        {
            _orderIdInput = value;
            OnPropertyChanged();
            ((RelayCommand)PlaceOrderCommand).RaiseCanExecuteChanged();
        }
    }

    public string Output
    {
        get => _output;
        set
        {
            _output = value;
            OnPropertyChanged();
        }
    }

    public string MessageSenderType
    {
        get => _messageSenderType;
        set
        {
            _messageSenderType = value;
            OnPropertyChanged();
        }
    }

    public ICommand PlaceOrderCommand { get; }
    public ICommand ShowOrdersCommand { get; }
    public ICommand ClearOutputCommand { get; }
    public ICommand SwitchToEmailCommand { get; }
    public ICommand SwitchToSmsCommand { get; }
    public ICommand ProcessOrderWithEmailCommand { get; }
    public ICommand ProcessOrderWithSmsCommand { get; }

    public string CurrentSenderType => _currentSenderType;

    private void PlaceOrder()
    {
        if (string.IsNullOrWhiteSpace(OrderIdInput)) return;

        _logger.LogInformation("Placing order via UI: {OrderId}", OrderIdInput);
        
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        AppendOutput($"[{timestamp}] Processing order: {OrderIdInput}");
        
        try
        {
            var result = _orderService.PlaceOrder(OrderIdInput);
            AppendOutput(result);
            AppendOutput("✅ Order completed successfully!\n");
            
            // Auto-increment order ID for convenience
            if (OrderIdInput.StartsWith("ORDER-") && int.TryParse(OrderIdInput.Substring(6), out int orderNumber))
            {
                OrderIdInput = $"ORDER-{orderNumber + 1:D3}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error placing order via UI: {OrderId}", OrderIdInput);
            AppendOutput($"❌ Error: {ex.Message}\n");
        }
    }

    private bool CanPlaceOrder()
    {
        return !string.IsNullOrWhiteSpace(OrderIdInput);
    }

    private void ShowOrders()
    {
        try
        {
            _logger.LogDebug("User requested order summary via UI");
            
            AppendOutput("📊 Retrieving order summary...");
            
            var summary = _orderService.GetOrderSummary();
            AppendOutput($"� {summary}");
        }
        catch (Exception ex)
        {
            var errorMessage = $"❌ Error retrieving orders: {ex.Message}";
            AppendOutput(errorMessage);
            _logger.LogError(ex, "Error in ShowOrders");
        }
    }

    private void ClearOutput()
    {
        _logger.LogDebug("Clearing output via UI");
        Output = "Output cleared! 🧹\n\n";
    }

    private void SwitchToEmail()
    {
        _currentSenderType = "Email";
        AppendOutput($"🔄 Current display switched to Email sender");
        OnPropertyChanged(nameof(CurrentSenderType));
        _logger.LogInformation("User switched display to Email sender");
    }

    private void SwitchToSms()
    {
        _currentSenderType = "SMS";
        AppendOutput($"🔄 Current display switched to SMS sender");
        OnPropertyChanged(nameof(CurrentSenderType));
        _logger.LogInformation("User switched display to SMS sender");
    }

    private void ProcessOrderWithEmail()
    {
        try
        {
            var orderId = OrderIdInput;
            
            AppendOutput($"� Processing order {orderId} with Email sender...");
            
            // 直接使用Email发送器处理订单
            var result = ProcessOrderWithSpecificSender(orderId, _emailSender);
            
            if (!string.IsNullOrEmpty(result))
            {
                AppendOutput($"✅ Order {orderId} processed successfully with Email!");
                AppendOutput($"📊 {result}");
            }
            else
            {
                AppendOutput($"❌ Failed to process order {orderId} with Email");
            }
        }
        catch (Exception ex)
        {
            var errorMessage = $"❌ Error processing order with Email: {ex.Message}";
            AppendOutput(errorMessage);
            _logger.LogError(ex, "Error in ProcessOrderWithEmail");
        }
    }

    private void ProcessOrderWithSms()
    {
        try
        {
            var orderId = OrderIdInput;
            
            AppendOutput($"📱 Processing order {orderId} with SMS sender...");
            
            // 直接使用SMS发送器处理订单
            var result = ProcessOrderWithSpecificSender(orderId, _smsSender);
            
            if (!string.IsNullOrEmpty(result))
            {
                AppendOutput($"✅ Order {orderId} processed successfully with SMS!");
                AppendOutput($"📊 {result}");
            }
            else
            {
                AppendOutput($"❌ Failed to process order {orderId} with SMS");
            }
        }
        catch (Exception ex)
        {
            var errorMessage = $"❌ Error processing order with SMS: {ex.Message}";
            AppendOutput(errorMessage);
            _logger.LogError(ex, "Error in ProcessOrderWithSms");
        }
    }

    private string ProcessOrderWithSpecificSender(string orderId, IMessageSender sender)
    {
        // 模拟订单处理流程
        _logger.LogInformation("Processing order: {OrderId} with {SenderType}", orderId, sender.SenderType);
        
        // 发送消息
        var message = $"Order {orderId} processed";
        var sendResult = sender.Send(message);
        
        _logger.LogInformation("Order processed successfully: {OrderId} using {SenderType}", 
            orderId, sender.GetType().Name);
            
        return $"Message sent via {sender.SenderType}: {sendResult}";
    }

    private void AppendOutput(string message)
    {
        Output += message + "\n";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
