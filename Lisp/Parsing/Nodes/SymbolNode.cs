namespace Lisp.Parsing.Nodes;

public class SymbolNode : TokenNode
{
    public override void Print(string indent, TextWriter? writer = null)
    {
        writer ??= Console.Out;
        writer.WriteLine($"{indent}[{nameof(RestIdentifierNode)}, |'{Text}|, {Location?.Line}]");
    }
}