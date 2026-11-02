// Strategy: define a family of algorithms, encapsulate each one, and make them interchangeable.
// Example: a shopping cart whose shipping cost calculation can be swapped at runtime.

namespace DesignPatterns.Behavioral.Strategy;

public record CartItem(string Name, decimal Price, double WeightKg);

public interface IShippingStrategy
{
    string Name { get; }
    decimal Calculate(IReadOnlyList<CartItem> items);
}

public class StandardShipping : IShippingStrategy
{
    public string Name => "Standard (5-7 days)";
    public decimal Calculate(IReadOnlyList<CartItem> items) => 4.99m + (decimal)items.Sum(i => i.WeightKg) * 0.50m;
}

public class ExpressShipping : IShippingStrategy
{
    public string Name => "Express (1-2 days)";
    public decimal Calculate(IReadOnlyList<CartItem> items) => 14.99m + (decimal)items.Sum(i => i.WeightKg) * 1.25m;
}

public class FreeOverThresholdShipping(decimal threshold, IShippingStrategy fallback) : IShippingStrategy
{
    public string Name => $"Free over {threshold:C} (else {fallback.Name})";
    public decimal Calculate(IReadOnlyList<CartItem> items) =>
        items.Sum(i => i.Price) >= threshold ? 0m : fallback.Calculate(items);
}

// Context
public class ShoppingCart
{
    private readonly List<CartItem> _items = new();
    public IShippingStrategy Shipping { get; set; } = new StandardShipping();

    public void Add(CartItem item) => _items.Add(item);

    public decimal Subtotal => _items.Sum(i => i.Price);
    public decimal ShippingCost => Shipping.Calculate(_items);
    public decimal Total => Subtotal + ShippingCost;
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Strategy Pattern ===");

        var cart = new ShoppingCart();
        cart.Add(new CartItem("Keyboard", 49.99m, 1.2));
        cart.Add(new CartItem("Mouse", 19.99m, 0.3));
        Console.WriteLine($"Subtotal: {cart.Subtotal:C}");

        IShippingStrategy[] strategies =
        [
            new StandardShipping(),
            new ExpressShipping(),
            new FreeOverThresholdShipping(50m, new StandardShipping())
        ];

        foreach (var strategy in strategies)
        {
            cart.Shipping = strategy;
            Console.WriteLine($"  {strategy.Name,-45} shipping {cart.ShippingCost,8:C}  total {cart.Total,8:C}");
        }

        // Strategies can also be plain delegates in C#.
        Func<decimal, decimal> blackFriday = subtotal => subtotal * 0.8m;
        Console.WriteLine($"Black Friday price (lambda strategy): {blackFriday(cart.Subtotal):C}");
    }
}
