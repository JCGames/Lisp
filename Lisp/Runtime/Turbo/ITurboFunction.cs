using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

/// <summary>
/// A Turbo Function is any natively implemented function.
/// Used for performance, special forms, etc.
/// </summary>
public interface ITurboFunction : IExecutableLispValue
{ }