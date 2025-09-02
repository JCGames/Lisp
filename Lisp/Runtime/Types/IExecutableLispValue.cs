using Lisp.Parsing.Nodes;

namespace Lisp.Types;

public interface IExecutableLispValue
{
    BaseLispValue Execute(List<Node> parameters, LispScope scope);
}