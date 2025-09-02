using Lisp.Diagnostics;
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
            Location = Location.None
        },
        new()
        {
            Text = "body",
            Location = Location.None
        }
    ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 2) Report.Error(new WrongArgumentCountReportMessage(ArgumentDeclaration, parameters.Count));
        if (parameters[0] is not ListNode argNodeList) throw Report.Error(new WrongArgumentTypeReportMessage("Expected the first argument to be a list."));
        
        var argList = argNodeList.Nodes
            .OfType<IdentifierNode>()
            .ToList();
        
        if (argList.Count != argNodeList.Nodes.Count) Report.Error(new InvalidFunctionReportMessage("Expected all parameters to be identifiers."));

        var rawBody = parameters[1..];
        var body = rawBody.OfType<ListNode>().ToList();
        if (body.Count != rawBody.Count) Report.Error(new InvalidFunctionReportMessage("Each item in the body must be a list."));

        return new LispFunctionValue(argList, body);
    }
}