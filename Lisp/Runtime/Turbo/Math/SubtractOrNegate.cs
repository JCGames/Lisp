using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo.Math;

public class SubtractOrNegate : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new IdentifierNode()
        {
            Text = "items",
            Location = Location.None
        },
    ];

    public IEnumerable<IParameterNode> Parameters => ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count < 1) throw Report.Error(new WrongArgumentCountReportMessage(Parameters, arguments.Count), function.Location);

        var accum = GetValue(arguments[0], scope);
        if (arguments.Count == 1)
        {
            return new LispNumberValue(-accum);
        }
        
        foreach (var parameter in arguments[1..])
        {
            var value = GetValue(parameter, scope);
            accum -= value;
        }
        
        return new LispNumberValue(accum);
    }

    private decimal GetValue(Node node, LispScope scope)
    {
        var value = Runner.EvaluateNode(node, scope);
        if (value is not LispNumberValue number) throw Report.Error(new WrongArgumentTypeReportMessage("Multiply requires its arguments to be numbers."), node.Location);
        return number.Value;
    }
}