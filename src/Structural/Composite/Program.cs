// Composite: compose objects into tree structures and let clients treat individual objects
// and compositions uniformly.
// Example: a file system with files and folders.

namespace DesignPatterns.Structural.Composite;

// Component
public abstract class FileSystemItem(string name)
{
    public string Name { get; } = name;
    public abstract long GetSize();
    public abstract void Print(string indent = "");
}

// Leaf
public class FileItem(string name, long size) : FileSystemItem(name)
{
    public override long GetSize() => size;
    public override void Print(string indent = "") =>
        Console.WriteLine($"{indent}- {Name} ({GetSize()} KB)");
}

// Composite
public class Folder(string name) : FileSystemItem(name)
{
    private readonly List<FileSystemItem> _children = new();

    public Folder Add(FileSystemItem item)
    {
        _children.Add(item);
        return this;
    }

    public void Remove(FileSystemItem item) => _children.Remove(item);

    public override long GetSize() => _children.Sum(c => c.GetSize());

    public override void Print(string indent = "")
    {
        Console.WriteLine($"{indent}+ {Name}/ ({GetSize()} KB)");
        foreach (var child in _children)
            child.Print(indent + "    ");
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Composite Pattern ===");

        var root = new Folder("root")
            .Add(new FileItem("readme.txt", 4))
            .Add(new Folder("src")
                .Add(new FileItem("Program.cs", 12))
                .Add(new FileItem("Utils.cs", 8))
                .Add(new Folder("Models")
                    .Add(new FileItem("User.cs", 3))))
            .Add(new Folder("images")
                .Add(new FileItem("logo.png", 250)));

        root.Print();

        // Client code treats a single file and a whole tree the same way.
        FileSystemItem single = new FileItem("notes.md", 2);
        Console.WriteLine($"Single file size: {single.GetSize()} KB, whole tree size: {root.GetSize()} KB");
    }
}
