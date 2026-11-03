// Abstract Factory: provide an interface for creating families of related objects
// without specifying their concrete classes.
// Example: a cross-platform UI toolkit (Windows vs. macOS widgets).

namespace DesignPatterns.Creational.AbstractFactory;

// Abstract products
public interface IButton
{
    string Render();
}

public interface ICheckbox
{
    string Render();
}

// Windows family
public class WindowsButton : IButton
{
    public string Render() => "[ Windows Button ]";
}

public class WindowsCheckbox : ICheckbox
{
    public string Render() => "[x] Windows Checkbox";
}

// macOS family
public class MacButton : IButton
{
    public string Render() => "( macOS Button )";
}

public class MacCheckbox : ICheckbox
{
    public string Render() => "(*) macOS Checkbox";
}

// Abstract factory
public interface IGuiFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}

// Concrete factories
public class WindowsFactory : IGuiFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ICheckbox CreateCheckbox() => new WindowsCheckbox();
}

public class MacFactory : IGuiFactory
{
    public IButton CreateButton() => new MacButton();
    public ICheckbox CreateCheckbox() => new MacCheckbox();
}

// Client works only with abstractions.
public class SettingsDialog(IGuiFactory factory)
{
    private readonly IButton _okButton = factory.CreateButton();
    private readonly ICheckbox _rememberMe = factory.CreateCheckbox();

    public void Show()
    {
        Console.WriteLine($"  {_rememberMe.Render()}");
        Console.WriteLine($"  {_okButton.Render()}");
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Abstract Factory Pattern ===");

        foreach (var os in new[] { "Windows", "macOS" })
        {
            IGuiFactory factory = os == "Windows" ? new WindowsFactory() : new MacFactory();
            Console.WriteLine($"Settings dialog on {os}:");
            new SettingsDialog(factory).Show();
        }
    }
}
