# DesignPatterns

All 23 Gang of Four (GoF) design patterns in C#. Each pattern has its own **console project** with a small, real-world example you can run.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or newer

## Build and run

```bash
# Build every project
dotnet build DesignPatterns.sln

# Run one pattern
dotnet run --project src/Creational/Singleton
dotnet run --project src/Behavioral/Observer
```

You can also open `DesignPatterns.sln` in Visual Studio or Rider. Projects are grouped into solution folders by category. Settings shared by every project (target framework, nullable, warnings as errors) are in `Directory.Build.props`.

## Patterns

### Creational: how objects get created

| Pattern | Project | Example |
|---|---|---|
| Singleton | [src/Creational/Singleton](src/Creational/Singleton/Program.cs) | Thread-safe app configuration manager using `Lazy<T>` |
| Factory Method | [src/Creational/FactoryMethod](src/Creational/FactoryMethod/Program.cs) | Logistics companies creating trucks, ships, drones |
| Abstract Factory | [src/Creational/AbstractFactory](src/Creational/AbstractFactory/Program.cs) | Cross-platform UI widgets (Windows / macOS) |
| Builder | [src/Creational/Builder](src/Creational/Builder/Program.cs) | Fluent computer builder with a director |
| Prototype | [src/Creational/Prototype](src/Creational/Prototype/Program.cs) | Document cloning (shallow vs. deep) plus a prototype registry |

### Structural: how objects are put together

| Pattern | Project | Example |
|---|---|---|
| Adapter | [src/Structural/Adapter](src/Structural/Adapter/Program.cs) | Legacy XML payment gateway behind a modern interface |
| Bridge | [src/Structural/Bridge](src/Structural/Bridge/Program.cs) | Remote controls × devices (TV, radio) |
| Composite | [src/Structural/Composite](src/Structural/Composite/Program.cs) | File system tree of files and folders |
| Decorator | [src/Structural/Decorator](src/Structural/Decorator/Program.cs) | Coffee with stackable condiments |
| Facade | [src/Structural/Facade](src/Structural/Facade/Program.cs) | Home theater "watch movie" in one call |
| Flyweight | [src/Structural/Flyweight](src/Structural/Flyweight/Program.cs) | 10,000 trees sharing 3 tree-type objects |
| Proxy | [src/Structural/Proxy](src/Structural/Proxy/Program.cs) | Lazy-loading, access-controlled image proxy |

### Behavioral: how objects interact and share work

| Pattern | Project | Example |
|---|---|---|
| Chain of Responsibility | [src/Behavioral/ChainOfResponsibility](src/Behavioral/ChainOfResponsibility/Program.cs) | Expense approval escalation chain |
| Command | [src/Behavioral/Command](src/Behavioral/Command/Program.cs) | Text editor with undo/redo |
| Interpreter | [src/Behavioral/Interpreter](src/Behavioral/Interpreter/Program.cs) | Arithmetic expression parser and evaluator with variables |
| Iterator | [src/Behavioral/Iterator](src/Behavioral/Iterator/Program.cs) | Playlist with a custom shuffle iterator and `yield return` |
| Mediator | [src/Behavioral/Mediator](src/Behavioral/Mediator/Program.cs) | Chat room coordinating users |
| Memento | [src/Behavioral/Memento](src/Behavioral/Memento/Program.cs) | Game save checkpoints |
| Observer | [src/Behavioral/Observer](src/Behavioral/Observer/Program.cs) | Stock ticker (classic interface and C# `event`) |
| State | [src/Behavioral/State](src/Behavioral/State/Program.cs) | Order lifecycle: New → Paid → Shipped → Delivered |
| Strategy | [src/Behavioral/Strategy](src/Behavioral/Strategy/Program.cs) | Interchangeable shipping cost strategies |
| Template Method | [src/Behavioral/TemplateMethod](src/Behavioral/TemplateMethod/Program.cs) | CSV / JSON data exporters |
| Visitor | [src/Behavioral/Visitor](src/Behavioral/Visitor/Program.cs) | Shape area calculation and SVG export |
