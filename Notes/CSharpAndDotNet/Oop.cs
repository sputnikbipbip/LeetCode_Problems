// Encapsulation: 
// The concept of bundling data and methods that operate on that data within a single unit (class) and restricting access to some of the object's components.
// This is achieved through access modifiers (public, private, protected).

using System.Drawing;

class Person
{
    private string name;


/*
Properties are a higher-level construct used to access and manipulate fields indirectly. 
They provide a way to expose the internal state of an object while controlling how it is accessed and modified. 
Properties use accessor methods (getters and setters) to define the logic for getting and setting the underlying field's value.

A field is a data member of a class that directly stores a value. 
Fields are often declared with private or protected access modifiers to encapsulate the data and provide controlled access to it within the class. 
They are typically used for storing the internal state of an object.
*/

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public Person(string name)
    {
        this.name = name;
    }
}

// inheritance:
// The mechanism by which one class (derived class) can inherit properties and behaviors (fields and
// methods) from another class (base class). This promotes code reusability and establishes a natural hierarchical relationship between classes.

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

// Polymorphism:
// The ability of different classes to be treated as instances of the same class through inheritance.
// It allows methods to do different things based on the object it is acting upon, even if they share the same name.

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

// Abstraction:
// The concept of hiding the complex implementation details and showing only the necessary features of an object.
// It allows the user to interact with the object at a higher level without needing to understand the underlying complexity.   

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

// structs are value types and classes are reference types.

class PointClass
{
    public int X { get; set; } // auto-implemented property
    public int Y { get; set; }
}

class PointStruct
{
    public int X { get; set; }
    public int Y { get; set; }
}

PointClass pc1 = new() { X = 1, Y = 2 };
PointClass pc2 = pc1; // pc2 references the same object as pc1
pc2.X = 10;

Console.WriteLine($"PointClass pc1: ({pc1.X}, {pc1.Y}), PointClass pc2: ({pc2.X}, {pc2.Y})"); 
// Output: PointClass pc1: (10, 2), PointClass pc2: (10, 2)

PointStruct ps1 = new() { X = 1, Y = 2 };
PointStruct ps2 = ps1; // ps2 gets a copy of ps1's value
ps2.X = 10; 

Console.WriteLine($"PointStruct ps1: ({ps1.X}, {ps1.Y}), PointStruct ps2: ({ps2.X}, {ps2.Y})"); 
// Output: PointStruct ps1: (1, 2), PointStruct ps2: (10, 2)    

// Access modifiers: 
// public: Accessible from anywhere
// private: Accessible only within the class
// protected: Accessible within the class and by derived classes
class BaseClass
{
    protected int protectedField = 1;
    private int privateField = 2;
    public int publicField = 3;  
}

