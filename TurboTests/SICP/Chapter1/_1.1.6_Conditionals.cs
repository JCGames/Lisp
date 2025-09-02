namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_1_6_Conditionals
{
    [TestMethod]
    public void TestConditional()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def abs (lambda (x)
                (switch
                    ((> x 0) x)
                    ((= x 0) 0)
                    ((< x 0) (- x)))))
                    
            (print (abs 5))
            (print (abs 0))
            (print (abs -5))
            """);

        Assert.AreEqual("5\n0\n5\n", output);
    }
    
    [TestMethod]
    public void TestConditionalSimplified()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def abs (lambda (x)
                (switch
                    ((< x 0) (- x))
                    (else x))))
                    
            (print (abs 5))
            (print (abs 0))
            (print (abs -5))
            """);

        Assert.AreEqual("5\n0\n5\n", output);
    }
    
    [TestMethod]
    public void TestConditionalIf()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def abs (lambda (x)
                (if (< x 0)
                    (- x)
                    x)))
                    
            (print (abs 5))
            (print (abs 0))
            (print (abs -5))
            """);

        Assert.AreEqual("5\n0\n5\n", output);
    }


    
}