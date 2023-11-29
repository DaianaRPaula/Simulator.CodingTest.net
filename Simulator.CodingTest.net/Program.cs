using System.Collections.Concurrent;

class Program
{
    static async Task Main(string[] args)
    {
        // API endpoint
        string apiEndpoint = "https://localhost:7149/api/Customer";

        // Simulate POST requests
        var postTasks = new List<Task>();
        Parallel.ForEach(Enumerable.Range(0, 10), _ => postTasks.Add(SimulatePostRequest(apiEndpoint)));

        // Simulate GET requests
        var getTasks = new List<Task>();
        Parallel.ForEach(Enumerable.Range(0, 10), _ => getTasks.Add(SimulateGetRequest(apiEndpoint)));

        // Wait for all tasks to complete
        await Task.WhenAll(postTasks);
        await Task.WhenAll(getTasks);

        Console.WriteLine("All tasks completed.");
    }

    static async Task SimulatePostRequest(string apiEndpoint)
    {
        var customers = GenerateRandomCustomers();
        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(customers), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiEndpoint, content);

            // Display the response content
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"POST Request Status Code: {response.StatusCode}");
            Console.WriteLine($"POST Request Response: {responseContent}");
        }
    }

    static async Task SimulateGetRequest(string apiEndpoint)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(apiEndpoint);

            // Display the response content
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GET Request Status Code: {response.StatusCode}");
            Console.WriteLine($"GET Request Response: {responseContent}");
        }
    }

    static BlockingCollection<Customer> GenerateRandomCustomers()
    {
        var customers = new BlockingCollection<Customer>();
        var rand = new Random();

        for (int i = 0; i < 2; i++)
        {
            customers.Add(new Customer
            {
                FirstName = GetRandomFirstName(),
                LastName = GetRandomLastName(),
                Age = rand.Next(10, 91),
                Id = i + 1 // IDs increasing sequentially
            });
        }

        return customers;
    }

    static string GetRandomFirstName()
    {
        BlockingCollection<string> firstNames = new BlockingCollection<string>() { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };

        int index = new Random().Next(firstNames.Count);
        return firstNames.ElementAt(index);
    }

    static string GetRandomLastName()
    {
        BlockingCollection<string> lastNames = new BlockingCollection<string>() { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };
        int index = new Random().Next(lastNames.Count);
        return lastNames.ElementAt(index);
    }
}

class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public int Id { get; set; }
}
