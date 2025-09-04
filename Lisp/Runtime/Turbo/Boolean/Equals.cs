using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class Equals : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new()
        {
            Text = "items",
            Location = Location.None
        },
    ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 2) Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count, 2), function.Location);
        
        var left = Runner.EvaluateNode(parameters[0], scope);
        var right = Runner.EvaluateNode(parameters[1], scope);
        
        return new LispBooleanValue(left.Equals(right));
    }
}