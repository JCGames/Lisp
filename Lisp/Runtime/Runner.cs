using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.Security;
using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing.Nodes;
using Lisp.Turbo;
using Lisp.Types;

namespace Lisp;

public static class Runner
{
    public static TextWriter StdOut = Console.Out;
    public static TextReader StdIn = Console.In;
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
        IdentifierNode identifier => scope.Read(identifier.Text) ?? throw Report.Error($"{identifier.Text} is undefined.", node.Location),
        NumberLiteralNode number => new LispNumberValue(number.Value),
        StringLiteralNode stringLiteral => new LispStringValue(stringLiteral.Text),
        _ => throw new NotImplementedException("Unknown token type.")
    };

    private static BaseLispValue ExecuteList(ListNode listNode, LispScope scope)
    {
        if (listNode.Nodes.Count == 0) throw new InvalidOperationException("Cannot execute an empty list.");

        var function = EvaluateNode(listNode.Nodes[0], scope);

        if (function is not IExecutableLispValue executable) throw Report.Error(new NotAFunctionReportMessage(), listNode.Location);
        
        return executable.Execute(listNode.Nodes[1..], scope);
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