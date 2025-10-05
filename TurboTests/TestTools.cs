using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Lisp;
using Lisp.Diagnostics;
using Lisp.Parsing;

namespace TurboTests;

public static class TestTools
{
    public static string Run(string programText)
    {
        var writer = new StringWriter();
        writer.NewLine = "\n";
        Runner.StdOut = writer;
        Report.PreferThrownErrors = true;
        
        var standardLibraryFile = new SourceFile(new FileInfo("Runtime/StandardLibrary/prelude.lisp"));
        var sourceFile = new SourceFile(programText);
        
        var parserStandardLibrary = new Parser(standardLibraryFile);
        var parser = new Parser(sourceFile);
        
        var standardLibrary = parserStandardLibrary.Parse();
        var program = parser.Parse();
        
        Runner.Run([..standardLibrary, ..program]);
        
        return writer.ToString();
    }
}