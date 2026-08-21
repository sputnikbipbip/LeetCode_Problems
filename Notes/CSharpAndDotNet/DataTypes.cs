/* 
Value types store the value directly in the stack. 
They include primitive types like int, double, bool, and structs.
When you assign a value type to another variable, a copy of the value is created.
*/

int a = 1;
int b = a; // b gets a copy of a's value
b = 10;

Console.WriteLine($"a: {a}, b: {b}"); // Output: a: 1, b: 10

/*
Reference types store a reference to the value in the heap.
They include classes, arrays, and strings.
When you assign a reference type to another variable, both variables point to the same object in memory
*/

class MyClass
{
    public int Value { get; set; }
}

MyClass obj1 = new() { Value = 1 };
MyClass obj2 = obj1; // obj2 references the same object as obj1
obj2.Value = 10;   

Console.WriteLine($"obj1.Value: {obj1.Value}, obj2.Value: {obj2.Value}"); 
// Output: obj1.Value: 10, obj2.Value: 10

int? nullableInt = null; // Nullable value type
if (nullableInt.HasValue)
{
    Console.WriteLine($"Value: {nullableInt.Value}");
}
else
{        
    Console.WriteLine("No value");
}

for (int i = 0; i < 5; i++)
{
    Console.WriteLine($"Iteration {i}");
}

int[] array = new int[] { 1, 2, 3, 4, 5 };
foreach (int num in array)
{
    Console.WriteLine(num);
}

int count = 0;
while (count < 5)
{
    Console.WriteLine($"Count: {count}");
    count++;
}

try
{
    int result = 10 / 0; // This will throw a DivideByZeroException
}   
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    Console.WriteLine("Cleanup code goes here.");
}

void UpdateRef(ref int number)
{
    number += 10; // ref parameters allow you to pass a variable by reference, meaning the method can modify the original variable.
}

void UpdateOut(out int number)
{
    number = 42; // Must assign a value before exiting the method
}

int a = 5;
UpdateRef(ref a); //'a' must be initialized before passing it as a ref parameter
Console.WriteLine($"After UpdateRef: {a}"); // Output: After UpdateRef: 15

int b; // 'b' does not need to be initialized before passing it as an out parameter
UpdateOut(out b);
Console.WriteLine($"After UpdateOut: {b}"); // Output: After UpdateOut: 42

