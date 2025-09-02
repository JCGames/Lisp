using Lisp.Exceptions;
using Lisp.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class And : ITurboFunction
{
    private static readonly List<Token> ArgumentDeclaration =
    [
        new()
        {
            Type = TokenType.RestIdentifier,
            Text = "items",
            Line = -1,
        },
    ];

    public List<Token> Arguments => ArgumentDeclaration;

    BaseLispValue ITurboFunction.Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 2) throw new WrongArgumentCountException(Arguments, parameters.Count, 2);

        foreach (var parameter in parameters)
        {
            var value = Runner.EvaluateNode(parameter, scope);
            if (value is not LispBooleanValue boolValue) throw new WrongArgumentTypeException("And expects it's arguments to be booleans.");
            
            if (!boolValue.Value) return new LispBooleanValue(false);
        }

        return new LispBooleanValue(true);
    }
}