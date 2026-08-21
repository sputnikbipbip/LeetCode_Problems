// Delegate and Events in C#
// A delegate is a type that represents references to methods with a particular parameter list and return type.
// It allows you to encapsulate a method as an object, which can be passed around and
// invoked at a later time. Delegates are often used for implementing event handling and callback methods.
// An event is a way for a class to provide notifications to clients of that class when something of interest occurs.
// Events are based on delegates and provide a way to subscribe and unsubscribe to notifications.
using System;

delegate void PrintMessage(string message); // Delegate declaration

void PrintToConsole(string message)
{
    Console.WriteLine("Console: {message}", message);
}

// use the delegate
PrintMessage printer = PrintToConsole; // Assign method to delegate
printer("Hello, World!"); // Invoke the delegate

// LINQ (Language Integrated Query) is a powerful feature in C# that allows you to query and manipulate data in a more readable and concise way.
// It provides a consistent syntax for querying different data sources, such as collections, databases, XML, and more. 
// LINQ allows you to perform operations like filtering, sorting, grouping, and projecting.

var numbers = new[] { 1, 2, 3, 4, 5 };
var evenNumbers = numbers.Where(n => n % 2 == 0); // LINQ defined but not executed until we iterate over it
foreach (var num in evenNumbers)
{
    Console.WriteLine(num); // Output: 2, 4
}

