// Flyweight: use sharing to support large numbers of fine-grained objects efficiently.
// Example: a forest of trees where tree types (intrinsic state) are shared,
// and coordinates (extrinsic state) are stored per tree.

namespace DesignPatterns.Structural.Flyweight;

// Flyweight: immutable, shared intrinsic state.
public sealed class TreeType(string name, string color, string texture)
{
    public string Name { get; } = name;
    public string Color { get; } = color;
    public string Texture { get; } = texture;

    public void Draw(int x, int y) =>
        Console.WriteLine($"  Drawing {Name} ({Color}, {Texture}) at ({x},{y})");
}

// Flyweight factory: caches and reuses TreeType instances.
public static class TreeTypeFactory
{
    private static readonly Dictionary<string, TreeType> _cache = new();

    public static int CachedTypes => _cache.Count;

    public static TreeType Get(string name, string color, string texture)
    {
        string key = $"{name}|{color}|{texture}";
        if (!_cache.TryGetValue(key, out var type))
        {
            type = new TreeType(name, color, texture);
            _cache[key] = type;
        }
        return type;
    }
}

// Context: holds extrinsic state plus a reference to the flyweight.
public readonly record struct Tree(int X, int Y, TreeType Type)
{
    public void Draw() => Type.Draw(X, Y);
}

public class Forest
{
    private readonly List<Tree> _trees = new();

    public int Count => _trees.Count;

    public void Plant(int x, int y, string name, string color, string texture) =>
        _trees.Add(new Tree(x, y, TreeTypeFactory.Get(name, color, texture)));

    public void Draw(int max) => _trees.Take(max).ToList().ForEach(t => t.Draw());
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Flyweight Pattern ===");

        var forest = new Forest();
        var random = new Random(42);
        string[] kinds = ["Oak", "Pine", "Birch"];

        for (int i = 0; i < 10_000; i++)
        {
            string kind = kinds[random.Next(kinds.Length)];
            forest.Plant(random.Next(1000), random.Next(1000), kind, kind == "Birch" ? "White" : "Green", $"{kind.ToLower()}.png");
        }

        forest.Draw(5);
        Console.WriteLine($"Trees planted: {forest.Count:N0}");
        Console.WriteLine($"TreeType objects in memory: {TreeTypeFactory.CachedTypes}");
    }
}
