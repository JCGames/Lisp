namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_1_1_Expressions
{
    [TestMethod]
    public void WriteNumber()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print 486)
            """);

        Assert.AreEqual("486\n", output);
    }

    [TestMethod]
    public void BasicAddition()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (+ 137 349))
            """);

        Assert.AreEqual("486\n", output);
    }

    [TestMethod]
    public void BasicSubtraction()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (- 1000 334))
            """);

        Assert.AreEqual("666\n", output);
    }
    
    [TestMethod]
    public void BasicMultiplication()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (* 5 99))
            """);

        Assert.AreEqual("495\n", output);
    }
    
    [TestMethod]
    public void BasicDivision()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (/ 10 5))
            """);

        Assert.AreEqual("2\n", output);
    }
    
    [TestMethod]
    public void FractionalAddition()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (+ 2.7 10))
            """);

        Assert.AreEqual("12.7\n", output);
    }

    [TestMethod]
    public void MultipleAddition()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (+ 21 35 12 7))
            """);

        Assert.AreEqual("75\n", output);
    }
    
    [TestMethod]
    public void MultipleMultiplication()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (* 25 4 12))
            """);

        Assert.AreEqual("1200\n", output);
    }

    [TestMethod]
    public void CombineOperators1()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (+ (* 3 5) (- 10 6)))
            """);

        Assert.AreEqual("19\n", output);
    }
    
    [TestMethod]
    public void CombineOperators2()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (print (+ (* 3
                         (+ (* 2 4)
                            (+ 3 5)))
                      (+ (- 10 7)
                         6)))
            """);

        Assert.AreEqual("57\n", output);
    }
}