using Lisp.Diagnostics;
using Lisp.Parsing;

namespace TurboTests;

[TestClass]
public class ParserTests
{
    [TestInitialize]
    public void InitMethod()
    {
        Report.PreferThrownErrors = true;
    }
    
    [Ignore]
    [TestMethod]
    public void TestStringLiteralParse()
    {
        var parser = new Parser(new SourceFile(
            """
            ("Hello, world")
            """
        ));
        
        var result = parser.Parse();

        result.First().Print("", Console.Out);
    }
}