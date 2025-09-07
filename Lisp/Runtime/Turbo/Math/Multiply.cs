using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo.Math;

public class Multiply : ITurboFunction
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
        if (arguments.Count < 2) throw Report.Error(new WrongArgumentCountReportMessage(Parameters, arguments.Count, 2), function.Location);

        var accum = 1m;        
        foreach (var parameter in arguments)
        {
            var value = Runner.EvaluateNode(parameter, scope);
            if (value is not LispNumberValue number) throw Report.Error(new WrongArgumentTypeReportMessage("Multiply requires its arguments to be numbers."), parameter.Location);
            accum *= number.Value;
        }
        
        return new LispNumberValue(accum);
    }
}