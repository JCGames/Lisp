using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class DefineGlobal : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new()
        {
            Text = "name",
            Location = Location.None
        },
        new()
        {
            Text = "value",
            Location = Location.None
        }
    ];

    public List<IdentifierNode> Arguments =>  ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> parameters, LispScope scope)
    {
        if (parameters.Count != 2) Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count), function.Location);

        if (parameters[0] is not IdentifierNode identifier) throw Report.Error(new WrongArgumentTypeReportMessage($"Expected {Arguments[0].Text} to be an {nameof(IdentifierNode)}."), parameters[0].Location);
        var identifierName = identifier.Text;
        
        var value = Runner.EvaluateNode(parameters[1], scope);
        if (value is not LispValue lispValue) throw Report.Error(new WrongArgumentTypeReportMessage($"{value} is not a valid value."), parameters[1].Location);
        
        scope.UpdateGlobalScope(identifierName, lispValue);
        
        return LispVoidValue.Instance;
    }
}