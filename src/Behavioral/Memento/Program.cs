// Memento: without violating encapsulation, capture and externalize an object's internal
// state so that the object can be restored to this state later.
// Example: a game character with save/load checkpoints.

namespace DesignPatterns.Behavioral.Memento;

// Originator
public class GameCharacter(string name)
{
    public string Name { get; } = name;
    private int _level = 1;
    private int _health = 100;
    private List<string> _inventory = new();

    public void Fight(int damage) { _health = Math.Max(0, _health - damage); }
    public void LevelUp() { _level++; _health = 100; }
    public void Pick(string item) => _inventory.Add(item);

    // Create a memento; its contents are opaque to everyone except the originator.
    public IMemento Save(string label) => new Snapshot(label, _level, _health, [.. _inventory]);

    public void Restore(IMemento memento)
    {
        if (memento is not Snapshot s) throw new ArgumentException("Unknown memento type");
        _level = s.Level;
        _health = s.Health;
        _inventory = [.. s.Inventory];
    }

    public override string ToString() =>
        $"{Name}: Level {_level}, HP {_health}, Items [{string.Join(", ", _inventory)}]";

    // Nested private type: only GameCharacter can read the state.
    private sealed record Snapshot(string Label, int Level, int Health, List<string> Inventory) : IMemento
    {
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}

// Narrow interface exposed to the caretaker.
public interface IMemento
{
    string Label { get; }
    DateTime CreatedAt { get; }
}

// Caretaker: stores mementos but never inspects or modifies their contents.
public class SaveSlots
{
    private readonly Stack<IMemento> _history = new();

    public void Push(IMemento m)
    {
        _history.Push(m);
        Console.WriteLine($"  Saved checkpoint '{m.Label}'");
    }

    public IMemento Pop() => _history.Pop();
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Memento Pattern ===");

        var hero = new GameCharacter("Aria");
        var slots = new SaveSlots();

        hero.Pick("Sword");
        hero.LevelUp();
        Console.WriteLine(hero);
        slots.Push(hero.Save("Before boss"));

        hero.Pick("Cursed Amulet");
        hero.Fight(95);
        Console.WriteLine($"After boss fight: {hero}");

        var checkpoint = slots.Pop();
        hero.Restore(checkpoint);
        Console.WriteLine($"Restored '{checkpoint.Label}': {hero}");
    }
}
