// Command: encapsulate a request as an object, allowing parameterization, queuing,
// logging, and undoable operations.
// Example: a text editor with undo/redo.

using System.Text;

namespace DesignPatterns.Behavioral.Command;

// Receiver
public class TextDocument
{
    private readonly StringBuilder _text = new();
    public string Text => _text.ToString();

    public void Insert(int position, string value) => _text.Insert(position, value);
    public void Delete(int position, int length) => _text.Remove(position, length);
}

// Command interface
public interface ICommand
{
    void Execute();
    void Undo();
}

// Concrete commands
public class AppendTextCommand(TextDocument doc, string text) : ICommand
{
    private int _position;

    public void Execute()
    {
        _position = doc.Text.Length;
        doc.Insert(_position, text);
    }

    public void Undo() => doc.Delete(_position, text.Length);

    public override string ToString() => $"Append \"{text}\"";
}

public class DeleteLastCommand(TextDocument doc, int count) : ICommand
{
    private string _deleted = "";
    private int _position;

    public void Execute()
    {
        int n = Math.Min(count, doc.Text.Length);
        _position = doc.Text.Length - n;
        _deleted = doc.Text.Substring(_position, n);
        doc.Delete(_position, n);
    }

    public void Undo() => doc.Insert(_position, _deleted);

    public override string ToString() => $"Delete last {count}";
}

// Invoker: executes commands and maintains history.
public class Editor(TextDocument doc)
{
    private readonly Stack<ICommand> _undo = new();
    private readonly Stack<ICommand> _redo = new();

    public void Run(ICommand command)
    {
        command.Execute();
        _undo.Push(command);
        _redo.Clear();
        Log($"Do   {command}");
    }

    public void Undo()
    {
        if (_undo.TryPop(out var command))
        {
            command.Undo();
            _redo.Push(command);
            Log($"Undo {command}");
        }
    }

    public void Redo()
    {
        if (_redo.TryPop(out var command))
        {
            command.Execute();
            _undo.Push(command);
            Log($"Redo {command}");
        }
    }

    private void Log(string action) => Console.WriteLine($"  {action,-25} => \"{doc.Text}\"");
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Command Pattern ===");

        var doc = new TextDocument();
        var editor = new Editor(doc);

        editor.Run(new AppendTextCommand(doc, "Hello"));
        editor.Run(new AppendTextCommand(doc, ", World"));
        editor.Run(new AppendTextCommand(doc, "!!!"));
        editor.Run(new DeleteLastCommand(doc, 2));
        editor.Undo();
        editor.Undo();
        editor.Redo();
    }
}
