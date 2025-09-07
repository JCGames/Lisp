using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo;

public class CreateLambda : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new IdentifierNode()
        {
            Text = "arguments",
            Location = Location.None
        },
        new IdentifierNode()
        {
            Text = "body",
            Location = Location.None
        }
    ];

    public IEnumerable<IParameterNode> Parameters => ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count < 2) Report.Error(new WrongArgumentCountReportMessage(ArgumentDeclaration, arguments.Count), function.Location);
        if (arguments[0] is not ListNode argNodeList) throw Report.Error(new WrongArgumentTypeReportMessage("Expected the first argument to be a list."), arguments[0].Location);
        
        var argList = argNodeList.Nodes
            .OfType<IParameterNode>()
            .ToList();
        
        if (argList.Count != argNodeList.Nodes.Count) Report.Error(new InvalidFunctionReportMessage("Expected all parameters to be identifiers."), arguments[0].Location);

        var rawBody = arguments[1..];
        var body = rawBody.OfType<ListNode>().ToList();
        if (body.Count != rawBody.Count) Report.Error(new InvalidFunctionReportMessage("Each item in the body must be a list."), arguments[1].Location);

        return new LispFunctionValue(function, argList, body);
    }
}