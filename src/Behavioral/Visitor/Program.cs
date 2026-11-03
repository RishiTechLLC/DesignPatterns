// Visitor: represent an operation to be performed on the elements of an object structure.
// Visitor lets you define new operations without changing the classes of the elements.
// Example: geometric shapes with area calculation and SVG export as separate visitors.

using System.Globalization;

namespace DesignPatterns.Behavioral.Visitor;

public interface IShapeVisitor<out TResult>
{
    TResult VisitCircle(Circle circle);
    TResult VisitRectangle(Rectangle rectangle);
    TResult VisitTriangle(Triangle triangle);
}

// Element interface: double dispatch via Accept.
public interface IShape
{
    TResult Accept<TResult>(IShapeVisitor<TResult> visitor);
}

public record Circle(double X, double Y, double Radius) : IShape
{
    public TResult Accept<TResult>(IShapeVisitor<TResult> v) => v.VisitCircle(this);
}

public record Rectangle(double X, double Y, double Width, double Height) : IShape
{
    public TResult Accept<TResult>(IShapeVisitor<TResult> v) => v.VisitRectangle(this);
}

public record Triangle(double Base, double Height) : IShape
{
    public TResult Accept<TResult>(IShapeVisitor<TResult> v) => v.VisitTriangle(this);
}

// Concrete visitor #1: compute area
public class AreaVisitor : IShapeVisitor<double>
{
    public double VisitCircle(Circle c) => Math.PI * c.Radius * c.Radius;
    public double VisitRectangle(Rectangle r) => r.Width * r.Height;
    public double VisitTriangle(Triangle t) => 0.5 * t.Base * t.Height;
}

// Concrete visitor #2: export to SVG markup
public class SvgExportVisitor : IShapeVisitor<string>
{
    private static string F(double d) => d.ToString(CultureInfo.InvariantCulture);

    public string VisitCircle(Circle c) => $"<circle cx=\"{F(c.X)}\" cy=\"{F(c.Y)}\" r=\"{F(c.Radius)}\" />";
    public string VisitRectangle(Rectangle r) =>
        $"<rect x=\"{F(r.X)}\" y=\"{F(r.Y)}\" width=\"{F(r.Width)}\" height=\"{F(r.Height)}\" />";
    public string VisitTriangle(Triangle t) =>
        $"<polygon points=\"0,{F(t.Height)} {F(t.Base)},{F(t.Height)} {F(t.Base / 2)},0\" />";
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Visitor Pattern ===");

        IShape[] shapes = [new Circle(10, 10, 5), new Rectangle(0, 0, 4, 6), new Triangle(3, 8)];

        var area = new AreaVisitor();
        var svg = new SvgExportVisitor();

        foreach (var shape in shapes)
        {
            Console.WriteLine($"{shape.GetType().Name,-10} area = {shape.Accept(area),8:F2}   svg = {shape.Accept(svg)}");
        }

        Console.WriteLine($"Total area: {shapes.Sum(s => s.Accept(area)):F2}");
    }
}
