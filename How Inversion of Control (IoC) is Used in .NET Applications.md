# How Inversion of Control (IoC) is Used in .NET Applications

## 1. 引言（Introduction）

### 1.1 主题背景：为什么要讨论 IoC

#### 生活类比

> "假设你要装修房子，传统做法是你亲自去找水电工、木工、油漆工……还要盯进度。而 IoC 就像一个装修公司，你只需要告诉他们你需要什么，他们帮你安排好一切。"

#### 技术背景引入

- 软件项目越来越复杂，组件之间相互依赖。
- 如果每个类自己去创建依赖，就像自己"亲力亲为"找人做事，既费力又难维护。

### 1.2 在软件开发中的常见痛点

#### 耦合度高
- 类直接依赖具体实现，一改就牵一发而动全身。

#### 难以测试
- 不能轻易替换为 Mock 对象进行单元测试。

#### 难以维护
- 新需求或替换实现，需要改动多处代码。

**举个简单的例子**

```csharp
// 耦合高的写法
public class OrderService {
    private EmailSender _emailSender = new EmailSender(); // 直接依赖具体类
}
```

这种写法一旦要换成 SMS 通知，就得改 `OrderService` 源码。

### 1.3 目标：让大家理解 IoC 在 .NET 中的应用方式

- 让听众理解什么是 IoC。
- 明白 IoC 在 .NET 中如何落地实现。
- 学会在项目中用 IoC 解决"耦合高、难测试、难维护"的问题。

## 2. 基本概念（Fundamentals）

### 2.1 依赖反转原则（Dependency Inversion Principle, DIP）

#### 定义
高层模块不应该依赖低层模块，二者都应该依赖抽象。

#### 解释
让"稳定"的抽象层（接口/抽象类）处于中间，隔离"多变"的实现层。

#### 例子

**不符合 DIP**：

```csharp
public class OrderService {
    private EmailSender _emailSender = new EmailSender();
}
```

**符合 DIP**：

```csharp
public class OrderService {
    private readonly IMessageSender _sender;
    public OrderService(IMessageSender sender) {
        _sender = sender;
    }
}
```

这样，`OrderService` 只依赖 `IMessageSender` 这个抽象接口，至于发送邮件还是短信，由外部决定。

### 2.2 控制反转（Inversion of Control, IoC）定义

依赖反转原则是理念，而 IoC 是实现这个理念的手段。所谓 **控制反转**，就是把"依赖的创建和管理"这个控制权，从类本身交给外部容器来做。这样类只管用，不管造。

### 2.3 IoC vs 传统调用流程（调用方 vs 被调用方）

我们可以对比一下：

- **传统调用**：`OrderService` 自己 new 一个 `EmailSender`，然后调用它的方法。
- **IoC 调用**：IoC 容器先创建好 `EmailSender`，然后在创建 `OrderService` 时，把 `EmailSender` 传进去。

这样 `OrderService` 根本不知道是谁在帮它发消息，它只知道有人会帮它发。

### 2.4 IoC 的常见实现方式

IoC 可以有两种主要实现方式：

#### 1. Service Locator
- 可以理解为"全局工具箱"，需要依赖时去全局取。
- 这种方式的缺点是依赖关系是隐藏的，不在构造函数里声明，别人读代码时不容易发现。
- 所以在现代 .NET 项目里，它往往被认为是反模式。

#### 2. 依赖注入（Dependency Injection, DI）
- 最推荐的方式。
- 有三种注入方式：构造函数、属性、方法。
- 其中构造函数注入最常用，因为依赖关系清晰、不可变。
- 例如：

```csharp
public OrderService(IMessageSender sender) { ... }
```

## 3. 依赖注入（Dependency Injection）模式

### 3.1 构造函数注入（Constructor Injection）

#### 定义
通过构造函数将依赖对象传入类。

#### 特点
依赖关系显式、不可变，最常用且推荐。

#### 示例

```csharp
public class OrderService {
    private readonly IMessageSender _sender;
    public OrderService(IMessageSender sender) {
        _sender = sender;
    }
    public void Process(string order) {
        _sender.Send($"Processing order: {order}");
    }
}
```

### 3.2 属性注入（Property Injection）

#### 定义
通过公共属性设置依赖对象。

#### 特点
依赖可选，可在对象创建后再设置。

#### 示例

```csharp
public class OrderService {
    public IMessageSender Sender { get; set; }
    public void Process(string order) {
        Sender?.Send($"Processing order: {order}");
    }
}
```

### 3.3 方法注入（Method Injection）

#### 定义
通过方法参数传入依赖对象。

#### 特点
依赖只在调用时提供，通常用于临时或可选依赖。

#### 示例

```csharp
public class OrderService {
    public void Process(string order, IMessageSender sender) {
        sender.Send($"Processing order: {order}");
    }
}
```

### 3.4 优缺点对比

