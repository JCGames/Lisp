namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_1_3_Evaluating
{
    [TestMethod]
    public void Example()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (* (+ 2 (* 4 6))
                      (+ 3 5 7)))
            """);

        Assert.AreEqual("390\n", output);
    }
}