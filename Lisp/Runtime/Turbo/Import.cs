using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Parsing;
using Lisp.Parsing.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class Import : ITurboFunction
{
    private static readonly List<IdentifierNode> ArgumentDeclaration =
    [
        new()
        {
            Text = "path",
            Location = Location.None
        }
    ];

    public List<IdentifierNode> Arguments => ArgumentDeclaration;
    
    public BaseLispValue Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 1) Report.Error(new WrongArgumentCountReportMessage(ArgumentDeclaration, parameters.Count, 1));
        
        var result = Runner.EvaluateNode(parameters[0], scope);
        
        if (result is not LispStringValue str) throw Report.Error(new WrongArgumentTypeReportMessage("Import expects a string for the path."));
        if (parameters[0] is not TokenNode token) throw Report.Error(new WrongArgumentTypeReportMessage("Import only accepts a token as its argument."));
        
        var path = Path.Join(token.Location.SourceFile?.FileInfo?.DirectoryName ?? Directory.GetCurrentDirectory(), str.Value);
        
        var sourceFile = new SourceFile(new FileInfo(path));
        var parser = new Parser(sourceFile);
        var lispListList = parser.Parse();

        foreach (var lispList in lispListList)
        {
            Runner.EvaluateNode(lispList, scope);
        }
        
        return new LispVoidValue();
    }
}