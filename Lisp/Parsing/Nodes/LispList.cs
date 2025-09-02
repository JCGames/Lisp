namespace Lisp.Parsing.Nodes;

public class LispList : Node
{
    public bool IsQuoted { get; set; }
    public List<Node> Nodes { get; set; } = [];

    public override void Print(string indent, TextWriter? writer = null)
    {
        foreach (var node in Nodes)
        {
            node.Print(indent + '\t', writer);
        }
    }
}