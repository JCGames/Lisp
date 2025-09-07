namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_2_5_Divisors
{
    [TestMethod]
    public void TestGreatestCommonDivisor()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def gcd (lambda (a b)
                (if (= b 0)
                    a
                    (gcd b (% a b)))))
                    
            (print (gcd 206 40))
            """);

        Assert.AreEqual("2\n", output);
    }
    
    [TestMethod]
    public void TestIsPime()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def square (lambda (x) (* x x)))
            (def smallest-divisor (lambda (n)
                (find-divisor n 2)))
            (def find-divisor (lambda (n test-divisor)
                (switch
                    ((> (square test-divisor) n) n)
                    ((divides? test-divisor n) test-divisor)
                    (else (find-divisor n (inc test-divisor))))))
            (def divides? (lambda (a b)
                (= (% b a) 0)))
                
            (def prime? (lambda (n)
                (= n (smallest-divisor n))))
            
            (print (prime? 7))
            (print (prime? 6))
            (print (prime? 59141))
            (print (prime? 59142))
            (print (prime? 59143))
            (print (prime? 101159))
            """);

        Assert.AreEqual("True\nFalse\nTrue\nFalse\nFalse\nTrue\n", output);
    }
}