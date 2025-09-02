using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class Or : ITurboFunction
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
        if (parameters.Count < 2) Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count, 2));

        foreach (var parameter in parameters)
        {
            var value = Runner.EvaluateNode(parameter, scope);
            if (value is not LispBooleanValue boolValue) throw Report.Error(new WrongArgumentTypeReportMessage("And expects it's arguments to be booleans."));
            
            if (boolValue.Value) return new LispBooleanValue(true);
        }

        return new LispBooleanValue(false);
    }
}