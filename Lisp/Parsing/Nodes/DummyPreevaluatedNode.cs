using Lisp.Types;

namespace Lisp.Parsing.Nodes;

public class DummyPreEvaluatedNode : TokenNode
{
    public required LispValue Value { get; set; }
}