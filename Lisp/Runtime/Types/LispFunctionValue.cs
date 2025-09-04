using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;

namespace Lisp.Types;

public class LispFunctionValue : LispValue, IExecutableLispValue
{
    public List<IdentifierNode> Arguments { get; }
    public List<ListNode> Definition { get; }

    public LispFunctionValue(List<IdentifierNode> arguments, List<ListNode> definition)
    {
        Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        if (Definition.Count == 0) throw Report.Error(new InvalidFunctionReportMessage("A function must contain a body!"));
    }


    public override string ToString()
    {
        var writer = new StringWriter();
        
        writer.WriteLine("Arguments:");
        // Arguments.Print("\t", writer);
        writer.WriteLine("Definition:");
        foreach (ListNode list in Definition)
        {
            writer.Write("-");
            list.Print("\t", writer);
        }
        return writer.ToString();
    }

    public BaseLispValue Execute(Node function, List<Node> parameters, LispScope scope)
    {
        if (parameters.Count != Arguments.Count) Report.Error(new WrongArgumentCountReportMessage(Arguments, parameters.Count));
        
        var newScope = scope.PushScope();
        for (var i = 0; i < Arguments.Count; i++)
        {
            var name = Arguments[i].Text;
            var value = Runner.EvaluateNode(parameters[i], scope);
            if (value is not LispValue lispValue) throw Report.Error(new WrongArgumentTypeReportMessage($"{value} is not a valid value."));

            newScope.UpdateScope(name, lispValue);
        }

        BaseLispValue result = null!;
        foreach (var list in Definition)
        {
            result = Runner.EvaluateNode(list, newScope);    
        }
        
        return result;
    }

    protected override bool Equals(BaseLispValue other)
    {
        if (other is not LispFunctionValue function) return false;
        if (!function.Arguments.Equals(Arguments)) return false;
        if (!function.Definition.Equals(Definition)) return false;
        
        return true;
    }

    public override int GetHashCode()
    {
        return Arguments.GetHashCode() ^ Definition.GetHashCode();
    }
}