| 注入方式 | 优点 | 缺点 | 使用场景 |
|----------|------|------|----------|
| 构造函数注入 | 依赖明确、不可变、易测试 | 构造函数参数多时略繁琐 | 推荐主流方式，核心依赖 |
| 属性注入 | 可选依赖、灵活 | 依赖可能未初始化、测试不便 | 可选依赖、插件场景 |
| 方法注入 | 依赖仅在调用时生效 | 每次调用都需传入依赖 | 临时依赖或可选依赖 |

## 4. IoC 在 .NET 中的实现方式

### 4.1 .NET Core / .NET 6+ 内置的 IoC 容器

- .NET Core 及之后版本自带轻量级 IoC 容器（`IServiceCollection` + `IServiceProvider`）。
- 提供标准化方式注册和解析依赖。
- **优点**：无需第三方容器，生命周期管理自动，易于与 ASP.NET Core 集成。

### 4.2 注册服务（Registering Services）

| 生命周期 | 描述 | 使用场景 |
|----------|------|----------|
| **Transient** | 每次请求都创建新实例 | 短生命周期对象，如轻量工具类 |
| **Scoped** | 每个请求作用域一个实例 | Web 请求级别对象，如 DbContext |
| **Singleton** | 应用生命周期内单实例 | 全局共享对象，如配置、缓存 |

**注册示例（Program.cs）**：

```csharp
services.AddTransient<IEmailSender, EmailSender>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddSingleton<IAppConfig, AppConfig>();
```

### 4.3 解析服务（Resolving Services）

#### 自动解析
通过构造函数注入，容器自动提供依赖对象。

```csharp
public class OrderService {
    private readonly IEmailSender _emailSender;
    public OrderService(IEmailSender emailSender) {
        _emailSender = emailSender;
    }
}
```

#### 手动解析（不推荐频繁使用）

```csharp
var serviceProvider = services.BuildServiceProvider();
var emailSender = serviceProvider.GetService<IEmailSender>();
```

### 4.4 生命周期管理（Lifetime Management）

- **Transient**：每次请求新对象 → 无状态或轻量对象
- **Scoped**：同一请求中共享实例 → 数据库上下文、事务对象
- **Singleton**：整个应用共享实例 → 配置对象、缓存对象
- 容器自动管理对象的创建和销毁，减少内存泄漏和重复创建的风险

## 5. 代码示例（Code Demonstration）

### 5.1 传统写法 vs IoC 写法

#### 传统写法：直接依赖具体类

```csharp
public class OrderService
{
    private readonly SqlOrderRepository _repository;

    public OrderService()
    {
        _repository = new SqlOrderRepository(); // 直接依赖具体实现
    }

    public void PlaceOrder(string orderId)
    {
        _repository.Save(orderId);
    }
}
```

**缺点**：耦合度高，测试或替换实现很困难。

#### IoC 写法：依赖接口，通过构造函数注入

```csharp
public interface IOrderRepository
{
    void Save(string orderId);
}

public class SqlOrderRepository : IOrderRepository
{
    public void Save(string orderId) { /* 保存到数据库 */ }
}

public class OrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public void PlaceOrder(string orderId)
    {
        _repository.Save(orderId);
    }
}
```

**好处**：OrderService 不依赖具体实现，只依赖接口，可以在测试时替换为 Mock 实现。

### 5.2 使用 `IServiceCollection` 和 `IServiceProvider`

```csharp
var services = new ServiceCollection();

// 注册依赖
services.AddTransient<IOrderRepository, SqlOrderRepository>();
services.AddTransient<OrderService>();

// 生成 ServiceProvider
var provider = services.BuildServiceProvider();

// 解析依赖
var service = provider.GetRequiredService<OrderService>();
service.PlaceOrder("123");
```

- **AddTransient**：每次请求都会创建新对象
- **AddScoped**：每个请求（scope）内共享同一个对象
- **AddSingleton**：应用程序生命周期内只创建一个对象

### 5.3 ASP.NET Core 中的应用场景

#### Controller 中的依赖注入

```csharp
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("orders")]
    public IActionResult CreateOrder(string orderId)
    {
        _orderService.PlaceOrder(orderId);
        return Ok();
    }
}
```

ASP.NET Core 会自动从容器里解析 `OrderService`，不用手动写 `new`。

#### 中间件依赖注入

```csharp
public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly OrderService _orderService;

    public CustomMiddleware(RequestDelegate next, OrderService orderService)
    {
        _next = next;
        _orderService = orderService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 可以使用注入的服务
        _orderService.PlaceOrder("middleware-test");

        await _next(context);
    }
}
```

## 6. IoC 的优势（Benefits）

### 6.1 降低耦合度

- **说明**：传统写法下，类会主动创建依赖对象（new 关键字），导致模块之间紧密绑定。
- **IoC 的好处**：依赖的创建和生命周期交给容器管理，类只关心"需要什么"，而不关心"如何得到"。
- **例子**：如果 `OrderService` 依赖 `EmailService`，换成 `SmsService` 只需修改注册，而不是改 `OrderService` 内部代码。

