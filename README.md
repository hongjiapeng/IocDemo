# IoC (Inversion of Control) Demo in .NET

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![Serilog](https://img.shields.io/badge/Logging-Serilog-orange.svg)](https://serilog.net/)
[![xUnit](https://img.shields.io/badge/Testing-xUnit-green.svg)](https://xunit.net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A comprehensive demonstration of **Inversion of Control (IoC)** and **Dependency Injection (DI)** patterns in .NET applications, featuring a WPF demo app with full unit testing coverage.

## 🎯 Purpose

This project serves as a **tech sharing resource** for demonstrating:
- How IoC/DI improves code maintainability and testability
- Practical implementation using .NET's built-in DI container
- Integration with modern logging frameworks (Serilog)
- Best practices for project structure and testing

## 🏗️ Project Structure

```
📁 IocDemo/
├── 📁 src/
│   ├── 📁 IocDemo.Core/              # Core business logic library
│   │   ├── 📁 Contracts/             # Interfaces (IMessageSender, IOrderRepository)
│   │   ├── 📁 Services/              # Concrete implementations
│   │   └── ServiceCollectionExtensions.cs
│   └── 📁 IocDemo.WpfApp/            # WPF demonstration application
├── 📁 tests/
│   └── 📁 IocDemo.Core.Tests/        # Unit tests with xUnit
│       ├── 📁 Services/              # Service unit tests
│       └── 📁 Integration/           # Integration tests
├── 📁 doc/                           # Documentation
└── 📁 logs/                          # Application logs (auto-created)
```

## 🚀 Features

### Core IoC Concepts Demonstrated
- ✅ **Constructor Injection** - Dependencies injected through constructors
- ✅ **Dependency Inversion Principle** - High-level modules depend on abstractions
- ✅ **Service Lifetimes** - Singleton, Scoped, and Transient registrations
- ✅ **Loose Coupling** - Easy to swap implementations (Email ↔ SMS)
- ✅ **Testability** - Full unit test coverage with mocking

### Technical Features
- 🏗️ **Clean Architecture** - Separated core logic from UI
- 📝 **Comprehensive Logging** - Serilog integration with console and file outputs
- 🧪 **Unit Testing** - xUnit with Moq and FluentAssertions
- 🖥️ **WPF Demo** - Interactive application showing IoC in action
- 📊 **Integration Tests** - End-to-end DI container validation

## 🛠️ Technologies Used

- **.NET 9.0** - Latest .NET framework
- **Microsoft.Extensions.DependencyInjection** - Built-in DI container
- **Microsoft.Extensions.Hosting** - Host builder pattern
- **Serilog** - Structured logging
- **WPF** - Desktop UI framework
- **xUnit** - Testing framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library

## 🏃‍♂️ Quick Start

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 or Visual Studio Code

### Running the Demo

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/IocDemo.git
   cd IocDemo
   ```

2. **Build the solution**
   ```bash
   dotnet build
   ```

3. **Run the WPF application**
   ```bash
   cd src/IocDemo.WpfApp
   dotnet run
   ```

4. **Run the tests**
   ```bash
   dotnet test
   ```

### What You'll See

The WPF application demonstrates:
- 📦 **Order Processing** - Place orders using injected services
- 📧 **Message Sending** - Email notifications (configurable to SMS)
- 📊 **Data Persistence** - In-memory repository pattern
- 📝 **Logging** - Real-time logging to console and files

## 🔧 Configuration Options

### Switching Message Providers

In `ServiceCollectionExtensions.cs`, you can easily switch between email and SMS:

```csharp
// Use Email
services.AddIocDemoCore()
        .AddEmailSender();
// OR use convenience method
services.AddIocDemoCoreWithEmail();

// Use SMS instead
services.AddIocDemoCore()
        .AddSmsSender();
// OR use convenience method
services.AddIocDemoCoreWithSms();

// Use custom implementation
services.AddIocDemoCore()
        .AddMessageSender<MyCustomSender>();
```

### Logging Configuration

Serilog is configured to output to:
- **Console** - Colored, structured logs during development
- **File** - Rolling daily log files in `logs/` directory

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/IocDemo.Core.Tests/
```

### Test Categories

- **Unit Tests** - Test individual components in isolation
- **Integration Tests** - Test DI container configuration
- **Mocking Examples** - Demonstrate testing with fake dependencies

## 📚 Learning Resources

### Key Files to Study

1. **`src/IocDemo.Core/ServiceCollectionExtensions.cs`** - DI registration patterns
2. **`src/IocDemo.Core/Services/OrderService.cs`** - Constructor injection example
3. **`tests/IocDemo.Core.Tests/Services/OrderServiceTests.cs`** - Testing with mocks
4. **`src/IocDemo.WpfApp/App.xaml.cs`** - Host builder configuration

### Documentation

- 📖 [**Complete IoC Guide**](doc/How%20Inversion%20of%20Control%20(IoC)%20is%20Used%20in%20.NET%20Applications.md) - Comprehensive explanation with examples
- 🔗 [Microsoft DI Documentation](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- 🔗 [Serilog Documentation](https://serilog.net/)

## 🎯 Benefits Demonstrated

### Before IoC (Traditional Approach)
```csharp
public class OrderService
{
    private readonly EmailSender _emailSender = new EmailSender(); // ❌ Tight coupling
    private readonly SqlRepository _repository = new SqlRepository(); // ❌ Hard to test
}
```

### After IoC (Recommended Approach)
```csharp
public class OrderService
{
    private readonly IMessageSender _sender;     // ✅ Depends on abstraction
    private readonly IOrderRepository _repository; // ✅ Easily mockable
    
    public OrderService(IMessageSender sender, IOrderRepository repository)
    {
        _sender = sender;
        _repository = repository;
    }
}
```

### Key Improvements
- 🔄 **Easy to swap implementations** (Email → SMS)
- 🧪 **Highly testable** with mocks
- 📈 **Scalable architecture** for larger applications
- 🔧 **Configuration-driven** behavior changes

## 🤝 Contributing

This project is designed for educational purposes. Feel free to:
- 🐛 Report issues or bugs
- 💡 Suggest improvements
- 🍴 Fork and create your own examples
- 📝 Improve documentation

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

Created for tech sharing and educational purposes. 

**Happy Learning! 🎓**

---

### 🏷️ Tags
`#dotnet` `#csharp` `#ioc` `#dependency-injection` `#clean-architecture` `#unit-testing` `#wpf` `#serilog` `#xunit` `#best-practices`
