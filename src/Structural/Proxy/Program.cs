// Proxy: provide a surrogate or placeholder for another object to control access to it.
// Example: a virtual proxy (lazy loading) + protection proxy (access control) + caching
// for an expensive image service.

namespace DesignPatterns.Structural.Proxy;

public interface IImage
{
    string FileName { get; }
    void Display(string user);
}

// Real subject: expensive to create.
public class HighResolutionImage : IImage
{
    public string FileName { get; }

    public HighResolutionImage(string fileName)
    {
        FileName = fileName;
        Console.WriteLine($"  [Loading '{fileName}' from disk... (expensive)]");
    }

    public void Display(string user) => Console.WriteLine($"  Displaying '{FileName}' to {user}");
}

// Proxy: same interface, controls access to the real subject.
public class ImageProxy(string fileName, ISet<string> authorizedUsers) : IImage
{
    private HighResolutionImage? _realImage;

    public string FileName { get; } = fileName;

    public void Display(string user)
    {
        // Protection proxy behavior
        if (!authorizedUsers.Contains(user))
        {
            Console.WriteLine($"  Access denied for {user} to '{FileName}'");
            return;
        }

        // Virtual proxy behavior: create the real object only on first use.
        _realImage ??= new HighResolutionImage(FileName);
        _realImage.Display(user);
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Proxy Pattern ===");

        var authorized = new HashSet<string> { "alice", "bob" };
        IImage image = new ImageProxy("vacation.jpg", authorized);
        Console.WriteLine("Proxy created (image not loaded yet).");

        image.Display("mallory");
        image.Display("alice");   // loads on first access
        image.Display("bob");     // reuses loaded image
    }
}
