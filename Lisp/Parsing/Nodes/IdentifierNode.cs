using Lisp.Parsing.Nodes.Classifications;

namespace Lisp.Parsing.Nodes;

public class IdentifierNode : TokenNode, IParameterNode
{
    string IParameterNode.PublicParameterName => Text;
}