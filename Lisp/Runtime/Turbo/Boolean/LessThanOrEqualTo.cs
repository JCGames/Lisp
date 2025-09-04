using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class LessThanOrEqualTo : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new()
        {
            Text = "left",
            Location = Location.None
        },
        new()
        {
            Text = "right",
            Location = Location.None
        },
    ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> parameters, LispScope scope)
    {
        if (parameters.Count != 2) Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count), function.Location);
        
        var left = Runner.EvaluateNode(parameters[0], scope);
        if (left is not LispNumberValue leftNumber) throw Report.Error(new WrongArgumentTypeReportMessage("Expected the left value to be a number"), parameters[0].Location);
        var right = Runner.EvaluateNode(parameters[1], scope);
        if (right is not LispNumberValue rightNumber) throw Report.Error(new WrongArgumentTypeReportMessage("Expected the right value to be a number"), parameters[1].Location);

        var result = leftNumber.Value <= rightNumber.Value;
        return new LispBooleanValue(result);
    }
}