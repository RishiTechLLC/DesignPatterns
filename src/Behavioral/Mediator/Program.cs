// Mediator: define an object that encapsulates how a set of objects interact, promoting
// loose coupling by keeping objects from referring to each other explicitly.
// Example: a chat room where users communicate only through the room.

namespace DesignPatterns.Behavioral.Mediator;

public interface IChatMediator
{
    void Join(User user);
    void Send(string message, User from);
    void SendPrivate(string message, User from, string toName);
}

public class ChatRoom(string name) : IChatMediator
{
    private readonly Dictionary<string, User> _users = new();

    public void Join(User user)
    {
        _users[user.Name] = user;
        user.Room = this;
        Broadcast($"{user.Name} joined #{name}", exclude: user);
    }

    public void Send(string message, User from) => Broadcast($"{from.Name}: {message}", exclude: from);

    public void SendPrivate(string message, User from, string toName)
    {
        if (_users.TryGetValue(toName, out var to))
            to.Receive($"(private) {from.Name}: {message}");
        else
            from.Receive($"[system] user '{toName}' not found");
    }

    private void Broadcast(string text, User exclude)
    {
        foreach (var u in _users.Values.Where(u => u != exclude))
            u.Receive(text);
    }
}

// Colleague: knows only the mediator, never other users directly.
public class User(string name)
{
    public string Name { get; } = name;
    public IChatMediator? Room { get; set; }

    public void Say(string message) => Room?.Send(message, this);
    public void Whisper(string to, string message) => Room?.SendPrivate(message, this, to);
    public void Receive(string text) => Console.WriteLine($"  [{Name}'s screen] {text}");
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Mediator Pattern ===");

        var room = new ChatRoom("design-patterns");
        var alice = new User("Alice");
        var bob = new User("Bob");
        var carol = new User("Carol");

        room.Join(alice);
        room.Join(bob);
        room.Join(carol);

        alice.Say("Hi everyone!");
        bob.Whisper("Alice", "Hey Alice, got a minute?");
        carol.Whisper("Dave", "Are you there?");
    }
}
