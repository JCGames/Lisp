using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

/// <summary>
/// A Turbo Function is any natively implemented function.
/// Used for performance, special forms, etc.
/// </summary>
public interface ITurboFunction
{
    public List<Token> Arguments { get; }

    /// <summary>
    /// Takes a list of pre-evaluated nodes.
    /// Not required to follow normal evaluation rules.
    /// </summary>
    public BaseLispValue Execute(List<Node> parameters, LispScope scope);
}