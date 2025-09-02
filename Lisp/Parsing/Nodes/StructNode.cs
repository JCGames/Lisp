namespace Lisp.Parsing.Nodes;

public class StructNode : Node
{
    public List<KeyValueNode> Struct { get; set; }
    
    public override void Print(string indent, TextWriter? writer = null)
    {
        
    }
}