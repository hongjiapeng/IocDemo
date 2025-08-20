using IocDemo.Core.Services;
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
    private readonly ILogger<MainViewModel> _logger;
    private string _orderIdInput = "ORDER-001";
    private string _output = "Welcome to IoC Demo! 🎉\n\nReady to process orders...\n\n";
    private string _messageSenderType = "";

    /// <summary>
    /// Initializes a new instance of the MainViewModel class
    /// </summary>
    /// <param name="orderService">Order service (injected by DI container)</param>
    /// <param name="logger">Logger instance for logging operations</param>
    public MainViewModel(OrderService orderService, ILogger<MainViewModel> logger)
    {
        _orderService = orderService;
        _logger = logger;
        
        // Initialize commands
        PlaceOrderCommand = new RelayCommand(PlaceOrder, CanPlaceOrder);
        ShowOrdersCommand = new RelayCommand(ShowOrders);
        ClearOutputCommand = new RelayCommand(ClearOutput);
        
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
        _logger.LogDebug("Showing order summary via UI");
        
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        AppendOutput($"[{timestamp}] Retrieving order summary...");
        
        try
        {
            var summary = _orderService.GetOrderSummary();
            AppendOutput($"📊 {summary}\n");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order summary via UI");
            AppendOutput($"❌ Error retrieving summary: {ex.Message}\n");
        }
    }

    private void ClearOutput()
    {
        _logger.LogDebug("Clearing output via UI");
        Output = "Output cleared! 🧹\n\n";
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
