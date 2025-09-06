using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class GreaterThan : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new IdentifierNode()
        {
            Text = "left",
            Location = Location.None
        },
        new IdentifierNode()
        {
            Text = "right",
            Location = Location.None
        },
    ];

    public IEnumerable<IParameterNode> Parameters => ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count != 2) Report.Error(new WrongArgumentCountReportMessage(Parameters, arguments.Count), function.Location);
        
        var left = Runner.EvaluateNode(arguments[0], scope);
        if (left is not LispNumberValue leftNumber) throw Report.Error(new WrongArgumentTypeReportMessage("Expected the left value to be a number"), arguments[0].Location);
        var right = Runner.EvaluateNode(arguments[1], scope);
        if (right is not LispNumberValue rightNumber) throw Report.Error(new WrongArgumentTypeReportMessage("Expected the right value to be a number"), arguments[1].Location);

        var result = leftNumber.Value > rightNumber.Value;
        return new LispBooleanValue(result);
    }
}