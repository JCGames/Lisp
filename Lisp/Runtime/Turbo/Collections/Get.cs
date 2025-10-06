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
        new AccessorNode
        {
            Text = "path",
            Location = Location.None
        }
    ];

    public IEnumerable<IParameterNode> Parameters =>  ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        Report.AssertArgumentCount(Parameters, arguments.Count, function.Location);
        
        if (arguments[0] is not AccessorNode accessorNode) throw Report.Error("This should be an accessor.", arguments[1].Location);
        
        var firstArg = Runner.EvaluateNode(accessorNode.Identifiers[0], scope);
        if (firstArg is not ICollectionLispValue collection) throw Report.Error(new WrongArgumentTypeReportMessage("Get expects it's first argument to be a collection type"), function.Location);
        
        LispValue? result = null;
        for (var i = 1; i < accessorNode.Identifiers.Count; i++)
        {
            var node = accessorNode.Identifiers[i];
            
            var keyType = collection.KeyType;
            var accessor = node.Text;
            // if (!keyType.IsInstanceOfType(accessor)) throw Report.Error(new WrongArgumentTypeReportMessage($"The given collection expects it's accessor to be a {keyType.Name}"), node.Location);
            
            result = collection.GetValue(new LispStringValue(accessor));
            if (result is null) throw Report.Error($"The key {accessor} was not found in the collection", node.Location);

            if (i == accessorNode.Identifiers.Count - 1) continue;
            if (result is not ICollectionLispValue nextCollection) throw Report.Error("More accessors were passed in, but the value found was not a collection", accessorNode.Identifiers[i + 1].Location);
            collection = nextCollection;
        }

        if (result is null) throw Report.Error("No value was found", function.Location);
        
        return result;
    }
}