// Builder: separate the construction of a complex object from its representation so that
// the same construction process can create different representations.
// Example: building computers step by step, with a Director for common configurations.

using System.Text;

namespace DesignPatterns.Creational.Builder;

// Product
public class Computer
{
    public string Cpu { get; set; } = "";
    public int RamGb { get; set; }
    public string Storage { get; set; } = "";
    public string? Gpu { get; set; }
    public List<string> Extras { get; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"CPU={Cpu}, RAM={RamGb}GB, Storage={Storage}");
        if (Gpu is not null) sb.Append($", GPU={Gpu}");
        if (Extras.Count > 0) sb.Append($", Extras=[{string.Join(", ", Extras)}]");
        return sb.ToString();
    }
}

// Builder interface (fluent)
public interface IComputerBuilder
{
    IComputerBuilder WithCpu(string cpu);
    IComputerBuilder WithRam(int gb);
    IComputerBuilder WithStorage(string storage);
    IComputerBuilder WithGpu(string gpu);
    IComputerBuilder WithExtra(string extra);
    Computer Build();
}

// Concrete builder
public class ComputerBuilder : IComputerBuilder
{
    private Computer _computer = new();

    public IComputerBuilder WithCpu(string cpu) { _computer.Cpu = cpu; return this; }
    public IComputerBuilder WithRam(int gb) { _computer.RamGb = gb; return this; }
    public IComputerBuilder WithStorage(string storage) { _computer.Storage = storage; return this; }
    public IComputerBuilder WithGpu(string gpu) { _computer.Gpu = gpu; return this; }
    public IComputerBuilder WithExtra(string extra) { _computer.Extras.Add(extra); return this; }

    public Computer Build()
    {
        if (string.IsNullOrEmpty(_computer.Cpu))
            throw new InvalidOperationException("A computer needs a CPU.");

        var result = _computer;
        _computer = new Computer(); // reset so the builder can be reused
        return result;
    }
}

// Director: knows recipes for common configurations.
public class ComputerShop(IComputerBuilder builder)
{
    public Computer BuildOfficePc() =>
        builder.WithCpu("Intel i5").WithRam(16).WithStorage("512GB SSD").Build();

    public Computer BuildGamingPc() =>
        builder.WithCpu("AMD Ryzen 9").WithRam(32).WithStorage("2TB NVMe")
               .WithGpu("RTX 4080").WithExtra("RGB lighting").WithExtra("Liquid cooling")
               .Build();
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Builder Pattern ===");

        var builder = new ComputerBuilder();
        var shop = new ComputerShop(builder);

        Console.WriteLine($"Office PC : {shop.BuildOfficePc()}");
        Console.WriteLine($"Gaming PC : {shop.BuildGamingPc()}");

        // The client can also use the builder directly for a custom build.
        var custom = builder.WithCpu("Apple M3").WithRam(24).WithStorage("1TB SSD")
                            .WithExtra("Touch ID").Build();
        Console.WriteLine($"Custom PC : {custom}");
    }
}
