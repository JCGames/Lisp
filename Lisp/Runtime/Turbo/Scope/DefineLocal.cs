using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo;

public class DefineLocal : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new IdentifierNode()
        {
            Text = "name",
            Location = Location.None
        },
        new IdentifierNode()
        {
            Text = "value",
            Location = Location.None
        }
    ];

    public IEnumerable<IParameterNode> Parameters { get; } = ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count != 2) Report.Error(new WrongArgumentCountReportMessage(Parameters, arguments.Count), function.Location);

        if (arguments[0] is not IdentifierNode identifier) throw Report.Error(new WrongArgumentTypeReportMessage($"Expected {Parameters.First().PublicParameterName} to be an {nameof(IdentifierNode)}."), arguments[0].Location);
        var identifierName = identifier.Text;
        
        var value = Runner.EvaluateNode(arguments[1], scope);
        if (value is not LispValue lispValue) throw Report.Error(new WrongArgumentTypeReportMessage($"{value} is not a valid value."), arguments[1].Location);
        
        scope.UpdateScope(identifierName, lispValue);
        
        return LispVoidValue.Instance;
    }
}