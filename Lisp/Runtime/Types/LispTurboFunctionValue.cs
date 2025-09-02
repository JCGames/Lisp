using Lisp.Parsing.Nodes;
using Lisp.Turbo;

namespace Lisp.Types;

public class LispTurboFunctionValue : LispValue, IExecutableLispValue
{
    
    public ITurboFunction Implementation { get; }
    public List<IdentifierNode> Arguments => Implementation.Arguments;

    public LispTurboFunctionValue(ITurboFunction implementation)
    {
        Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
    }

    public override string ToString()
    {
        var writer = new StringWriter();
        writer.WriteLine("Turbo Function (native implementation)");
        writer.WriteLine($"Original Name: Turbo.{Implementation.GetType().Name}");
        writer.WriteLine("Arguments:");
        // Implementation.Arguments.Print("\t", writer);
        
        return writer.ToString();
    }

    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        return Implementation.Execute(parameters, scope);
    }

    protected override bool Equals(BaseLispValue other) =>
        other is LispTurboFunctionValue otherTurbo
        && otherTurbo.Implementation == Implementation;

    public override int GetHashCode()
    {
        return Implementation.GetHashCode();
    }
}