namespace Lisp.Parsing.Nodes;

public class AccessorNode : Node
{
    public List<IdentifierNode> Identifiers { get; set; }
    
    public override void Print(string indent, TextWriter? writer = null)
    {
        writer ??= Console.Out;
        writer.WriteLine($"{indent}[{nameof(AccessorNode)}]:");
        writer?.WriteLine($"{indent}[");
        
        foreach (var identifier in Identifiers)
        {
            identifier.Print(indent + '\t', writer);
        }
        
        writer?.WriteLine($"{indent}]");
    }
}