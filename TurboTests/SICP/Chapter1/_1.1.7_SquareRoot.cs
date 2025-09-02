namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_1_7_SquareRoot
{
    // TODO: This is broken, but it should work.
    // `(sqrt (+ (sqrt 2) (sqrt 3)))` is the longest and should take only 12 steps
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
                    
            (def square (lambda (x) (* x x)))
                
            (def sqrt-iter (lambda (guess x)
                (if (good-enough? guess x)
                    guess
                    (sqrt-iter (improve guess x) x))))
                    
            (def improve (lambda (guess x)
                (average guess (/ x guess))))
                
            (def average (lambda (x y)
                (/ (+ x y) 2)))
                
            (def good-enough? (lambda (guess x)
                (< (abs (- (square guess) x)) 100)))
                
            (def sqrt (lambda (x)
                (sqrt-iter 1.0 x)))
                
            (print (sqrt 9))
            (print (sqrt (+ 100 37)))
            (print (sqrt (+ (sqrt 2) (sqrt 3))))
            (print (square (sqrt 1000)))
            """);

        // Assert.AreEqual("5\n0\n5\n", output);
    }
}