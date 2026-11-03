// Template Method: define the skeleton of an algorithm in a base class, deferring some steps
// to subclasses without changing the algorithm's structure.
// Example: a data-export pipeline (read -> transform -> write) with CSV and JSON exporters.

using System.Text;

namespace DesignPatterns.Behavioral.TemplateMethod;

public record Employee(string Name, string Department, decimal Salary);

public abstract class DataExporter
{
    // The template method: fixed algorithm skeleton. Not virtual, so subclasses can't change the order.
    public string Export(IEnumerable<Employee> data)
    {
        var records = Filter(data).ToList();       // hook with default behavior
        var sb = new StringBuilder();
        WriteHeader(sb);                           // optional hook
        for (int i = 0; i < records.Count; i++)
            WriteRecord(sb, records[i], i == records.Count - 1); // required step
        WriteFooter(sb);                           // optional hook
        return sb.ToString();
    }

    // Hooks (optional overrides)
    protected virtual IEnumerable<Employee> Filter(IEnumerable<Employee> data) => data;
    protected virtual void WriteHeader(StringBuilder sb) { }
    protected virtual void WriteFooter(StringBuilder sb) { }

    // Primitive operation (must override)
    protected abstract void WriteRecord(StringBuilder sb, Employee e, bool isLast);
}

public class CsvExporter : DataExporter
{
    protected override void WriteHeader(StringBuilder sb) => sb.AppendLine("Name,Department,Salary");
    protected override void WriteRecord(StringBuilder sb, Employee e, bool isLast) =>
        sb.AppendLine($"{e.Name},{e.Department},{e.Salary}");
}

public class JsonExporter : DataExporter
{
    protected override void WriteHeader(StringBuilder sb) => sb.AppendLine("[");
    protected override void WriteRecord(StringBuilder sb, Employee e, bool isLast) =>
        sb.AppendLine($"  {{ \"name\": \"{e.Name}\", \"department\": \"{e.Department}\", \"salary\": {e.Salary} }}{(isLast ? "" : ",")}");
    protected override void WriteFooter(StringBuilder sb) => sb.AppendLine("]");
}

// Overrides the Filter hook to change one step of the algorithm.
public class EngineeringCsvExporter : CsvExporter
{
    protected override IEnumerable<Employee> Filter(IEnumerable<Employee> data) =>
        data.Where(e => e.Department == "Engineering");
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Template Method Pattern ===");

        Employee[] employees =
        [
            new("Alice", "Engineering", 120000m),
            new("Bob", "Sales", 80000m),
            new("Carol", "Engineering", 135000m)
        ];

        DataExporter[] exporters = [new CsvExporter(), new JsonExporter(), new EngineeringCsvExporter()];
        foreach (var exporter in exporters)
        {
            Console.WriteLine($"--- {exporter.GetType().Name} ---");
            Console.Write(exporter.Export(employees));
        }
    }
}
