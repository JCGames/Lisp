using Lisp.Parsing;

namespace TurboTests;

[TestClass]
public sealed class SourceFileTests
{
    [TestMethod]
    public void TestSourceFile()
    {
        var sourceFile = new SourceFile(new FileInfo("TestSourceFile.lisp"));

        while (!sourceFile.EndOfFile)
        {
            var c = sourceFile.ReadChar();
            Console.WriteLine($"|{c}| Line: {sourceFile.CurrentLine}, Position: {sourceFile.CurrentPosition}");
        }
    }
}