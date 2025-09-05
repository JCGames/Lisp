namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_2_1_LinearRecursion
{
    [TestMethod]
    public void TestNaiveFactorial()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def factorial (lambda (n)
              (if (= n 1)
                  1
                  (* n (factorial (- n 1))))))
                  
            (print (factorial 6))
            (print (factorial 15))
            (print (factorial 20))
            (print (factorial 25))
            """);

        Assert.AreEqual("720\n1307674368000\n2432902008176640000\n15511210043330985984000000\n", output);
    }
    
    [TestMethod]
    public void TestFactorialTailCallOptimizable()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def factorial (lambda (n)
              (factorial-iterator 1 1 n)))
              
            (def factorial-iterator (lambda (product counter max-count)
                (if (> counter max-count)
                    product
                    (factorial-iterator (* counter product)
                                        (+ counter 1)
                                        max-count))))
                  
            (print (factorial 6))
            (print (factorial 15))
            (print (factorial 20))
            (print (factorial 25))
            """);

        Assert.AreEqual("720\n1307674368000\n2432902008176640000\n15511210043330985984000000\n", output);
    }
    
    [TestMethod]
    public void TestFactorialTailCallOptimizableClean()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def factorial (lambda (n)
                (let iter (lambda (product counter)
                    (if (> counter n)
                        product
                        (iter (* counter product)
                              (+ counter 1)))))
                (iter 1 1)))
              
            (print (factorial 6))
            (print (factorial 15))
            (print (factorial 20))
            (print (factorial 25))
            """);

        Assert.AreEqual("720\n1307674368000\n2432902008176640000\n15511210043330985984000000\n", output);
    }
}