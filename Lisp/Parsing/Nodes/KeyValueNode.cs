using Lisp.Diagnostics;
using Lisp.Parsing.Nodes.Classifications;

namespace Lisp.Parsing.Nodes;

public class KeyValueNode : Node, IParameterNode
{
    public IdentifierNode Key { get; set; }
    public Node Value { get; set; }
    
    string IParameterNode.PublicParameterName => Key.Text;
    string IParameterNode.PrivateParameterName => (Value as IdentifierNode)?.Text ?? throw Report.Error($"Unable to use {Value} in as a parameter", Value.Location);
    
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