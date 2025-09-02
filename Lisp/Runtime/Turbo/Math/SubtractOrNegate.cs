using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Math;

public class SubtractOrNegate : ITurboFunction
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
    
    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 1) throw Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count));

        var accum = GetValue(parameters[0], scope);
        if (parameters.Count == 1)
        {
            return new LispNumberValue(-accum);
        }
        
        foreach (var parameter in parameters[1..])
        {
            var value = GetValue(parameter, scope);
            accum -= value;
        }
        
        return new LispNumberValue(accum);
    }

    private decimal GetValue(Node node, LispScope scope)
    {
        var value = Runner.EvaluateNode(node, scope);
        if (value is not LispNumberValue number) throw Report.Error(new WrongArgumentTypeReportMessage("Multiply requires its arguments to be numbers."));
        return number.Value;
    }
}