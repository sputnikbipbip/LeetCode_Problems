# Object-Oriented Programming (OOP)

Object-Oriented Programming is a paradigm built around **objects** — bundles of data and the
methods that operate on that data. C# supports four core OOP principles: **encapsulation**,
**inheritance**, **polymorphism**, and **abstraction**.

## Encapsulation

Encapsulation bundles data and the methods that operate on it within a single unit (a class)
and restricts direct access to some of its components. This is achieved through **access
modifiers** (`public`, `private`, `protected`).

### Fields vs Properties

- A **field** is a data member that directly stores a value, typically declared `private`.
- A **property** is a higher-level construct that exposes a field indirectly, using `get` and
  `set` accessors to control read/write access.

```csharp
class Person
{
    private string name; // field

    public string Name   // property controlling access to the field
    {
        get { return name; }
        set { name = value; }
    }

    public Person(string name)
    {
        this.name = name;
    }
}
```

### Access Modifiers

| Modifier | Visibility |
| --- | --- |
| `public` | Accessible from anywhere. |
| `private` | Accessible only within the class. |
| `protected` | Accessible within the class and derived classes. |

```csharp
class BaseClass
{
    protected int protectedField = 1;
    private int privateField = 2;
    public int publicField = 3;
}
```

## Inheritance

Inheritance lets a derived class reuse fields and methods from a base class, promoting code
reuse and establishing a hierarchical relationship.

```csharp
class Animal
{
    public void Eat()
    {
        Console.WriteLine("Eating...");
    }
}

class Dog : Animal
{
    public void Bark()
    {
        Console.WriteLine("Barking...");
    }
}
```

`Dog` inherits `Eat()` from `Animal` and adds its own `Bark()`.

## Polymorphism

Polymorphism allows different classes to be treated as instances of the same base class, so
the same method name can behave differently depending on the actual object type.

```csharp
class Shape
{
    public virtual void Draw()
    {
        Console.WriteLine("Drawing a shape...");
    }
}

class Circle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a circle...");
    }
}

class Square : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a square...");
    }
}
```

Calling `Draw()` on a `Shape` reference dispatches to the correct override at runtime.

## Abstraction

Abstraction hides complex implementation details and exposes only the necessary features.
Abstract classes define a contract that derived classes must implement.

```csharp
abstract class Vehicle
{
    public abstract void Start();
}

class Car : Vehicle
{
    public override void Start()
    {
        Console.WriteLine("Car is starting...");
    }
}
```

## Structs vs Classes (Value vs Reference)

Structs are **value types** and classes are **reference types**. This difference affects how
assignment behaves.

```csharp
class PointClass
{
    public int X { get; set; }
    public int Y { get; set; }
}

struct PointStruct
{
    public int X { get; set; }
    public int Y { get; set; }
}
```

### Class (reference) assignment

Both variables reference the **same** object, so changes are shared.

```csharp
var pc1 = new PointClass { X = 1, Y = 2 };
var pc2 = pc1;   // pc2 references the same object as pc1
pc2.X = 10;
```

**Output:**

```text
PointClass pc1: (10, 2), PointClass pc2: (10, 2)
```

### Struct (value) assignment

Each variable gets its **own copy**, so changes are independent.

```csharp
var ps1 = new PointStruct { X = 1, Y = 2 };
var ps2 = ps1;   // ps2 gets a copy of ps1's value
ps2.X = 10;
```

**Output:**

```text
PointStruct ps1: (1, 2), PointStruct ps2: (10, 2)
```

## Key Takeaways

- **Encapsulation** — bundle data + methods, control access with modifiers.
- **Inheritance** — derive classes to reuse base behavior.
- **Polymorphism** — same method, different behavior via `virtual`/`override`.
- **Abstraction** — expose essentials, hide complexity (`abstract` classes/methods).
- **Classes** are reference types; **structs** are value types.
