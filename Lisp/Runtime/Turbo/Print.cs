using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class Print : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new RestIdentifierNode()
        {
            Text = "items",
            Location = Location.None
        },
    ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        foreach (var parameter in parameters)
        {
            var value = Runner.EvaluateNode(parameter, scope);
            Runner.StdOut.Write(value);
        }
        
        Runner.StdOut.WriteLine();
        
        return LispVoidValue.Instance;
    }
}