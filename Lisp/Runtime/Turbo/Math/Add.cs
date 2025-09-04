using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Math;

public class Add : ITurboFunction
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

        var accum = 0m;        
        foreach (var parameter in parameters)
        {
            var value = Runner.EvaluateNode(parameter, scope);
            if (value is not LispNumberValue number) throw Report.Error(new WrongArgumentTypeReportMessage("Multiply requires its arguments to be numbers."), parameter.Location);
            accum += number.Value;
        }
        
        return new LispNumberValue(accum);
    }
}