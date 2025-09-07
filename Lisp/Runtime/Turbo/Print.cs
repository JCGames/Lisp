using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;
using Lisp.Types;

namespace Lisp.Turbo;

public class Print : ITurboFunction
{
    private static readonly List<IParameterNode> ArgumentDeclaration =
    [
        new RestIdentifierNode()
        {
            Text = "items",
            Location = Location.None
        },
    ];

    public IEnumerable<IParameterNode> Parameters => ArgumentDeclaration;
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        foreach (var parameter in arguments)
        {
            var value = Runner.EvaluateNode(parameter, scope);
            Runner.StdOut.Write(value);
        }
        
        Runner.StdOut.WriteLine();
        
        return LispVoidValue.Instance;
    }
}