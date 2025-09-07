using Lisp.Diagnostics;

namespace Lisp.Parsing.Nodes.Classifications;

public interface INode
{
    public Location Location { get; set; }
}