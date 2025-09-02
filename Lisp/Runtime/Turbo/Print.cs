using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class Print : ITurboFunction
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
        
        Console.WriteLine(value);
        
        return new LispVoidValue();
    }
}