using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class Or : ITurboFunction
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
        if (arguments.Count < 2) Report.Error(new WrongArgumentCountReportMessage(Parameters, arguments.Count, 2), function.Location);

        foreach (var parameter in arguments)
        {
            var value = Runner.EvaluateNode(parameter, scope);
            if (value is not LispBooleanValue boolValue) throw Report.Error(new WrongArgumentTypeReportMessage("And expects it's arguments to be booleans."), parameter.Location);
            
            if (boolValue.Value) return new LispBooleanValue(true);
        }

        return new LispBooleanValue(false);
    }
}