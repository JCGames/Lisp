namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_1_4_Procedures
{
    [TestMethod]
    public void SimpleProcedure()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def square (lambda (x)
                (* x x)))
            (print (square 21))
            """);

        Assert.AreEqual("441\n", output);
    }
    
    [TestMethod]
    public void ProcedureArguments1()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def square (lambda (x)
                (* x x)))
            (print (square (+ 2 5)))
            """);

        Assert.AreEqual("49\n", output);
    }
    
    [TestMethod]
    public void ProcedureArguments2()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def square (lambda (x)
                (* x x)))
            (print (square (square 3)))
            """);

        Assert.AreEqual("81\n", output);
    }
    
    [TestMethod]
    public void CompoundProcedures1()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def square (lambda (x)
                (* x x)))
                
            (def sum-of-squares (lambda (x y)
                (+ (square x) (square y))))
                
            (print (sum-of-squares 3 4))
            """);

        Assert.AreEqual("25\n", output);
    }
    
    [TestMethod]
    public void CompoundProcedures2()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def square (lambda (x)
                (* x x)))
                
            (def sum-of-squares (lambda (x y)
                (+ (square x) (square y))))
                
            (def f (lambda (a)
                (sum-of-squares (+ a 1) (* a 2))))
                
            (print (f 5))
            """);

        Assert.AreEqual("136\n", output);
    }
}