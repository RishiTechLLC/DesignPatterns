// State: allow an object to alter its behavior when its internal state changes.
// The object will appear to change its class.
// Example: an online order moving through New -> Paid -> Shipped -> Delivered (or Cancelled).

namespace DesignPatterns.Behavioral.State;

public interface IOrderState
{
    string Name { get; }
    void Pay(Order order) => Invalid(order, "pay");
    void Ship(Order order) => Invalid(order, "ship");
    void Deliver(Order order) => Invalid(order, "deliver");
    void Cancel(Order order) => Invalid(order, "cancel");

    private void Invalid(Order order, string action) =>
        Console.WriteLine($"  Cannot {action} order #{order.Id} while it is {Name}.");
}

public class NewState : IOrderState
{
    public string Name => "New";
    public void Pay(Order o) => o.TransitionTo(new PaidState());
    public void Cancel(Order o) => o.TransitionTo(new CancelledState());
}

public class PaidState : IOrderState
{
    public string Name => "Paid";
    public void Ship(Order o) => o.TransitionTo(new ShippedState());
    public void Cancel(Order o)
    {
        Console.WriteLine("  Refunding payment...");
        o.TransitionTo(new CancelledState());
    }
}

public class ShippedState : IOrderState
{
    public string Name => "Shipped";
    public void Deliver(Order o) => o.TransitionTo(new DeliveredState());
}

public class DeliveredState : IOrderState
{
    public string Name => "Delivered";
}

public class CancelledState : IOrderState
{
    public string Name => "Cancelled";
}

// Context
public class Order(int id)
{
    public int Id { get; } = id;
    private IOrderState _state = new NewState();
    public string Status => _state.Name;

    internal void TransitionTo(IOrderState next)
    {
        Console.WriteLine($"  Order #{Id}: {_state.Name} -> {next.Name}");
        _state = next;
    }

    public void Pay() => _state.Pay(this);
    public void Ship() => _state.Ship(this);
    public void Deliver() => _state.Deliver(this);
    public void Cancel() => _state.Cancel(this);
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== State Pattern ===");

        Console.WriteLine("Happy path:");
        var order1 = new Order(1);
        order1.Ship();     // invalid
        order1.Pay();
        order1.Ship();
        order1.Cancel();   // invalid
        order1.Deliver();
        Console.WriteLine($"  Final status: {order1.Status}");

        Console.WriteLine("Cancelled after payment:");
        var order2 = new Order(2);
        order2.Pay();
        order2.Cancel();
        order2.Ship();     // invalid
        Console.WriteLine($"  Final status: {order2.Status}");
    }
}
