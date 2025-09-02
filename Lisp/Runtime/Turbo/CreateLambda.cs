using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class CreateLambda : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new()
        {
            Text = "arguments",
        },
        new()
        {
            Text = "body",
        }
    ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 2) throw new WrongArgumentCountException(ArgumentDeclaration, parameters.Count);
        if (parameters[0] is not ListNode argNodeList) throw new WrongArgumentTypeException("Expected the first argument to be a list.");
        
        var argList = argNodeList.Nodes
            .OfType<IdentifierNode>()
            .ToList();
        
        if (argList.Count != argNodeList.Nodes.Count) throw new InvalidFunctionException("Expected all parameters to be identifiers.");

        var rawBody = parameters[1..];
        var body = rawBody.OfType<ListNode>().ToList();
        if (body.Count != rawBody.Count) throw new InvalidFunctionException("Each item in the body must be a list.");

        return new LispFunctionValue(argList, body);
    }
}