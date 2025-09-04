using Lisp.Parsing.Nodes;

namespace Lisp.Types;

public interface IExecutableLispValue
{
    public List<IdentifierNode> Arguments { get; }
    
    BaseLispValue Execute(Node function, List<Node> parameters, LispScope scope);
}