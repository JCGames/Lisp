namespace Lisp.Parsing.Nodes;

public class KeyValueNode : Node
{
    public IdentifierNode Key { get; set; }
    public Node Value { get; set; }
    
    public override void Print(string indent, TextWriter? writer = null)
    {
        writer ??= Console.Out;
        writer.WriteLine($"{indent}[{nameof(KeyValueNode)}]:");
        indent += "\t";
        writer.WriteLine($"{indent}KEY:");
        Key.Print(indent, writer);
        writer.WriteLine($"{indent}KEY:");
        Value.Print(indent, writer);
    }
}