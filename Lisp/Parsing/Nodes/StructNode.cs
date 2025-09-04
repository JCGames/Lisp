namespace Lisp.Parsing.Nodes;

public class StructNode : Node
{
    public List<KeyValueNode> Struct { get; set; }
    
    public override void Print(string indent, TextWriter? writer = null)
    {
        writer?.WriteLine($"{indent}Struct: [");
        foreach (var keyValueNode in Struct)
        {
            keyValueNode.Print(indent + '\t', writer);
        }
        writer?.WriteLine($"{indent}]");
    }
}