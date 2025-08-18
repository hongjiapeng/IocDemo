using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfIocDemo.Services;

namespace WpfIocDemo.ViewModels
{
    /// <summary>
    /// 主窗口的ViewModel - 演示在WPF中使用依赖注入
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        private string _output = "欢迎使用 IoC 演示程序！\n点击按钮查看不同的 IoC 演示效果。";
        private string _orderIdInput = "ORD001";

        public MainViewModel(OrderService orderService)
        {
            _orderService = orderService;
            
            // 初始化命令
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
                AppendOutput("❌ 订单ID不能为空");
                return;
            }

            var result = _orderService.PlaceOrder(OrderIdInput);
            AppendOutput($"\n🎯 处理订单 {OrderIdInput}:");
            AppendOutput(result);
            
            // 自动递增订单ID
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
            AppendOutput($"\n📊 订单汇总:");
            AppendOutput(summary);
        }

        private void ClearOutput()
        {
            Output = "输出已清空...";
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
    /// 简单的 RelayCommand 实现
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
