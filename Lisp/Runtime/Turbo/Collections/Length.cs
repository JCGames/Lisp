using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo;

public class Length : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new IdentifierNode()
        {
            Text = "collection",
            Location = Location.None
        },
    ];

    public IEnumerable<IParameterNode> Parameters =>  ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count != 1) throw Report.Error(new WrongArgumentCountReportMessage(Parameters, arguments.Count), function.Location);
        
        var value = Runner.EvaluateNode(arguments[0], scope);
        if (value is not ICollectionLispValue collection) throw Report.Error(new WrongArgumentTypeReportMessage("Length expects it's argument to be a collection type"), function.Location);
        
        return collection.Count();
    }
}