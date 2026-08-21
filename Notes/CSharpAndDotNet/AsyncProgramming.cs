// Example of an asynchronous method in C#
// Asynchronous programming allows you to perform tasks without blocking the main thread, improving responsiveness and performance.
// The async keyword is used to define an asynchronous method, and the await keyword is used to wait for an asynchronous operation to complete without blocking the thread.

async Task<int> GetDataAsync()
{
    // Simulate an asynchronous operation
    await Task.Delay(1000);
    return 42; // Return some data after the delay
}

async void CallAsyncMethod()
{
    Console.WriteLine("Calling asynchronous method...");
    int result = await GetDataAsync(); // Await the asynchronous method
    Console.WriteLine($"Result: {result}");
}

async Task<int> GetDataWithErrorAsync()
{
    await Task.Delay(1000);
    throw new Exception("Something went wrong!"); // Simulate an error
}

async void CallAsyncMethodWithErrorHandling()
{
    try
    {
        Console.WriteLine("Calling asynchronous method with error handling...");
        int result = await GetDataWithErrorAsync();
        Console.WriteLine($"Result: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}