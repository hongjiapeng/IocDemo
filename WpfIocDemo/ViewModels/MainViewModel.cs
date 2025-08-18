using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfIocDemo.Services;

namespace WpfIocDemo.ViewModels
{
    /// <summary>
    /// Main window ViewModel - Demonstrates the use of dependency injection in WPF
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        private string _output = "Welcome to IoC Demo Program!\nClick buttons to see different IoC demonstration effects.";
        private string _orderIdInput = "ORD001";

        public MainViewModel(OrderService orderService)
        {
            _orderService = orderService;
            
            // Initialize commands
            PlaceOrderCommand = new RelayCommand(PlaceOrder);
            ShowOrdersCommand = new RelayCommand(ShowOrders);
            ClearOutputCommand = new RelayCommand(ClearOutput);
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

        public string OrderIdInput
        {
            get => _orderIdInput;
            set
            {
                _orderIdInput = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlaceOrderCommand { get; }
        public ICommand ShowOrdersCommand { get; }
        public ICommand ClearOutputCommand { get; }

        private void PlaceOrder()
        {
            if (string.IsNullOrWhiteSpace(OrderIdInput))
            {
                AppendOutput("❌ Order ID cannot be empty");
                return;
            }

            var result = _orderService.PlaceOrder(OrderIdInput);
            AppendOutput($"\n🎯 Processing order {OrderIdInput}:");
            AppendOutput(result);
            
            // Auto-increment order ID
            if (OrderIdInput.StartsWith("ORD"))
            {
                var numberPart = OrderIdInput.Substring(3);
                if (int.TryParse(numberPart, out int number))
                {
                    OrderIdInput = $"ORD{(number + 1):D3}";
                }
            }
        }

        private void ShowOrders()
        {
            var summary = _orderService.GetOrderSummary();
            AppendOutput($"\n📊 Order Summary:");
            AppendOutput(summary);
        }

        private void ClearOutput()
        {
            Output = "Output cleared...";
        }

        private void AppendOutput(string text)
        {
            Output += $"\n{text}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Simple RelayCommand implementation
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute();
        }
    }
}
