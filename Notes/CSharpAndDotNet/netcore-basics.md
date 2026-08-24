# .NET Core Basics: Middleware, IoC & DI

This note covers core ASP.NET Core concepts: **middleware**, **Inversion of Control (IoC)**,
and **Dependency Injection (DI)**.

## Middleware

Middleware is software assembled into an application pipeline to handle requests and
responses. Each middleware component can perform work **before** and **after** the next one
in the pipeline.

```csharp
public class CustomMiddleWare
{
    private readonly RequestDelegate _next;

    public CustomMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Do something before the next middleware
        Console.WriteLine("Before next middleware");

        await _next(context); // call the next middleware

        // Do something after the next middleware
        Console.WriteLine("After next middleware");
    }
}
```

### Registering Middleware

```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseMiddleware<CustomMiddleWare>(); // add the custom middleware to the pipeline
    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("Hello from the terminal middleware!");
    });
}
```

The pipeline processes a request in registration order and unwinds back in reverse order.

## Inversion of Control (IoC)

**Inversion of Control** is a design principle where the control of object creation and
dependency management is inverted from the class itself to an external entity — often a
framework or container. This promotes **loose coupling** and enhances **testability**.

```csharp
public interface IMessageService
{
    void GetMessage(string message);
}
```

### Dependency Injection (DI)

Dependency Injection is the most common way to implement IoC. Dependencies are provided to a
class rather than created internally — typically through the **constructor**.

```csharp
public class MessageService : IMessageService
{
    public void GetMessage(string message)
    {
        Console.WriteLine(message);
    }
}

public class HomeController : Controller
{
    private readonly IMessageService _messageService;

    public HomeController(IMessageService messageService) // constructor injection
    {
        _messageService = messageService;
    }

    public IActionResult Index()
    {
        _messageService.GetMessage("Hello from MessageService!");
        return Content("Done");
    }
}
```

### Registering a Service with the IoC Container

Services are registered in `ConfigureServices`, and the container constructs them when needed.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register the service with the IoC container
    services.AddScoped<IMessageService, MessageService>();
}
```

### Service Lifetimes

| Lifetime | Behavior |
| --- | --- |
| `AddTransient` | A new instance is created **every** time it's requested. |
| `AddScoped` | One instance per request/scope. |
| `AddSingleton` | One instance for the entire application lifetime. |

## Key Takeaways

- **Middleware** builds a request pipeline with before/after hooks.
- **IoC** inverts dependency control to a container, promoting loose coupling.
- **DI** supplies dependencies (typically via constructor), improving testability.
- Choose the right **lifetime** (`Transient`/`Scoped`/`Singleton`) for each service.
