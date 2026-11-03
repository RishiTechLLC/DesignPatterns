// Observer: define a one-to-many dependency so that when one object changes state,
// all its dependents are notified and updated automatically.
// Example: a stock ticker with subscribers, shown both with a classic interface
// and with C# events.

namespace DesignPatterns.Behavioral.Observer;

// ---- Classic GoF style ----
public interface IStockObserver
{
    void Update(string symbol, decimal price);
}

public class StockTicker
{
    private readonly List<IStockObserver> _observers = new();
    private readonly Dictionary<string, decimal> _prices = new();

    public void Subscribe(IStockObserver o) => _observers.Add(o);
    public void Unsubscribe(IStockObserver o) => _observers.Remove(o);

    public void SetPrice(string symbol, decimal price)
    {
        _prices[symbol] = price;
        foreach (var o in _observers.ToList()) o.Update(symbol, price);
    }
}

public class PriceDisplay(string name) : IStockObserver
{
    public void Update(string symbol, decimal price) =>
        Console.WriteLine($"  [{name}] {symbol} is now {price:C}");
}

public class PriceAlert(string symbol, decimal threshold) : IStockObserver
{
    public void Update(string s, decimal price)
    {
        if (s == symbol && price > threshold)
            Console.WriteLine($"  [ALERT] {s} crossed {threshold:C} (now {price:C})!");
    }
}

// ---- Idiomatic C# events ----
public class PriceChangedEventArgs(string symbol, decimal oldPrice, decimal newPrice) : EventArgs
{
    public string Symbol { get; } = symbol;
    public decimal OldPrice { get; } = oldPrice;
    public decimal NewPrice { get; } = newPrice;
}

public class Stock(string symbol, decimal price)
{
    public string Symbol { get; } = symbol;
    public decimal Price { get; private set; } = price;

    public event EventHandler<PriceChangedEventArgs>? PriceChanged;

    public void Update(decimal newPrice)
    {
        var old = Price;
        Price = newPrice;
        PriceChanged?.Invoke(this, new PriceChangedEventArgs(Symbol, old, newPrice));
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Observer Pattern ===");

        Console.WriteLine("Classic observer interface:");
        var ticker = new StockTicker();
        var mobile = new PriceDisplay("Mobile");
        ticker.Subscribe(mobile);
        ticker.Subscribe(new PriceDisplay("Web"));
        ticker.Subscribe(new PriceAlert("MSFT", 400m));

        ticker.SetPrice("MSFT", 390m);
        ticker.SetPrice("MSFT", 410m);
        ticker.Unsubscribe(mobile);
        ticker.SetPrice("AAPL", 225m);

        Console.WriteLine("C# events:");
        var stock = new Stock("NVDA", 120m);
        stock.PriceChanged += (_, e) =>
            Console.WriteLine($"  Logger: {e.Symbol} {e.OldPrice:C} -> {e.NewPrice:C}");
        stock.PriceChanged += (_, e) =>
        {
            var change = (e.NewPrice - e.OldPrice) / e.OldPrice;
            Console.WriteLine($"  Analytics: {e.Symbol} changed {change:P1}");
        };
        stock.Update(132m);
    }
}
