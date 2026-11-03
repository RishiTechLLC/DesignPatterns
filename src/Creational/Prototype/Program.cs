// Prototype: create new objects by copying an existing object (the prototype).
// Example: cloning document templates, showing shallow vs. deep copies, plus a prototype registry.

namespace DesignPatterns.Creational.Prototype;

public class Author(string name)
{
    public string Name { get; set; } = name;
}

public interface IPrototype<T>
{
    T ShallowClone();
    T DeepClone();
}

public class Document : IPrototype<Document>
{
    public string Title { get; set; } = "";
    public Author Author { get; set; } = new("Unknown");
    public List<string> Tags { get; set; } = new();

    // MemberwiseClone copies fields; reference types are shared.
    public Document ShallowClone() => (Document)MemberwiseClone();

    // Deep clone copies referenced objects too.
    public Document DeepClone()
    {
        var clone = (Document)MemberwiseClone();
        clone.Author = new Author(Author.Name);
        clone.Tags = new List<string>(Tags);
        return clone;
    }

    public override string ToString() =>
        $"'{Title}' by {Author.Name} [tags: {string.Join(", ", Tags)}]";
}

// Registry of pre-configured prototypes.
public class DocumentRegistry
{
    private readonly Dictionary<string, Document> _prototypes = new();

    public void Register(string key, Document prototype) => _prototypes[key] = prototype;

    public Document Create(string key) => _prototypes[key].DeepClone();
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Prototype Pattern ===");

        var original = new Document
        {
            Title = "Quarterly Report",
            Author = new Author("Alice"),
            Tags = ["finance", "q1"]
        };

        var shallow = original.ShallowClone();
        var deep = original.DeepClone();

        // Mutate the original's referenced objects.
        original.Author.Name = "Bob";
        original.Tags.Add("draft");

        Console.WriteLine($"Original : {original}");
        Console.WriteLine($"Shallow  : {shallow}   <- shares Author and Tags with original");
        Console.WriteLine($"Deep     : {deep}   <- fully independent");

        var registry = new DocumentRegistry();
        registry.Register("invoice", new Document { Title = "Invoice", Author = new("Billing"), Tags = ["invoice"] });
        registry.Register("memo", new Document { Title = "Memo", Author = new("HR"), Tags = ["internal"] });

        var invoice = registry.Create("invoice");
        invoice.Title = "Invoice #1001";
        Console.WriteLine($"From registry: {invoice}");
        Console.WriteLine($"From registry: {registry.Create("memo")}");
    }
}
