using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class DefineLocal : ITurboFunction
{
    private static readonly List<Token> ArgumentDeclaration =
    [
        new()
        {
            Type = TokenType.Identifier,
            Text = "name",
            Line = -1,
        },
        new()
        {
            Type = TokenType.Identifier,
            Text = "value",
            Line = -1,
        }
    ];

    public List<Token> Arguments { get; } = ArgumentDeclaration;
    
    BaseLispValue ITurboFunction.Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count != 2) throw new WrongArgumentCountException(Arguments, parameters.Count);

        if (parameters[0] is not Token { Type: TokenType.Identifier } identifier) throw new WrongArgumentTypeException($"Expected {Arguments[0].Text} to be an {TokenType.Identifier}.");
        var identifierName = identifier.Text;
        
        var value = Runner.EvaluateNode(parameters[1], scope);
        if (value is not LispValue lispValue) throw new WrongArgumentTypeException($"{value} is not a valid value.");
        
        scope.UpdateScope(identifierName, lispValue);
        
        return new LispVoidValue();
    }
}