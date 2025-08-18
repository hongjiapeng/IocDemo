# WPF IoC 演示程序

这是一个演示如何在 WPF 应用程序中使用控制反转 (Inversion of Control, IoC) 和依赖注入 (Dependency Injection, DI) 的示例项目。

## 📁 项目结构

```
WpfIocDemo/
├── Contracts/              # 接口定义 (抽象层)
│   ├── IMessageSender.cs   # 消息发送器接口
│   └── IOrderRepository.cs # 订单仓储接口
├── Services/               # 具体实现
│   ├── EmailSender.cs      # 邮件发送器实现
│   ├── SmsSender.cs        # 短信发送器实现
│   ├── InMemoryOrderRepository.cs # 内存订单仓储
│   └── OrderService.cs     # 订单服务 (业务逻辑)
├── ViewModels/             # 视图模型
│   └── MainViewModel.cs    # 主窗口视图模型
├── App.xaml(.cs)          # 应用程序入口 + IoC 配置
├── MainWindow.xaml(.cs)   # 主窗口
└── ServiceCollectionExtensions.cs # 依赖注入配置扩展
```

## 🎯 IoC 演示要点

### 1. 依赖反转原则 (DIP)
- `OrderService` 不直接依赖具体的 `EmailSender` 或 `InMemoryOrderRepository`
- 而是依赖抽象接口 `IMessageSender` 和 `IOrderRepository`
- 具体实现由 IoC 容器在运行时注入

### 2. 构造函数注入
```csharp
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMessageSender _messageSender;

    // 依赖通过构造函数注入，关系清晰
    public OrderService(IOrderRepository repository, IMessageSender messageSender)
    {
        _repository = repository;
        _messageSender = messageSender;
    }
}
```

### 3. 服务生命周期管理
- **Singleton**: `IMessageSender` - 全局共享
- **Scoped**: `IOrderRepository` - 作用域内共享
- **Transient**: `OrderService`, `MainViewModel` - 每次请求新实例

### 4. 配置集中化
在 `ServiceCollectionExtensions.cs` 中统一配置所有依赖关系：
```csharp
services.AddSingleton<IMessageSender, EmailSender>();
services.AddScoped<IOrderRepository, InMemoryOrderRepository>();
services.AddTransient<OrderService>();
```

## 🔄 轻松切换实现

要从邮件通知切换到短信通知，只需修改一行代码：
```csharp
// 从这个：
services.AddSingleton<IMessageSender, EmailSender>();

// 改为这个：
services.AddSingleton<IMessageSender, SmsSender>();
```

无需修改任何业务逻辑代码！

## 🚀 运行程序

1. 确保已安装 .NET 8 或更高版本
2. 在项目目录下执行：
   ```bash
   dotnet run
   ```
3. 在界面中尝试创建订单，观察 IoC 的效果

## 📚 学习收益

通过这个示例，您将了解到：

- ✅ 如何在 WPF 中集成 Microsoft.Extensions.DependencyInjection
- ✅ 依赖反转原则的实际应用
- ✅ 构造函数注入的最佳实践
- ✅ 服务生命周期的选择策略
- ✅ 如何组织和管理依赖注册
- ✅ IoC 如何提高代码的可测试性和可维护性

## 🔧 扩展建议

1. **添加配置系统**: 使用 `IConfiguration` 来配置不同环境的服务
2. **添加日志**: 注入 `ILogger` 来记录应用程序行为
3. **添加单元测试**: 使用 Mock 框架测试各个组件
4. **添加更多实现**: 如 `DatabaseOrderRepository`、`WeChatSender` 等

---

这个演示项目展示了现代 .NET 应用程序中 IoC 的最佳实践，是学习和理解依赖注入的绝佳起点！
