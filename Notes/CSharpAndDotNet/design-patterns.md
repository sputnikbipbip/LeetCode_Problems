# Design Patterns

Common design patterns in C#, grouped by the classic categories: **creational**, **structural**,
and **behavioral**. Each pattern includes a brief explanation, a code example, and expected
output.

## Creational Patterns

### Singleton

The Singleton pattern ensures a class has **exactly one instance** and provides a global point
of access to it.

#### Thread-Safe Singleton (classic)

```csharp
public class Singleton
{
    private static Singleton? _instance;
    private static readonly object _lock = new object(); // for thread safety

    private Singleton() { }

    public static Singleton Instance
    {
        get
        {
            lock (_lock) // ensure only one thread can access this block at a time
            {
                if (_instance == null)
                {
                    _instance = new Singleton();
                }
                return _instance;
            }
        }
    }

    public void SomeMethod()
    {
        Console.WriteLine("Executing some method in the singleton instance.");
    }
}
```

#### Modern Singleton using `Lazy<T>`

`Lazy<T>` delays creation of the object until it is actually needed, in a thread-safe way.

```csharp
public class LazySingleton
{
    // The Lazy<T> class ensures the instance is created thread-safely and only on first access.
    private static readonly Lazy<LazySingleton> _instance = new Lazy<LazySingleton>(
        () => new LazySingleton() // factory method creates the instance when needed
    );

    private LazySingleton() { } // private constructor prevents external instantiation

    public static LazySingleton Instance => _instance.Value;
}
```

#### Does Entity Framework Core use Singleton?

**No.** `DbContext` is registered as **scoped** by default because it is not thread-safe and
maintains state such as change tracking. A Singleton `DbContext` would cause concurrency
issues and memory problems. However, EF Core does use singletons internally for stateless
components like metadata caching.

Register `DbContext` as scoped to ensure proper lifecycle management:

```csharp
services.AddDbContext<AppDbContext>();
```

Use **DbContext pooling** for better performance in high-concurrency scenarios:

```csharp
services.AddDbContextPool<AppDbContext>();
```

### Factory Method

A creational pattern that provides an interface for creating objects in a superclass, letting
subclasses alter the type of objects created. It promotes loose coupling by removing the need
to bind application-specific classes directly in code.

```csharp
public interface IProduct
{
    string GetName();
}

public class ConcreteProductA : IProduct
{
    public string GetName() => "Product A.";
}

public class ConcreteProductB : IProduct
{
    public string GetName() => "Product B.";
}

public abstract class Creator
{
    public abstract IProduct CreateProduct();
}

public class ConcreteCreatorA : Creator
{
    public override IProduct CreateProduct() => new ConcreteProductA();
}

public class ConcreteCreatorB : Creator
{
    public override IProduct CreateProduct() => new ConcreteProductB();
}
```

Usage:

```csharp
var creatorA = new ConcreteCreatorA();
IProduct productA = creatorA.CreateProduct();
Console.WriteLine(productA.GetName()); // Output: Product A.
```

**Output:**

```text
Product A.
```

## Structural Patterns

### Adapter

A structural pattern that allows incompatible interfaces to work together. It acts as a bridge
by wrapping an existing class with a new interface that the client expects.

```csharp
public interface ITarget
{
    void Request();
}

public class Adaptee
{
    public void SpecificRequest()
    {
        Console.WriteLine("Specific request from Adaptee.");
    }
}

public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;

    public Adapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }

    public void Request()
    {
        _adaptee.SpecificRequest(); // translate the request to the adaptee's specific request
    }
}
```

Usage:

```csharp
ITarget adapter = new Adapter(new Adaptee()); // wrap the adaptee
adapter.Request(); // Output: Specific request from Adaptee.
```

**Output:**

```text
Specific request from Adaptee.
```

### Decorator

A structural pattern that adds behavior to individual objects **dynamically**, without
affecting other objects of the same class. Decorators wrap a component and implement the same
interface, adding behavior before or after delegating the call.

```csharp
public interface IComponent
{
    void Operation();
}

public class ConcreteComponent : IComponent
{
    public void Operation()
    {
        Console.WriteLine("Operation in ConcreteComponent.");
    }
}

public abstract class Decorator : IComponent
{
    protected IComponent _component;

    public Decorator(IComponent component)
    {
        _component = component;
    }

    public virtual void Operation()
    {
        _component.Operation(); // delegate the call to the component
    }
}

public class ConcreteDecoratorA : Decorator
{
    public ConcreteDecoratorA(IComponent component) : base(component) { }

    public override void Operation()
    {
        base.Operation(); // call the base operation
        AddedBehavior();  // add additional behavior
    }

    private void AddedBehavior()
    {
        Console.WriteLine("Added behavior in ConcreteDecoratorA.");
    }
}
```

Usage:

```csharp
IComponent decoratedComponent = new ConcreteDecoratorA(new ConcreteComponent());
decoratedComponent.Operation();
```

**Output:**

```text
Operation in ConcreteComponent.
Added behavior in ConcreteDecoratorA.
```

## Behavioral Patterns

### Observer

A behavioral pattern that defines a **one-to-many** dependency between objects: when one
object changes state, all its dependents are notified and updated automatically. It consists of
a **subject** (observable) that maintains a list of **observers** and notifies them of state
changes. This is commonly used in event-handling systems.

```csharp
public interface IObserver
{
    void Update(string message);
}

public class ConcreteObserver : IObserver
{
    private readonly string _name;

    public ConcreteObserver(string name)
    {
        _name = name;
    }

    public void Update(string message)
    {
        Console.WriteLine($"{_name} received update: {message}");
    }
}

public class Subject
{
    private readonly List<IObserver> _observers = new List<IObserver>();

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(string message)
    {
        foreach (var observer in _observers)
        {
            observer.Update(message); // notify all observers of a state change
        }
    }
}
```

Usage:

```csharp
var subject = new Subject();
IObserver observer1 = new ConcreteObserver("Observer 1");
IObserver observer2 = new ConcreteObserver("Observer 2");

subject.Attach(observer1);
subject.Attach(observer2);
subject.Notify("State has changed!");
```

**Output:**

```text
Observer 1 received update: State has changed!
Observer 2 received update: State has changed!
```

## Pattern Summary

| Category | Pattern | Intent |
| --- | --- | --- |
| Creational | Singleton | Ensure a single instance with global access. |
| Creational | Factory Method | Defer object creation to subclasses. |
| Structural | Adapter | Make incompatible interfaces work together. |
| Structural | Decorator | Add behavior dynamically at runtime. |
| Behavioral | Observer | Notify dependents when state changes. |

## Key Takeaways

- Patterns are proven solutions to recurring design problems.
- **Creational** patterns control object creation; **structural** patterns compose objects;
  **behavioral** patterns manage communication between objects.
- Choose the simplest pattern that solves the problem — don't over-engineer.
