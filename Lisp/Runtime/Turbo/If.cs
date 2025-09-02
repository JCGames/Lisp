using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class If : ITurboFunction
{
    private static readonly List<TokenNode> ArgumentDeclaration =
    [
        new()
        {
            Text = "condition",
            Type = TokenType.Identifier,
            Line = -1
        },
        new()
        {
            Text = "true-branch",
            Type = TokenType.Identifier,
            Line = -1
        },
        new()
        {
            Text = "false-branch",
            Type = TokenType.Identifier,
            Line = -1
        }
    ];

    public List<TokenNode> Arguments => ArgumentDeclaration;
    
    BaseLispValue ITurboFunction.Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 2) throw new WrongArgumentCountException(ArgumentDeclaration, parameters.Count, 2);

        var condition = Runner.EvaluateNode(parameters[0], scope);
        if (condition is not LispBooleanValue boolean) throw new WrongArgumentTypeException("If condition should return a boolean.");

        if (boolean.Value)
        {
            return Runner.EvaluateNode(parameters[1], scope);
        }
        
        if (parameters.Count >= 3)
        {
            return Runner.EvaluateNode(parameters[2], scope);
        }

        return new LispVoidValue();
    }
}