using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class CreateLambda : ITurboFunction
{
    private static readonly List<TokenNode> ArgumentDeclaration =
    [
        new()
        {
            Type = TokenType.Identifier,
            Text = "arguments",
            Line = -1,
        },
        new()
        {
            Type = TokenType.RestIdentifier,
            Text = "body",
            Line = -1,
        }
    ];

    public List<TokenNode> Arguments => ArgumentDeclaration;
    
    BaseLispValue ITurboFunction.Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 2) throw new WrongArgumentCountException(ArgumentDeclaration, parameters.Count);
        if (parameters[0] is not ListNode argNodeList) throw new WrongArgumentTypeException("Expected the first argument to be a list.");
        
        var argList = argNodeList.Nodes
            .OfType<TokenNode>()
            .Where(t => t.Type == TokenType.Identifier)
            .ToList();
        
        if (argList.Count != argNodeList.Nodes.Count) throw new InvalidFunctionException("Expected all parameters to be identifiers.");

        var rawBody = parameters[1..];
        var body = rawBody.OfType<ListNode>().ToList();
        if (body.Count != rawBody.Count) throw new InvalidFunctionException("Each item in the body must be a list.");

        return new LispFunctionValue(argList, body);
    }
}