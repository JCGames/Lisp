using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo;

public class Switch : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new RestIdentifierNode()
        {
            Text = "cases",
            Location = Location.None
        },
    ];

    public IEnumerable<IParameterNode> Parameters => ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count < 1) throw Report.Error(new WrongArgumentCountReportMessage(ArgumentDeclaration, arguments.Count), function.Location);

        foreach (var parameter in arguments)
        {
            if (parameter is not ListNode list) throw Report.Error(new WrongArgumentTypeReportMessage("Switch expects each of it's arguments to be lists."), parameter.Location);
            if (list.Nodes.Count != 2) throw Report.Error(new WrongArgumentTypeReportMessage("Each switch item should have two arguments - a condition and a body."), parameter.Location);

            var value = Runner.EvaluateNode(list.Nodes[0], scope);
            if (value is not LispBooleanValue boolean) throw Report.Error(new WrongArgumentTypeReportMessage("The first item in each switch item should be a boolean."), list.Nodes[0].Location);
            
            if (boolean.Value) return Runner.EvaluateNode(list.Nodes[1], scope);
        }

        return LispVoidValue.Instance;
    }
}