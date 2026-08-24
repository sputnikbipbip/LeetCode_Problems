# SOLID Principles

SOLID is an acronym for five design principles that make object-oriented code more
maintainable, flexible, and testable. They complement the concepts in [oop.md](oop.md) and the
[design-patterns.md](design-patterns.md) notes.

## S — Single Responsibility Principle (SRP)

A class should have **one reason to change** — that is, it should do one thing and do it well.

```csharp
// ❌ Violates SRP: reporting + persistence + business logic in one class
public class Report
{
    public string Title { get; set; } = "";

    public void SaveToFile() { /* ... */ }
    public void Print() { /* ... */ }
}

// ✅ Each concern has its own class
public class Report { public string Title { get; set; } = ""; }
public class ReportRepository { public void Save(Report r) { /* ... */ } }
public class ReportPrinter { public void Print(Report r) { /* ... */ } }
```

## O — Open/Closed Principle (OCP)

Software entities should be **open for extension but closed for modification** — you add
behavior without changing existing code, typically via interfaces and polymorphism.

```csharp
// ❌ Adding a new shape means modifying the calculation logic
public double Area(object shape)
{
    if (shape is Rectangle r) return r.Width * r.Height;
    if (shape is Circle c) return Math.PI * c.Radius * c.Radius;
    throw new NotSupportedException();
}

// ✅ Extend by adding new types implementing the interface
public interface IShape { double Area(); }

public class Rectangle : IShape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Area() => Width * Height;
}

public class Circle : IShape
{
    public double Radius { get; set; }
    public double Area() => Math.PI * Radius * Radius;
}
```

## L — Liskov Substitution Principle (LSP)

Derived classes must be substitutable for their base classes **without altering correctness**.
A subtype should behave like its base type wherever the base type is expected.

```csharp
// ❌ Rectangle is not a valid substitute for Square logic
// (setting Width changes Height, violating the base contract)
public class Rectangle { public virtual int Width { get; set; } public virtual int Height { get; set; } }

// ✅ Favor composition or a design where the invariant holds
public interface IShape { int Area { get; } }
```

The rule of thumb: if a subtype breaks the expectations of the base type, the inheritance
relationship is likely wrong.

## I — Interface Segregation Principle (ISP)

Clients should not be forced to depend on interfaces they don't use. Prefer **many small,
focused interfaces** over one large, fat interface.

```csharp
// ❌ A printer that can't scan/fax must still implement these
public interface IMachine
{
    void Print();
    void Scan();
    void Fax();
}

// ✅ Split into focused interfaces
public interface IPrinter { void Print(); }
public interface IScanner { void Scan(); }
public interface IFax { void Fax(); }

public class Printer : IPrinter
{
    public void Print() { /* ... */ }
}
```

## D — Dependency Inversion Principle (DIP)

High-level modules should not depend on low-level modules; **both should depend on
abstractions**. This is the principle behind Dependency Injection (see
[netcore-basics.md](netcore-basics.md)).

```csharp
// ❌ Depends on a concrete implementation
public class NotificationService
{
    private readonly EmailSender _sender = new EmailSender(); // tight coupling
    public void Notify(string message) => _sender.Send(message);
}

// ✅ Depend on an abstraction
public interface IMessageSender { void Send(string message); }

public class NotificationService
{
    private readonly IMessageSender _sender;
    public NotificationService(IMessageSender sender) => _sender = sender; // injected
    public void Notify(string message) => _sender.Send(message);
}
```

## Summary

| Principle | Meaning |
| --- | --- |
| **S**ingle Responsibility | One class, one reason to change. |
| **O**pen/Closed | Extend behavior without modifying existing code. |
| **L**iskov Substitution | Subtypes must be substitutable for their base type. |
| **I**nterface Segregation | Prefer many small, focused interfaces. |
| **D**ependency Inversion | Depend on abstractions, not concretions. |

## Key Takeaways

- SOLID guides clean, testable, maintainable object-oriented design.
- SRP and ISP keep responsibilities focused; OCP and LSP keep types extensible and safe.
- DIP underpins Dependency Injection and loose coupling.
- Apply these pragmatically — over-applying them to trivial code adds needless complexity.
