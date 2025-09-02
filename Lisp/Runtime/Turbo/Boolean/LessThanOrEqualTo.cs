using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class LessThanOrEqualTo : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new()
        {
            Text = "left",
        },
        new()
        {
            Text = "right",
        },
    ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count != 2) throw new WrongArgumentCountException(Arguments, parameters.Count);
        
        var left = Runner.EvaluateNode(parameters[0], scope);
        if (left is not LispNumberValue leftNumber) throw new WrongArgumentTypeException("Expected the left value to be a number");
        var right = Runner.EvaluateNode(parameters[1], scope);
        if (right is not LispNumberValue rightNumber) throw new WrongArgumentTypeException("Expected the right value to be a number");

        var result = leftNumber.Value <= rightNumber.Value;
        return new LispBooleanValue(result);
    }
}