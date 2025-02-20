using System;

class Greeter
{
    public string Greet(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) // This handles blank input
            return "Hello, anonymous user!";
        
        return $"Hello, {name.Trim()}!";
    }
}

class User
{
    public string SendName(Greeter greeter, string name)
    {
        // Remove the this == null check since it's not possible
        if (greeter == null) // This handles if the greeter object is not created
        {
            throw new ArgumentNullException(nameof(greeter), "Greeter object cannot be null");
        }
        return greeter.Greet(name);
    }
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            User user = new User(); // If you want to check for null User, do it here
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null");
            }

            Greeter greeter = new Greeter();

            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            string greeting = user.SendName(greeter, name);
            Console.WriteLine(greeting);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}