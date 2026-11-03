// Adapter: convert the interface of a class into another interface clients expect.
// Example: integrating a legacy XML-based payment gateway behind a modern IPaymentProcessor interface.

namespace DesignPatterns.Structural.Adapter;

// Target interface the application expects.
public interface IPaymentProcessor
{
    bool Pay(string customerId, decimal amount);
}

// Existing implementation that already matches the target.
public class StripeProcessor : IPaymentProcessor
{
    public bool Pay(string customerId, decimal amount)
    {
        Console.WriteLine($"  Stripe: charged {amount:C} to {customerId}");
        return true;
    }
}

// Adaptee: legacy API with an incompatible interface (amount in cents, XML payload).
public class LegacyPaymentGateway
{
    public string SubmitTransaction(string xmlPayload)
    {
        Console.WriteLine($"  LegacyGateway received: {xmlPayload}");
        return "<response><status>OK</status></response>";
    }
}

// Adapter: makes LegacyPaymentGateway look like an IPaymentProcessor.
public class LegacyPaymentAdapter(LegacyPaymentGateway gateway) : IPaymentProcessor
{
    public bool Pay(string customerId, decimal amount)
    {
        long cents = (long)(amount * 100);
        string xml = $"<txn><customer>{customerId}</customer><cents>{cents}</cents></txn>";
        string response = gateway.SubmitTransaction(xml);
        return response.Contains("<status>OK</status>");
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Adapter Pattern ===");

        IPaymentProcessor[] processors =
        [
            new StripeProcessor(),
            new LegacyPaymentAdapter(new LegacyPaymentGateway())
        ];

        foreach (var processor in processors)
        {
            Console.WriteLine($"{processor.GetType().Name}:");
            bool ok = processor.Pay("CUST-42", 19.99m);
            Console.WriteLine($"  Success: {ok}");
        }
    }
}
