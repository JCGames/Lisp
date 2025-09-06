namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_2_4_Exponentiation
{
    [TestMethod]
    public void TestNaiveExponentiation()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def exp (lambda (b n)
                (if (= n 0)
                    1
                    (* b (exp b (dec n))))))
                    
            (print (exp 5 5))
            """);

        Assert.AreEqual("3125\n", output);
    }
    
    [TestMethod]
    public void TestTailCallOptimizedExponentiation()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def exp (lambda (b n)
                (def iter (lambda (counter product)
                    (if (= counter 0)
                        product
                        (iter (- counter 1)
                              (* b product)))))
                (iter n 1)))
                    
            (print (exp 5 5))
            """);

        Assert.AreEqual("3125\n", output);
    }
    
    [TestMethod]
    public void TestLogarithmicExponentiation()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def square (lambda (x) (* x x)))
            (def even? (lambda (x) (= (% n 2) 0)))

            (def exp (lambda (b n)
                (switch
                    ((= n 0) 1)
                    ((even? n) (square (exp b (/ n 2))))
                    (else (* b (exp b (- n 1)))))))
                    
            (print (exp 5 5))
            """);

        Assert.AreEqual("3125\n", output);
    }
    
    [TestMethod]
    public void TestLogarithmicTailCallOptimizedExponentiation()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def square (lambda (x) (* x x)))
            (def even? (lambda (x) (= (% n 2) 0)))

            (def exp (lambda (b n)
                (switch
                    ((= n 0) 1)
                    ((even? n) (exp (square b) (/ n 2)))
                    ; This one will only ever leave one frame on the stack
                    (else (* b (exp b (- n 1)))))))
                    
            (print (exp 5 5))
            """);

        Assert.AreEqual("3125\n", output);
    }
}