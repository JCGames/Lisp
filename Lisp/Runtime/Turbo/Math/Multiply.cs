using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Math;

public class Multiply : ITurboFunction
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
        if (parameters.Count < 2) throw Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count, 2));

        var accum = 1m;        
        foreach (var parameter in parameters)
        {
            var value = Runner.EvaluateNode(parameter, scope);
            if (value is not LispNumberValue number) throw Report.Error(new WrongArgumentTypeReportMessage("Multiply requires its arguments to be numbers."));
            accum *= number.Value;
        }
        
        return new LispNumberValue(accum);
    }
}