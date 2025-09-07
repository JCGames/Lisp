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
    
    [TestMethod]
    public void TestCountChange()
    {
        // Counts the number of ways that a given amount of change can be made.
        // i.e. 1 change can only be a penny, 5 change can be one nickel or 5 pennies (2 ways)
        // lang=clojure
        var output = TestTools.Run(
            """
            
            (def count-change (lambda (amount)
                 (cc amount 5)))
                 
            (def cc (lambda (amount kinds-of-coins)
                 (switch 
                    ((= amount 0) 1)
                    ((or (< amount 0)
                         (= kinds-of-coins 0)) 0)
                    (else (+ (cc amount
                                 (- kinds-of-coins 1))
                             (cc (- amount
                                    (first-denomination kinds-of-coins))
                                    kinds-of-coins))))))
                                    
            (def first-denomination (lambda (kinds-of-coins)
                 (switch ((= kinds-of-coins 1) 1)
                       ((= kinds-of-coins 2) 5)
                       ((= kinds-of-coins 3) 10)
                       ((= kinds-of-coins 4) 25)
                       ((= kinds-of-coins 5) 50))))
                       
            
            (print (count-change 100))
            """);

        Assert.AreEqual("292\n", output);
    }
    
    [TestMethod]
    public void TestFNaive()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def f (lambda (n)
                (if (< n 3)
                    n
                    (+ (f (dec n))
                       (* 2 (f (- n 2)))
                       (* 3 (f (- n 3)))))))
                       
            (print (f 10))
            (print (f 15))
            (print (f 20))
            """);

        Assert.AreEqual("1892\n142717\n10771211\n", output);
    }
    
    [TestMethod]
    public void TestFTailCallOptimized()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def f (lambda (n)
                (def iter (lambda (n a b c count)
                    (if (= count n)
                        c
                        (iter 
                            n b c 
                            (+ (* 3 a)
                               (* 2 b)
                               c)
                            (inc count)))))
                
                (iter n 0 1 2 2)))
                
            (print (f 10))
            (print (f 15))
            (print (f 20))
            """);

        Assert.AreEqual("1892\n142717\n10771211\n", output);
    }
}