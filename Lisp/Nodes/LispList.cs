namespace Lisp.Nodes;

public class LispList : Node
{
    public bool IsQuoted { get; set; }
    public List<Node> Nodes { get; set; } = [];

    public override void Print(string indent)
    {
        foreach (var node in Nodes)
        {
            node.Print(indent + '\t');
        }
    }
}