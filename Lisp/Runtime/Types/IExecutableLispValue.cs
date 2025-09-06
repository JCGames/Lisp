using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;

namespace Lisp.Types;

public interface IExecutableLispValue
{
    public IEnumerable<IParameterNode> Parameters { get; }
    
    BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope);
}