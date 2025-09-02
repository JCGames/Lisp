namespace Lisp.Parsing.Nodes;

public class RestIdentifierNode : IdentifierNode
{
    public override void Print(string indent, TextWriter? writer = null)
    {
        writer ??= Console.Out;
        writer.WriteLine($"{indent}[{nameof(RestIdentifierNode)}, |&{Text}|, {Location?.Line}]");
    }
}