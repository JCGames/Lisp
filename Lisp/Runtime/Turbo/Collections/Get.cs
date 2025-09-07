using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo;

public class Get : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new IdentifierNode()
        {
            Text = "collection",
            Location = Location.None
        },
        new RestIdentifierNode()
        {
            Text = "path",
            Location = Location.None
        }
    ];

    public IEnumerable<IParameterNode> Parameters =>  ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count < 2) throw Report.Error(new WrongArgumentCountReportMessage(Parameters, arguments.Count), function.Location);

        var firstArg = Runner.EvaluateNode(arguments[0], scope);
        if (firstArg is not ICollectionLispValue collection) throw Report.Error(new WrongArgumentTypeReportMessage("Get expects it's first argument to be a collection type"), function.Location);

        LispValue? result = null;
        for (var i = 1; i < arguments.Count; i++)
        {
            var node = arguments[i];
            
            var keyType = collection.KeyType;
            var accessor = Runner.EvaluateNode(node, scope);
            if (!keyType.IsAssignableFrom(accessor.GetType())) throw Report.Error(new WrongArgumentTypeReportMessage($"The given collection expects it's accessor to be a {keyType.Name}"), node.Location);
            
            result = collection.GetValue((LispValue)accessor);
            if (result is null) throw Report.Error($"The key {accessor} was not found in the collection", node.Location);

            if (i != arguments.Count - 1)
            {
                if (result is not ICollectionLispValue nextCollection) throw Report.Error("More accessors were passed in, but the value found was not a collection", arguments[i + 1].Location);
                collection = nextCollection;
            }
        }

        if (result is null) throw Report.Error("No value was found", function.Location);
        
        return result;
    }
}