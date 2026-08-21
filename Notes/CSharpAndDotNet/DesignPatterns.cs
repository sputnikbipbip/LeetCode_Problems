    using System.Security.Cryptography.X509Certificates;

    public class Singleton
    {
        private static Singleton _instance;

        private static readonly object _lock = new object(); // For thread safety

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                lock (_lock) // Ensure that only one thread can access this block at a time
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

    // create a moddern c# singleton using Lazy<T>
    /*
        Lazy<T> (from .NET) is a helper class that:
        Delays the creation of an object until it is actually needed, and does it in a thread-safe way.
    */
    public class LazySingleton
    {
        // The Lazy<T> class ensures that the instance is created in a thread-safe manner and only when it is first accessed.
        private static readonly Lazy<LazySingleton> _instance = new Lazy<LazySingleton>(
            () => new LazySingleton() // This lambda expression is the factory method that creates the instance when needed.
        );
    
        private LazySingleton() { } // Private constructor to prevent instantiation from outside the class
        public static LazySingleton Instance => _instance.Value; // Initialize the singleton instance and return it when accessed
    }

    /*
        Does entity framework core use the singleton pattern?
        No, DbContext in Entity Framework Core is registered as scoped by default because it’s not thread-safe and maintains state like change tracking. 
        Singleton would cause concurrency issues and memory problems. 
        However, EF Core does use singleton internally for stateless components like metadata caching. 

        Register DbContext as scoped in your application to ensure proper lifecycle management and avoid issues with concurrent access.
        
        services.AddDbContext<AppDbContext>();

        Add DbContext polling for better performance in high-concurrency scenarios:

        services.AddDbContextPool<AppDbContext>();
    */


    // Factory Method Design Pattern:
    // A creational design pattern that provides an interface for creating objects in a superclass, but allows subclasses to alter the type of objects that will be created. 
    // It promotes loose coupling by eliminating the need to bind application-specific classes into the code. 
    // The client code interacts with the factory method, which then delegates the instantiation to the subclasses, allowing for greater flexibility and

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

    ConcreteCreatorA creatorA = new ConcreteCreatorA();
    IProduct productA = creatorA.CreateProduct();
    Console.WriteLine(productA.GetName()); // Output: Product A.

// Adapter Design Pattern:
// A structural design pattern that allows incompatible interfaces to work together.
// It acts as a bridge between two incompatible interfaces by wrapping an existing class with a new interface. 
// The adapter translates the interface of one class into another interface that the client expects, enabling classes to work together that otherwise couldn’t due to incompatible interfaces.  

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
            _adaptee.SpecificRequest(); // Translate the request to the adaptee's specific request
        }
    }

    ITarget adapter = new Adapter(new Adaptee()); // Create an adapter instance that wraps the adaptee
    adapter.Request(); // Output: Specific request from Adaptee.

// Decorator Design Pattern:
// A structural design pattern that allows behavior to be added to individual objects, dynamically, without affecting the behavior of other objects from the same class. 
// It involves a set of decorator classes that are used to wrap concrete components. 
// The decorator class has a reference to a component and implements the same interface, allowing it to add behavior before or after delegating the call to the component.

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
            _component.Operation(); // Delegate the call to the component
        }
    }

    public class ConcreteDecoratorA : Decorator
    {
        public ConcreteDecoratorA(IComponent component) : base(component) { }

        public override void Operation()
        {
            base.Operation(); // Call the base operation
            AddedBehavior(); // Add additional behavior
        }

        private void AddedBehavior()
        {
            Console.WriteLine("Added behavior in ConcreteDecoratorA.");
        }
    }

    IComponent decoratedComponent = new ConcreteDecoratorA(new ConcreteComponent()); // Wrap the concrete component with a decorator
    decoratedComponent.Operation(); 
    // Output:
    // Operation in ConcreteComponent.
    // Added behavior in ConcreteDecoratorA.

    // Observer Design Pattern:
    // A behavioral design pattern that defines a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically. 
    // It consists of a subject (or observable) that maintains a list of observers and notifies them of any state changes, usually by calling one of their methods. 
    // This pattern is commonly used in event handling systems, where the subject is the event source and the observers are the event handlers.

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
            _observers.Add(observer); // Add an observer to the list
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer); // Remove an observer from the list
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message); // Notify all observers of a state change
            }
        }
    }

    Subject subject = new Subject();
    IObserver observer1 = new ConcreteObserver("Observer 1");
    IObserver observer2 = new ConcreteObserver("Observer 2"); 
    subject.Attach(observer1); // Attach observers to the subject
    subject.Attach(observer2);
    subject.Notify("State has changed!"); // Notify observers of a state change
    // Output:
    // Observer 1 received update: State has changed!
    // Observer 2 received update: State has changed!