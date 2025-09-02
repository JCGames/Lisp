using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class Equals : ITurboFunction
{
    private static readonly List<TokenNode> ArgumentDeclaration =
    [
        new()
        {
            Type = TokenType.RestIdentifier,
            Text = "items",
            Line = -1,
        },
    ];

    public List<TokenNode> Arguments => ArgumentDeclaration;
    
    BaseLispValue ITurboFunction.Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 2) throw new WrongArgumentCountException(Arguments, parameters.Count, 2);
        
        var left = Runner.EvaluateNode(parameters[0], scope);
        var right = Runner.EvaluateNode(parameters[1], scope);
        
        return new LispBooleanValue(left.Equals(right));
    }
}