### 6.2 提高可测试性（Mock 依赖）

- **说明**：测试时往往不希望依赖真实实现，比如调用真实数据库、真实外部接口。
- **IoC 的好处**：可以在测试时注入 **Mock 对象**，替代真实依赖。
- **例子**：单元测试中，`UserService` 依赖 `IUserRepository`，我们可以注入一个假的 `FakeUserRepository`，避免访问数据库。

### 6.3 易于扩展与维护

- **说明**：系统需求变化时，只要有接口和 IoC 容器，就可以轻松替换实现。
- **IoC 的好处**：不同模块解耦，每个实现类职责单一，后期扩展新功能不用改动太多旧代码。
- **例子**：支付系统新增 `ApplePayService`，只需在容器注册新服务，不用动原有的业务逻辑。

### 6.4 代码复用性提升

- **说明**：当服务通过接口定义，多个项目可以共享相同的实现。
- **IoC 的好处**：容器管理下，公共模块更容易被抽取出来，减少重复造轮子。
- **例子**：日志服务 `ILogger` 的实现，可以在不同项目中复用，只需注册一次。

## 7. 常见误区与注意事项（Pitfalls & Best Practices）

### 7.1 过度依赖容器（Service Locator 反模式）

#### 问题
一些开发者会在代码中直接通过 `IServiceProvider.GetService<T>()` 来获取依赖。这样就退化回了"全局工具箱"，依赖关系被隐藏，不再显式地出现在构造函数中。

#### 危害
- **可读性差**：别人看代码时，不清楚这个类到底依赖了什么。
- **可测试性差**：难以用 Mock 替换依赖。

#### 最佳实践
优先使用 **构造函数注入**，让依赖显式暴露。

### 7.2 滥用 Singleton 导致状态问题

#### 问题
有些人习惯把所有服务都注册成 Singleton，觉得省事。但是如果服务里持有"有状态对象"，可能导致线程安全或数据串扰问题。

#### 案例
如果一个 Singleton Service 内部有一个 List 来缓存用户会话信息，那么多个请求同时操作时，数据可能混乱。

#### 最佳实践
- 仅在服务无状态时使用 Singleton（如日志、配置）。
- 对于请求级别的依赖，优先使用 Scoped。

### 7.3 生命周期管理不当导致内存泄漏

#### 问题
如果一个 Scoped 或 Transient 对象被一个 Singleton 持有引用，就会导致它无法被回收 → 内存泄漏。

#### 案例
例如，一个 Singleton 服务中保存了一个 DbContext（它本应是 Scoped 的），结果整个应用生命周期里，这个 DbContext 都不会释放。

#### 最佳实践
- 遵循依赖反转的层级：长生命周期不能依赖短生命周期。
- 使用容器提供的正确生命周期注册。

### 7.4 建议的命名与结构

#### 建议
- **接口命名**：遵循 .NET 约定，使用 I 前缀（如 IUserService）。
- **分层组织**：
  - 接口定义放在 Contracts 或 Abstractions 文件夹。
  - 实现类放在 Services 文件夹。
- **注册依赖统一放在 DependencyInjection 扩展方法中**，例如：

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
```

#### 好处
- 结构清晰，团队成员容易维护。
- 避免到处散落注册代码。

## 8. 总结（Conclusion）

### 8.1 回顾 IoC 核心概念

Inversion of Control 的本质是 **反转依赖关系的控制权** ——从传统由类自己决定依赖的创建，变成由外部容器或框架负责注入。主要实现方式包括 Service Locator（已过时，常被视为反模式）和 Dependency Injection（现代主流方案）。

### 8.2 强调在团队项目中的价值

在实际开发中，IoC 可以：

- 让代码更清晰，依赖关系透明化。
- 降低耦合，便于多人协作。
- 通过 Mock/Stub 提高可测试性，让团队能更快迭代。
- 便于新成员理解和扩展已有系统。

### 8.3 未来可结合的技术

IoC 并不是孤立的，它可以和其他架构模式结合：

- **AOP（面向切面编程）**：通过拦截器或中间件，结合 IoC 容器注入切面逻辑（如日志、事务、缓存）。
- **插件化架构（Plugin Architecture）**：通过依赖注入动态加载和替换模块，实现系统的高度扩展性。
- **微服务（Microservices）**：服务之间的依赖与生命周期同样需要 IoC 的思想来管理。

👉 **总之，IoC 是现代 .NET 应用中不可或缺的核心设计思想。理解并合理使用它，能显著提升项目的 可维护性、可扩展性和团队协作效率。**

## 9. Q&A

---

**感谢阅读！** 如有任何关于 IoC 在 .NET 中应用的问题，欢迎交流讨论。
