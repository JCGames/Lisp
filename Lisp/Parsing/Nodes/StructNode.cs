using System.Collections;
using Lisp.Types;

namespace Lisp.Parsing.Nodes;

public class StructNode : Node, IEnumerable<KeyValueNode>
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

    public IEnumerator<KeyValueNode> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}