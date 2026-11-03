// Facade: provide a unified, simplified interface to a set of interfaces in a subsystem.
// Example: a home theater with many components controlled through one facade.

namespace DesignPatterns.Structural.Facade;

// Subsystem classes
public class Amplifier
{
    public void On() => Console.WriteLine("  Amplifier on");
    public void SetVolume(int level) => Console.WriteLine($"  Amplifier volume set to {level}");
    public void Off() => Console.WriteLine("  Amplifier off");
}

public class Projector
{
    public void On() => Console.WriteLine("  Projector on");
    public void WideScreenMode() => Console.WriteLine("  Projector in widescreen mode (16:9)");
    public void Off() => Console.WriteLine("  Projector off");
}

public class StreamingPlayer
{
    public void On() => Console.WriteLine("  Streaming player on");
    public void Play(string movie) => Console.WriteLine($"  Streaming player playing \"{movie}\"");
    public void Stop() => Console.WriteLine("  Streaming player stopped");
    public void Off() => Console.WriteLine("  Streaming player off");
}

public class Lights
{
    public void Dim(int percent) => Console.WriteLine($"  Lights dimmed to {percent}%");
    public void On() => Console.WriteLine("  Lights on");
}

// Facade
public class HomeTheaterFacade(Amplifier amp, Projector projector, StreamingPlayer player, Lights lights)
{
    public void WatchMovie(string movie)
    {
        Console.WriteLine($"Get ready to watch \"{movie}\"...");
        lights.Dim(10);
        projector.On();
        projector.WideScreenMode();
        amp.On();
        amp.SetVolume(5);
        player.On();
        player.Play(movie);
    }

    public void EndMovie()
    {
        Console.WriteLine("Shutting the theater down...");
        player.Stop();
        player.Off();
        amp.Off();
        projector.Off();
        lights.On();
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Facade Pattern ===");

        var theater = new HomeTheaterFacade(new Amplifier(), new Projector(), new StreamingPlayer(), new Lights());
        theater.WatchMovie("Inception");
        theater.EndMovie();
    }
}
