// Bridge: decouple an abstraction from its implementation so the two can vary independently.
// Example: remote controls (abstraction) working with different devices (implementation).

namespace DesignPatterns.Structural.Bridge;

// Implementation hierarchy
public interface IDevice
{
    string Name { get; }
    bool IsOn { get; }
    int Volume { get; set; }
    void PowerOn();
    void PowerOff();
}

public class Television : IDevice
{
    public string Name => "TV";
    public bool IsOn { get; private set; }
    public int Volume { get; set; } = 20;
    public void PowerOn() => IsOn = true;
    public void PowerOff() => IsOn = false;
}

public class Radio : IDevice
{
    public string Name => "Radio";
    public bool IsOn { get; private set; }
    public int Volume { get; set; } = 10;
    public void PowerOn() => IsOn = true;
    public void PowerOff() => IsOn = false;
}

// Abstraction hierarchy
public class RemoteControl(IDevice device)
{
    protected readonly IDevice Device = device;

    public void TogglePower()
    {
        if (Device.IsOn) Device.PowerOff(); else Device.PowerOn();
        Console.WriteLine($"  {Device.Name} power: {(Device.IsOn ? "ON" : "OFF")}");
    }

    public void VolumeUp()
    {
        Device.Volume = Math.Min(100, Device.Volume + 10);
        Console.WriteLine($"  {Device.Name} volume: {Device.Volume}");
    }
}

// Refined abstraction adds features without touching devices.
public class AdvancedRemoteControl(IDevice device) : RemoteControl(device)
{
    public void Mute()
    {
        Device.Volume = 0;
        Console.WriteLine($"  {Device.Name} muted");
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Bridge Pattern ===");

        Console.WriteLine("Basic remote + TV:");
        var basic = new RemoteControl(new Television());
        basic.TogglePower();
        basic.VolumeUp();

        Console.WriteLine("Advanced remote + Radio:");
        var advanced = new AdvancedRemoteControl(new Radio());
        advanced.TogglePower();
        advanced.VolumeUp();
        advanced.Mute();
        advanced.TogglePower();
    }
}
