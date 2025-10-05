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
            sourceFile.MoveNext();
            Console.WriteLine($"|{(sourceFile.IsNewLine ? "\\n" : sourceFile.Current)}| Line: {sourceFile.CurrentLine}, Position: {sourceFile.CurrentPosition}");
        }
    }

    [TestMethod]
    public void TestGetLineSpan()
    {
        var sourceFile = new SourceFile(new FileInfo("TestSourceFile.lisp"));

        Assert.AreEqual("Line 1", sourceFile.GetLineSpan(1).ToString());
        Assert.AreEqual("Line 2", sourceFile.GetLineSpan(2).ToString());
        Assert.AreEqual("Help me here", sourceFile.GetLineSpan(3).ToString());
    }

    [TestMethod]
    public void TestGetSpan()
    {
        var sourceFile = new SourceFile(new FileInfo("TestSourceFile.lisp"));

        Assert.AreEqual("Line 1\r\nLine 2\r\n", sourceFile.GetSpan(0, 15).ToString());
    }

    [TestMethod]
    public void TestGetStartAndEndOfLine()
    {
        var sourceFile = new SourceFile(new FileInfo("TestSourceFile.lisp"));

        var result = sourceFile.GetStartAndEndOfLine(1);
        
        Assert.AreEqual(0, result.start);
        Assert.AreEqual(5, result.end);
    }

    [TestMethod]
    public void TestMoveToNextLine()
    {
        var sourceFile = new SourceFile(new FileInfo("TestSourceFile.lisp"));
        sourceFile.MoveToNextLine();
        Assert.AreEqual(2, sourceFile.CurrentLine);
    }
}