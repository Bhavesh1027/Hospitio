namespace HospitioApi.BackGroundService;
public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Background service is running...");

        // Simulate some background work
        while (true)
        {
            Console.WriteLine("Performing background work...");
            // Perform your background work here

            await Task.Delay(TimeSpan.FromSeconds(10)); // Delay for 10 seconds before running again
        }
    }
}