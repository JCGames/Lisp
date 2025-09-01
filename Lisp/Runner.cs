using System.Collections.Immutable;
using System.Diagnostics;
using Lisp.Nodes;

namespace Lisp;

public class Runner
{
    private readonly Dictionary<string, object?> _global = [];
    
    public void Run(List<LispList> list)
    {
        foreach (var lispList in list)
        {
            ExecuteList(lispList, ImmutableDictionary<string, object?>.Empty);
        }
    }

    private object? ExecuteList(LispList list, ImmutableDictionary<string, object?> scope)
    {
        if (list.Nodes.Count == 0) throw new InvalidOperationException("Cannot execute and empty list.");
        
        if (list.Nodes[0] is LispList functionList)
        {
            // expect a function to be returned from this
            var func = ExecuteList(functionList, scope);
        }
        else if (list.Nodes[0] is Token token)
        {
            if (token.Text == "define")
            {
                if (list.Nodes[1] is Token variableToken)
                {
                    _global.Add(variableToken.Text, EvaluateNode(list.Nodes[2], scope));
                    return null;
                }
                
                var definition = list.Nodes[1] as LispList ?? new LispList();
                var body = new List<LispList>();

                for (var i = 2; i < list.Nodes.Count; i++)
                {
                    body.Add(list.Nodes[i] as LispList ?? new LispList());
                }

                var functionName = definition.Nodes[0] as Token ?? throw new InvalidOperationException("Function name cannot be null.");
                
                _global.Add(functionName.Text, new Function
                {
                    Definition = definition,
                    Body = body
                });
            }
            else if (_global.TryGetValue(token.Text, out var value) && value is Function function)
            {
                var numArgs = function.Definition.Nodes.Count - 1;
                
                var args = new List<object?>();
        
                if (list.Nodes.Count > 1)
                {
                    for (var i = 1; i < list.Nodes.Count; i++)
                    {
                        args.Add(EvaluateNode(list.Nodes[i], scope));
                    }
                }

                if (args.Count < numArgs)
                {
                    throw new InvalidOperationException("Function arguments must have the same number of arguments.");
                }

                for (var i = 1; i < function.Definition.Nodes.Count; i++)
                {
                    var name = function.Definition.Nodes[i] as Token ?? throw new InvalidOperationException("Function arg must be token.");
                    scope = scope.Add(name.Text, args[i - 1]);
                }

                var val = new object();

                foreach (var lispList in function.Body)
                {
                    val = ExecuteList(lispList, scope);
                }

                return val;
            }
            else if (token.Text is "print")
            {
                var output = string.Empty;
                
                for (var i = 1; i < list.Nodes.Count; i++)
                {
                    output += EvaluateNode(list.Nodes[i], scope);
                }
                
                Console.WriteLine(output);
            }
        }
        
        return null;
    }

    private object? EvaluateNode(Node node, ImmutableDictionary<string, object?> scope)
    {
        switch (node)
        {
            case Token token:

                switch (token.Type)
                {
                    case TokenType.Decimal:
                        return decimal.Parse(token.Text.Replace(",", ""));
                    case TokenType.Integer:
                        return int.Parse(token.Text.Replace(",", ""));
                    case TokenType.Identifier:
                        return _global[token.Text];
                    case TokenType.StringLiteral:
                        return token.Text;
                }

                break;
            case LispList list:
                return ExecuteList(list, scope);
        }

        return null;
    }
}