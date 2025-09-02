using Lisp.Exceptions;
using Lisp.Nodes;
using Lisp.Types;

namespace Lisp.Turbo;

public class Import : ITurboFunction
{
    private static readonly List<Token> ArgumentDeclaration =
    [
        new()
        {
            Text = "path",
            Type = TokenType.Identifier,
            Line = -1
        }
    ];

    public List<Token> Arguments => ArgumentDeclaration;
    
    BaseLispValue ITurboFunction.Execute(List<Node> parameters, LispScope scope)
    {
        if (parameters.Count < 1) throw new WrongArgumentCountException(ArgumentDeclaration, parameters.Count, 1);
        
        var result = Runner.EvaluateNode(parameters[0], scope);
        
        if (result is not LispStringValue str) throw new WrongArgumentTypeException("Import expects a string for the path.");

        if (parameters[0] is not Token token) throw new WrongArgumentTypeException("Import only accepts a token as its argument.");
        
        var path = Path.Join(token.FileInfo?.DirectoryName ?? Directory.GetCurrentDirectory(), token.Text);
        
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