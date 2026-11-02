// Factory Method: define an interface for creating an object, but let subclasses decide
// which class to instantiate.
// Example: a logistics app where each logistics type creates its own transport.

namespace DesignPatterns.Creational.FactoryMethod;

// Product
public interface ITransport
{
    string Deliver(string cargo);
}

// Concrete products
public class Truck : ITransport
{
    public string Deliver(string cargo) => $"Truck delivers '{cargo}' by road.";
}

public class Ship : ITransport
{
    public string Deliver(string cargo) => $"Ship delivers '{cargo}' by sea in a container.";
}

public class Drone : ITransport
{
    public string Deliver(string cargo) => $"Drone delivers '{cargo}' by air.";
}

// Creator
public abstract class Logistics
{
    // The factory method
    protected abstract ITransport CreateTransport();

    // Business logic that relies on the product created by the factory method.
    public void PlanDelivery(string cargo)
    {
        ITransport transport = CreateTransport();
        Console.WriteLine($"{GetType().Name}: {transport.Deliver(cargo)}");
    }
}

// Concrete creators
public class RoadLogistics : Logistics
{
    protected override ITransport CreateTransport() => new Truck();
}

public class SeaLogistics : Logistics
{
    protected override ITransport CreateTransport() => new Ship();
}

public class AirLogistics : Logistics
{
    protected override ITransport CreateTransport() => new Drone();
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Factory Method Pattern ===");

        Logistics[] companies = [new RoadLogistics(), new SeaLogistics(), new AirLogistics()];

        foreach (var logistics in companies)
        {
            logistics.PlanDelivery("100 boxes of books");
        }
    }
}
