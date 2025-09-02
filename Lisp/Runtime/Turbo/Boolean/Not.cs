using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class Not : ITurboFunction
{
    private static readonly List<TokenNode> ArgumentDeclaration =
    [
        new()
        {
            Type = TokenType.Identifier,
            Text = "item",
            Line = -1,
        },
    ];

    public List<TokenNode> Arguments => ArgumentDeclaration;
    
    BaseLispValue ITurboFunction.Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count != 1) throw new WrongArgumentCountException(Arguments, parameters.Count);
        
        var value = Runner.EvaluateNode(parameters[0], scope);
        if (value is not LispBooleanValue boolValue) throw new WrongArgumentTypeException("Not only works with boolean arguments");
        
        return new LispBooleanValue(!boolValue.Value);
    }
}