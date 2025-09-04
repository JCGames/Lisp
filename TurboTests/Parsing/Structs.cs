using Lisp.Parsing;

namespace TurboTests.Parsing;

[TestClass]
public class Structs
{
    [TestMethod]
    public void NestedStructParsesSuccessfully()
    {
        var parser = new Parser(new SourceFile(
            """
            (def struct {
                a: "a"
                b: "b"
                test: (lambda (data) (print data))
                another-test: {
                    just-one-more-test: (lambda () (print "ok, this is crazy"))}})
            """));
        
        var result = parser.Parse();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
    }
}