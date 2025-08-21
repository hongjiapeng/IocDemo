# 动态消息发送器切换示例

本文档展示如何在运行时动态切换消息发送器实现。

## 🎯 核心概念

通过使用工厂模式和依赖注入，您现在可以：

1. **启动时配置**：通过配置文件决定使用哪种发送器
2. **运行时切换**：在应用程序运行过程中动态切换发送器类型
3. **灵活扩展**：轻松添加新的发送器实现

## 📝 使用方式

### 方法1：配置文件方式

在 `appsettings.json` 中配置：

```json
{
  "MessageSender": {
    "Type": "Email"  // 或 "Sms"
  }
}
```

应用启动时会自动读取配置并注册相应的发送器。

### 方法2：代码中动态切换

```csharp
// 获取动态订单服务
var dynamicOrderService = serviceProvider.GetRequiredService<DynamicOrderService>();

// 切换到邮件发送器
dynamicOrderService.SwitchDefaultSender(MessageSenderType.Email);

// 处理订单（使用当前默认发送器）
var success = dynamicOrderService.ProcessOrder("ORDER-001");

// 或者指定特定的发送器类型处理订单
var smsSuccess = dynamicOrderService.ProcessOrder("ORDER-002", MessageSenderType.Sms);
```

### 方法3：WPF界面操作

在WPF应用程序中，您可以：

1. 点击 **"📧 Switch to Email"** 按钮切换到邮件发送器
2. 点击 **"📱 Switch to SMS"** 按钮切换到短信发送器
3. 点击 **"🚀 Process with Current Sender"** 使用当前选择的发送器处理订单

## 🔧 技术实现

### 工厂模式

```csharp
public interface IMessageSenderFactory
{
    IMessageSender CreateSender(MessageSenderType type);
    void SetDefaultSender(MessageSenderType type);
    IMessageSender GetDefaultSender();
}
```

### 动态服务

```csharp
public class DynamicOrderService
{
    private readonly IMessageSenderFactory _messageSenderFactory;
    
    public void SwitchDefaultSender(MessageSenderType newType)
    {
        _messageSenderFactory.SetDefaultSender(newType);
    }
    
    public bool ProcessOrder(string orderId, MessageSenderType senderType)
    {
        var sender = _messageSenderFactory.CreateSender(senderType);
        // 处理订单逻辑...
    }
}
```

## 💡 优势

### 🔄 **灵活性**
- 运行时切换实现，无需重启应用
- 支持A/B测试和渐进式部署

### 🎯 **可测试性**
- 轻松模拟不同的发送器行为
- 独立测试每种实现

### 🚀 **可扩展性**
- 添加新的发送器类型无需修改现有代码
- 遵循开闭原则

### 📊 **监控能力**
- 实时观察哪种发送器正在使用
- 记录切换历史和性能指标

## 🌟 实际应用场景

1. **紧急通知**：平时用邮件，紧急情况切换到短信
2. **成本优化**：根据时间段切换成本不同的发送渠道
3. **故障转移**：主要服务不可用时自动切换到备用服务
4. **用户偏好**：根据用户设置选择通知方式
5. **地域差异**：不同地区使用不同的消息服务提供商

## 🔧 扩展示例

添加新的发送器类型：

```csharp
// 1. 扩展枚举
public enum MessageSenderType
{
    Email,
    Sms,
    Slack,    // 新增
    Teams     // 新增
}

// 2. 在工厂中添加实现
public IMessageSender CreateSender(MessageSenderType type)
{
    return type switch
    {
        MessageSenderType.Email => new EmailSender(_loggerFactory.CreateLogger<EmailSender>()),
        MessageSenderType.Sms => new SmsSender(_loggerFactory.CreateLogger<SmsSender>()),
        MessageSenderType.Slack => new SlackSender(_loggerFactory.CreateLogger<SlackSender>()),
        MessageSenderType.Teams => new TeamsSender(_loggerFactory.CreateLogger<TeamsSender>()),
        _ => throw new ArgumentException($"Unsupported sender type: {type}")
    };
}
```

这就是现代依赖注入的威力！🎉
