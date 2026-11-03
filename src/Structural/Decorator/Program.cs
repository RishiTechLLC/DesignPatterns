// Decorator: attach additional responsibilities to an object dynamically.
// Example: a coffee shop where condiments decorate a base beverage.

namespace DesignPatterns.Structural.Decorator;

// Component
public interface IBeverage
{
    string Description { get; }
    decimal Cost { get; }
}

// Concrete components
public class Espresso : IBeverage
{
    public string Description => "Espresso";
    public decimal Cost => 2.00m;
}

public class HouseBlend : IBeverage
{
    public string Description => "House Blend";
    public decimal Cost => 1.50m;
}

// Base decorator wraps another beverage.
public abstract class CondimentDecorator(IBeverage beverage) : IBeverage
{
    protected readonly IBeverage Beverage = beverage;
    public abstract string Description { get; }
    public abstract decimal Cost { get; }
}

// Concrete decorators
public class Milk(IBeverage beverage) : CondimentDecorator(beverage)
{
    public override string Description => Beverage.Description + ", Milk";
    public override decimal Cost => Beverage.Cost + 0.50m;
}

public class Mocha(IBeverage beverage) : CondimentDecorator(beverage)
{
    public override string Description => Beverage.Description + ", Mocha";
    public override decimal Cost => Beverage.Cost + 0.75m;
}

public class WhippedCream(IBeverage beverage) : CondimentDecorator(beverage)
{
    public override string Description => Beverage.Description + ", Whipped Cream";
    public override decimal Cost => Beverage.Cost + 0.40m;
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Decorator Pattern ===");

        IBeverage plain = new Espresso();
        Print(plain);

        IBeverage fancy = new WhippedCream(new Mocha(new Mocha(new Espresso())));
        Print(fancy);

        IBeverage latte = new Milk(new HouseBlend());
        Print(latte);
    }

    private static void Print(IBeverage b) => Console.WriteLine($"{b.Description,-45} ${b.Cost:0.00}");
}
