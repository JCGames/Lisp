using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo;

public class If : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new IdentifierNode()
        {
            Text = "condition",
            Location = Location.None
        },
        new IdentifierNode()
        {
            Text = "true-branch",
            Location = Location.None
        },
        new IdentifierNode()
        {
            Text = "false-branch",
            Location = Location.None
        }
    ];

    public IEnumerable<IParameterNode> Parameters => ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count < 2) Report.Error(new WrongArgumentCountReportMessage(ArgumentDeclaration, arguments.Count, 2), function.Location);

        var condition = Runner.EvaluateNode(arguments[0], scope);
        if (condition is not LispBooleanValue boolean) throw Report.Error(new WrongArgumentTypeReportMessage("If condition should return a boolean."), arguments[0].Location);

        if (boolean.Value)
        {
            return Runner.EvaluateNode(arguments[1], scope);
        }
        
        if (arguments.Count >= 3)
        {
            return Runner.EvaluateNode(arguments[2], scope);
        }

        return LispVoidValue.Instance;
    }
}