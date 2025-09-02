using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo.Boolean;

public class Not : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new()
        {
            Text = "item",
            Location = Location.None
        },
    ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count != 1) Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count));
        
        var value = Runner.EvaluateNode(parameters[0], scope);
        if (value is not LispBooleanValue boolValue) throw Report.Error(new WrongArgumentTypeReportMessage("Not only works with boolean arguments"));
        
        return new LispBooleanValue(!boolValue.Value);
    }
}