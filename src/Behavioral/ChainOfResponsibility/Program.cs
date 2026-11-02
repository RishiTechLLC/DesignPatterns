// Chain of Responsibility: pass a request along a chain of handlers; each handler decides
// either to process it or pass it to the next handler.
// Example: expense approval (Team Lead -> Manager -> Director -> CEO).

namespace DesignPatterns.Behavioral.ChainOfResponsibility;

public record ExpenseRequest(string Employee, decimal Amount, string Purpose);

public abstract class Approver(string title, decimal limit)
{
    private Approver? _next;

    // Returns the next handler to allow fluent chain construction.
    public Approver SetNext(Approver next)
    {
        _next = next;
        return next;
    }

    public void Handle(ExpenseRequest request)
    {
        if (request.Amount <= limit)
        {
            Console.WriteLine($"  {title} approved {request.Amount:C} for '{request.Purpose}' ({request.Employee})");
        }
        else if (_next is not null)
        {
            Console.WriteLine($"  {title} cannot approve {request.Amount:C}, escalating...");
            _next.Handle(request);
        }
        else
        {
            Console.WriteLine($"  {request.Amount:C} for '{request.Purpose}' REJECTED: exceeds every approval limit");
        }
    }
}

public class TeamLead() : Approver("Team Lead", 500m);
public class Manager() : Approver("Manager", 5_000m);
public class Director() : Approver("Director", 25_000m);
public class Ceo() : Approver("CEO", 100_000m);

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Chain of Responsibility Pattern ===");

        var chain = new TeamLead();
        chain.SetNext(new Manager()).SetNext(new Director()).SetNext(new Ceo());

        ExpenseRequest[] requests =
        [
            new("Ann", 120m, "Team lunch"),
            new("Raj", 3_200m, "Conference tickets"),
            new("Li", 18_000m, "New servers"),
            new("Sam", 250_000m, "Office building")
        ];

        foreach (var r in requests)
        {
            Console.WriteLine($"Request: {r.Purpose}");
            chain.Handle(r);
        }
    }
}
