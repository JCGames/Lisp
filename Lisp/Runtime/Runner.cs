using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.Security;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Turbo;
using Lisp.Types;

namespace Lisp;

public static class Runner
{
    public static void Run(List<ListNode> list)
    {
        var scope = new LispScope();
        InitializeGlobalScope(scope);
        
        foreach (var lispList in list)
        {
            ExecuteList(lispList, scope);
        }
    }

    public static BaseLispValue EvaluateNode(Node node, LispScope scope) => node switch
    {
        ListNode list => ExecuteList(list, scope),
        TokenNode token => token.Type switch
        {
            TokenType.Identifier => scope.Read(token.Text),
            TokenType.Decimal => new LispNumberValue(decimal.Parse(token.Text.Replace(",", ""))),
            TokenType.Integer => new LispNumberValue(int.Parse(token.Text.Replace(",", ""))),
            TokenType.StringLiteral => new LispStringValue(token.Text),
            // TokenType.Boolean => new LispBooleanValue(bool.Parse(token.Text)),
            _ => throw new NotImplementedException("Unknown token type."),
        },
        _ => throw new NotImplementedException("Unknown node type."),
    };

    private static BaseLispValue ExecuteList(ListNode list, LispScope scope)
    {
        if (list.Nodes.Count == 0) throw new InvalidOperationException("Cannot execute an empty list.");

        var function = EvaluateNode(list.Nodes[0], scope);

        if (function is not IExecutableLispValue executable) throw new NotAFunctionException();
        
        return executable.Execute(list.Nodes[1..], scope);
    }
    
    private static void InitializeGlobalScope(LispScope scope)
    {
        var turboFunctions = typeof(ITurboFunction).Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ITurboFunction)))
            .Where(t => t != typeof(ITurboFunction));

        foreach (var turboFunction in turboFunctions)
        {
            var name = $"Turbo.{turboFunction.Name}";
            var value = new LispTurboFunctionValue((ITurboFunction)Activator.CreateInstance(turboFunction)!);
            
            scope.UpdateGlobalScope(name, value);
        }
        
        scope.UpdateGlobalScope("true", new LispBooleanValue(true));
        scope.UpdateGlobalScope("false", new LispBooleanValue(false));
    }
}