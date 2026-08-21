using System.Reflection.Metadata.Ecma335;

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

        await _next(context); // Call the next middleware

        // Do something after the next middleware
        Console.WriteLine("After next middleware");
    }
}

public void Configure(IApplicationBuilder app)
{
    app.UseMiddleware<CustomMiddleWare>(); // Add the custom middleware to the pipeline
    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("Hello from the terminal middleware!");
    });
} 

// Inversion of Control (IoC):
// A design principle where the control of object creation and dependency management is inverted from the class itself to an external entity, often a framework or container. This promotes loose coupling and enhances testability.

public interface IMessageService
{
    void GetMessage(string message);
}

public class MessageService : IMessageService
{
    public void GetMessage() => "Hello from EmailMessageService!";
}

public class HomeController : Controller
{
    private readonly IMessageService _messageService;

    public HomeController(IMessageService messageService) // Dependency Injection through constructor
    {
        _messageService = messageService;
    }

    public IActionResult Index()
    {
        string message = _messageService.GetMessage();
        return Content(message);
    }
}

// register the service in Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IMessageService, MessageService>(); // Register the service with the IoC container
}   