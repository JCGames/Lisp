using Lisp;

var sourceFile = new SourceFile(new FileInfo("main.txt"));
var parser = new Parser(sourceFile);

var list = parser.Parse();

// foreach (var lispList in list)
// {
//     lispList.Print("");
// }

var runner = new Runner();

runner.Run(list);