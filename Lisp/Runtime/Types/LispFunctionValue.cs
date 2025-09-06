using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;

namespace Lisp.Types;

public class LispFunctionValue : LispValue, IExecutableLispValue
{
    public IEnumerable<IParameterNode> Parameters => PositionalParameters
        .Concat<IParameterNode>(NamedParameters.Select(pair => new KeyValueNode()
        {
            Key = pair.Key,
            Value = pair.Value,
            Location = pair.Value.Location
        }))
        .Append(RestParameter)
        .OfType<IParameterNode>();

    public List<IdentifierNode> PositionalParameters { get; } = new();
    public Dictionary<IdentifierNode, IdentifierNode> NamedParameters { get; } = new();
    public RestIdentifierNode? RestParameter { get; }
    public List<ListNode> Definition { get; }

    public LispFunctionValue(Node function, List<IParameterNode> parameters, List<ListNode> definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        if (Definition.Count == 0) throw Report.Error(new InvalidFunctionReportMessage("A function must contain a body."), function.Location);

        int i;
        for (i = 0; i < parameters.Count; i++)
        {
            if (parameters[i] is KeyValueNode or RestIdentifierNode) break;
            if (parameters[i] is not IdentifierNode identifier) throw Report.Error(new InvalidFunctionReportMessage("Unexpected parameter type."), parameters[i].Location);

            PositionalParameters.Add(identifier);
        }

        for (; i < parameters.Count; i++)
        {
            if (parameters[i] is RestIdentifierNode identifier) break;
            if (parameters[i] is not KeyValueNode rest) throw Report.Error(new InvalidFunctionReportMessage("Unexpected parameter type."), parameters[i].Location);
            if (rest.Value is not IdentifierNode value) throw Report.Error(new InvalidFunctionReportMessage("Unexpected parameter type."), rest.Value.Location);

            NamedParameters.Add(rest.Key, value);
        }

        for (; i < parameters.Count; i++)
        {
            if (parameters[i] is not RestIdentifierNode rest) throw Report.Error(new InvalidFunctionReportMessage("Rest parameters must be the final parameter."), parameters[i].Location);
            if (RestParameter is not null) throw Report.Error(new InvalidFunctionReportMessage("Only one rest parameter is allowed."), RestParameter.Location);

            RestParameter = rest;
        }
    }
    
    public BaseLispValue Execute(Node function, List<Node> arguments, LispScope scope)
    {
        if (arguments.Count < PositionalParameters.Count) Report.Error(new WrongArgumentCountReportMessage(PositionalParameters, arguments.Count));

        var newScope = ProcessArguments(arguments, scope);
        
        BaseLispValue result = null!;
        foreach (var list in Definition)
        {
            result = Runner.EvaluateNode(list, newScope);    
        }
        
        return result;
    }

    private LispScope ProcessArguments(List<Node> arguments, LispScope scope)
    {
        var newScope = scope.PushScope();

        // Process positional parameters
        int i = 0;
        for (; i < PositionalParameters.Count; i++)
        {
            var value = Runner.EvaluateNode(arguments[i], scope);
            if (value is not LispValue lispValue) throw Report.Error(new WrongArgumentTypeReportMessage($"{value} is not a valid value"), arguments[i].Location);
            
            newScope.UpdateScope(PositionalParameters[i].Text, lispValue);
        }
        
        // Process named parameters
        for (; i < NamedParameters.Count && arguments[i] is KeyValueNode keyValue; i++)
        {
            var value = Runner.EvaluateNode(keyValue, scope); 
            if (value is not LispValue lispValue) throw Report.Error(new WrongArgumentTypeReportMessage($"{value} is not a valid value"), keyValue.Location);
            
            newScope.UpdateScope(keyValue.Key.Text, lispValue);
        }

        foreach (var namedParameter in NamedParameters)
        {
            var key = namedParameter.Key.Text;
            if (!newScope.HasOwnValue(key))
            {
                newScope.UpdateScope(key, LispNilValue.Instance);
            }
        }

        if (i == arguments.Count) return newScope;
        if (RestParameter is null) throw Report.Error(new WrongArgumentCountReportMessage(PositionalParameters, arguments.Count));
        var restValue = new LispListValue();
        for (; i < arguments.Count; i++)
        {
            var value = Runner.EvaluateNode(arguments[i], scope);
            if (value is not LispValue lispValue) throw Report.Error(new WrongArgumentTypeReportMessage($"{value} is not a valid value"), arguments[i].Location);
            restValue.Value.Add(lispValue);
        }
        newScope.UpdateScope(RestParameter.Text, restValue);
        
        return newScope;
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

    protected override bool Equals(BaseLispValue other)
    {
        if (other is not LispFunctionValue function) return false;
        if (!function.Parameters.Equals(Parameters)) return false;
        if (!function.Definition.Equals(Definition)) return false;
        
        return true;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        
        hash.Add(Parameters);
        hash.Add(Definition);
        
        return hash.ToHashCode();
    }
}