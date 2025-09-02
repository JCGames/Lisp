using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Math;

public class Subtract : ITurboFunction
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
        if (parameters.Count < 2) throw new WrongArgumentCountException(Arguments, parameters.Count);

        var accum = GetValue(parameters[0], scope);        
        foreach (var parameter in parameters[1..])
        {
            var value = GetValue(parameter, scope);
            accum -= value;
        }
        
        return new LispNumberValue(accum);
    }

    private decimal GetValue(Node node, LispScope scope)
    {
        
        var value = Runner.EvaluateNode(node, scope);
        if (value is not LispNumberValue number) throw new WrongArgumentTypeException("Multiply requires its arguments to be numbers.");
        return number.Value;
    }
}