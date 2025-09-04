using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Math;

public class Modulo : ITurboFunction
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
        if (parameters.Count < 2) throw Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count, 2), function.Location);

        var accum = GetValue(parameters[0], scope);        
        foreach (var parameter in parameters[1..])
        {
            var value = GetValue(parameter, scope);
            accum %= value;
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