using Lisp.Parsing.Nodes;

namespace Lisp.Exceptions;

public class WrongArgumentCountException : LispException
{
    public WrongArgumentCountException(List<TokenNode> expectedArguments, int passedCount, int? requiredCount = null)
        : base($"{requiredCount ?? expectedArguments.Count} arguments were required ({string.Join(", ", expectedArguments)}), but only {passedCount} were passed.")
    { }
}