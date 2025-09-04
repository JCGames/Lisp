using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class DebugRepl : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration = [ ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count > 0) throw Report.Error(new WrongArgumentCountReportMessage(ArgumentDeclaration, parameters.Count));

        Report.PreferThrownErrors = true;
        
        Runner.StdOut.WriteLine("Welcome to the interactive debugger. Your state is right where you left it.");
        Runner.StdOut.WriteLine("Currently, only one line of input at a time is allowed.");
        Runner.StdOut.WriteLine(":h for help.");
        var exit = false;
        while (!exit)
        {
            try
            {
                Runner.StdOut.Write("> ");
                var line = Runner.StdIn.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    line = ":h";
                }

                if (line.StartsWith(":"))
                {
                    exit = HandleReplCommand(line, scope);
                }

                var parser = new Parser(new(line));
                var command = parser.Parse();
                BaseLispValue value = LispVoidValue.Instance;
                foreach (var node in command)
                {
                    value = Runner.EvaluateNode(node, scope);
                }

                if (value is LispValue lispValue)
                {
                    Runner.StdOut.WriteLine(lispValue);
                }
            }
            catch (Exception)
            {
                // Console.WriteLine(ex.Message);
            }
        }
        
        return LispVoidValue.Instance;
    }
    
    private bool HandleReplCommand(string command, LispScope scope)
    {
        switch (command)
        {
            case ":h":
                Runner.StdOut.WriteLine(
                    """
                    :h - print this help
                    :q - quit the repl
                    """);
                break;
            case ":q":
                return true;
            default:
                Runner.StdOut.WriteLine($"Unrecognized command {command} (:h for help)");
                break;
        }
        
        return false;
    }
}