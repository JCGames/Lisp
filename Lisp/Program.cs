using Lisp;

var standardLibrary = new SourceFile(new FileInfo("Runtime/StandardLibrary/preamble.lisp"));
var sourceFile = new SourceFile(new FileInfo("main.txt"));

var parserStandardLibrary = new Parser(standardLibrary);
var parser = new Parser(sourceFile);

var standardLibraryList = parserStandardLibrary.Parse();
var list = parser.Parse();

Runner.Run([..standardLibraryList, ..list]);