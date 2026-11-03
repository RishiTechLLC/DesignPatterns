// Interpreter: given a language, define a representation for its grammar along with an
// interpreter that uses the representation to interpret sentences in the language.
// Example: evaluating arithmetic expressions with variables, e.g. "(x + 3) * (y - 1)".

namespace DesignPatterns.Behavioral.Interpreter;

public class Context
{
    private readonly Dictionary<string, int> _variables = new();
    public void Set(string name, int value) => _variables[name] = value;
    public int Get(string name) =>
        _variables.TryGetValue(name, out var v) ? v : throw new KeyNotFoundException($"Undefined variable '{name}'");
}

// Abstract expression
public interface IExpression
{
    int Interpret(Context context);
}

// Terminal expressions
public record NumberExpression(int Value) : IExpression
{
    public int Interpret(Context context) => Value;
    public override string ToString() => Value.ToString();
}

public record VariableExpression(string Name) : IExpression
{
    public int Interpret(Context context) => context.Get(Name);
    public override string ToString() => Name;
}

// Non-terminal expressions
public record AddExpression(IExpression Left, IExpression Right) : IExpression
{
    public int Interpret(Context c) => Left.Interpret(c) + Right.Interpret(c);
    public override string ToString() => $"({Left} + {Right})";
}

public record SubtractExpression(IExpression Left, IExpression Right) : IExpression
{
    public int Interpret(Context c) => Left.Interpret(c) - Right.Interpret(c);
    public override string ToString() => $"({Left} - {Right})";
}

public record MultiplyExpression(IExpression Left, IExpression Right) : IExpression
{
    public int Interpret(Context c) => Left.Interpret(c) * Right.Interpret(c);
    public override string ToString() => $"({Left} * {Right})";
}

// A small recursive-descent parser that builds the expression tree.
// Grammar:  expr := term (('+'|'-') term)* ; term := factor ('*' factor)* ; factor := number | name | '(' expr ')'
public class Parser(string input)
{
    private readonly List<string> _tokens = Tokenize(input);
    private int _pos;

    public IExpression Parse()
    {
        var expr = ParseExpr();
        if (_pos != _tokens.Count) throw new FormatException($"Unexpected token '{_tokens[_pos]}'");
        return expr;
    }

    private IExpression ParseExpr()
    {
        var left = ParseTerm();
        while (Peek() is "+" or "-")
        {
            string op = Next();
            var right = ParseTerm();
            left = op == "+" ? new AddExpression(left, right) : new SubtractExpression(left, right);
        }
        return left;
    }

    private IExpression ParseTerm()
    {
        var left = ParseFactor();
        while (Peek() == "*")
        {
            Next();
            left = new MultiplyExpression(left, ParseFactor());
        }
        return left;
    }

    private IExpression ParseFactor()
    {
        string token = Next();
        if (token == "(")
        {
            var inner = ParseExpr();
            if (Next() != ")") throw new FormatException("Expected ')'");
            return inner;
        }
        return int.TryParse(token, out int n) ? new NumberExpression(n) : new VariableExpression(token);
    }

    private string? Peek() => _pos < _tokens.Count ? _tokens[_pos] : null;
    private string Next() => _pos < _tokens.Count ? _tokens[_pos++] : throw new FormatException("Unexpected end of input");

    private static List<string> Tokenize(string s)
    {
        var tokens = new List<string>();
        for (int i = 0; i < s.Length;)
        {
            char c = s[i];
            if (char.IsWhiteSpace(c)) { i++; continue; }
            if ("+-*()".Contains(c)) { tokens.Add(c.ToString()); i++; continue; }
            int start = i;
            while (i < s.Length && char.IsLetterOrDigit(s[i])) i++;
            if (start == i) throw new FormatException($"Invalid character '{c}'");
            tokens.Add(s[start..i]);
        }
        return tokens;
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Interpreter Pattern ===");

        var context = new Context();
        context.Set("x", 5);
        context.Set("y", 10);

        foreach (var source in new[] { "1 + 2 * 3", "(x + 3) * (y - 1)", "x * x - y" })
        {
            IExpression expr = new Parser(source).Parse();
            Console.WriteLine($"{source,-20} parsed as {expr,-25} = {expr.Interpret(context)}");
        }
    }
}
