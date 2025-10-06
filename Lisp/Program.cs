using Lisp;
using Lisp.Diagnostics;
using Lisp.Parsing;

var standardLibrary = new SourceFile(new FileInfo("Runtime/StandardLibrary/prelude.lisp"));
var sourceFile = new SourceFile(new FileInfo("main.txt"));

var parserStandardLibrary = new Parser(standardLibrary);
var parser = new Parser(sourceFile);

var standardLibraryList = parserStandardLibrary.Parse();
var list = parser.Parse();

foreach (var listNode in list)
{
    listNode.Print("", Console.Out);
}

Runner.Run([..standardLibraryList, ..list]);
