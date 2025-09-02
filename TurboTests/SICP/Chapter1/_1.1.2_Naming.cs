namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_1_2_Naming
{
    [TestMethod]
    public void BasicDefinition()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def size 2)
            (print size)
            """);

        Assert.AreEqual("2\n", output);
    }
    
    [TestMethod]
    public void UseDefinedValue()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def size 2)
            (print (* 5 size))
            """);

        Assert.AreEqual("10\n", output);
    }
    
    [TestMethod]
    public void CircleExample()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def pi 3.14159)
            (def radius 10)
            (print (* pi (* radius radius)))
            (def circumference (* 2 pi radius))
            (print circumference)
            """);
    
        Assert.AreEqual("314.159\n62.8318\n", output);
    }

}