namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_2_2_TreeRecursion
{
    [TestMethod]
    public void TestNaiveFibonacci()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def fib (lambda (n)
                (switch ((= n 0) 0)
                      ((= n 1) 1)
                      (else (+ (fib (- n 1))
                               (fib (- n 2)))))))
                              
            (print (fib 5))
            (print (fib 10))
            (print (fib 15))
            (print (fib 20))
            (print (fib 25))
            """);

        Assert.AreEqual("5\n55\n610\n6765\n75025\n", output);
    }
    
    [TestMethod]
    public void TestFibonacciTailCalOptimizable()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def fib (lambda (n)
                (let iter (lambda (a b count)
                    (if (= count 0)
                        b
                        (iter (+ a b)
                              a
                              (- count 1)))))
                (iter 1 0 n)))

            ; much faster
            (print (fib 5))
            (print (fib 10))
            (print (fib 15))
            (print (fib 20))
            (print (fib 25))
            (print (fib 30))
            (print (fib 35))
            (print (fib 40))
            (print (fib 45))
            (print (fib 50))
            (print (fib 100))
            """);

        Assert.AreEqual("5\n55\n610\n6765\n75025\n832040\n9227465\n102334155\n1134903170\n12586269025\n354224848179261915075\n", output);
    }
}