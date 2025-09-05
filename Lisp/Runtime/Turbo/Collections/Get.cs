using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class Get : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new()
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

    public List<IdentifierNode> Arguments =>  ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 2) throw Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count), function.Location);

        var firstArg = Runner.EvaluateNode(parameters[0], scope);
        if (firstArg is not ICollectionLispValue collection) throw Report.Error(new WrongArgumentTypeReportMessage("Get expects it's first argument to be a collection type"), function.Location);

        LispValue? result = null;
        for (var i = 1; i < parameters.Count; i++)
        {
            var node = parameters[i];
            
            var keyType = collection.KeyType;
            var accessor = Runner.EvaluateNode(node, scope);
            if (!keyType.IsAssignableFrom(accessor.GetType())) throw Report.Error(new WrongArgumentTypeReportMessage($"The given collection expects it's accessor to be a {keyType.Name}"), node.Location);
            
            result = collection.GetValue((LispValue)accessor);
            if (result is null) throw Report.Error($"The key {accessor} was not found in the collection", node.Location);

            if (i != parameters.Count - 1)
            {
                if (result is not ICollectionLispValue nextCollection) throw Report.Error("More accessors were passed in, but the value found was not a collection", parameters[i + 1].Location);
                collection = nextCollection;
            }
        }

        if (result is null) throw Report.Error("No value was found", function.Location);
        
        return result;
    }
}