using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class Print : ITurboFunction
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
        
        Console.WriteLine(value);
        
        return new LispVoidValue();
    }
}