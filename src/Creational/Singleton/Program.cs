// Singleton: ensure a class has only one instance and provide a global point of access to it.
// Example: an application-wide configuration manager.

namespace DesignPatterns.Creational.Singleton;

public sealed class ConfigurationManager
{
    // Lazy<T> gives thread-safe, lazy initialization out of the box.
    private static readonly Lazy<ConfigurationManager> _instance = new(() => new ConfigurationManager());

    private readonly Dictionary<string, string> _settings = new();

    public static ConfigurationManager Instance => _instance.Value;

    public Guid InstanceId { get; } = Guid.NewGuid();

    // Private constructor prevents external instantiation.
    private ConfigurationManager()
    {
        Console.WriteLine("  [ConfigurationManager created]");
        _settings["AppName"] = "DesignPatterns Demo";
        _settings["Environment"] = "Development";
    }

    public string Get(string key) => _settings.TryGetValue(key, out var value) ? value : "(not set)";

    public void Set(string key, string value) => _settings[key] = value;
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Singleton Pattern ===");

        var config1 = ConfigurationManager.Instance;
        config1.Set("Theme", "Dark");

        var config2 = ConfigurationManager.Instance;
        Console.WriteLine($"Theme read via second reference: {config2.Get("Theme")}");
        Console.WriteLine($"AppName: {config2.Get("AppName")}");
        Console.WriteLine($"Same instance? {ReferenceEquals(config1, config2)}");

        // Even across threads only one instance is ever created.
        var ids = new System.Collections.Concurrent.ConcurrentBag<Guid>();
        Parallel.For(0, 10, _ => ids.Add(ConfigurationManager.Instance.InstanceId));
        Console.WriteLine($"Distinct instances across 10 threads: {ids.Distinct().Count()}");
    }
}
