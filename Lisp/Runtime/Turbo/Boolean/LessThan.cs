using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class LessThan : ITurboFunction
{
    private static readonly List<TokenNode> ArgumentDeclaration =
    [
        new()
        {
            Type = TokenType.Identifier,
            Text = "left",
            Line = -1,
        },
        new()
        {
            Type = TokenType.Identifier,
            Text = "right",
            Line = -1,
        },
    ];

    public List<TokenNode> Arguments => ArgumentDeclaration;
    
    BaseLispValue ITurboFunction.Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count != 2) throw new WrongArgumentCountException(Arguments, parameters.Count);
        
        var left = Runner.EvaluateNode(parameters[0], scope);
        if (left is not LispNumberValue leftNumber) throw new WrongArgumentTypeException("Expected the left value to be a number");
        var right = Runner.EvaluateNode(parameters[1], scope);
        if (right is not LispNumberValue rightNumber) throw new WrongArgumentTypeException("Expected the right value to be a number");

        var result = leftNumber.Value < rightNumber.Value;
        return new LispBooleanValue(result);
    }
}