namespace TurboTests.SICP.Chapter1;

[TestClass]
public class _1_1_7_SquareRoot
{
    [TestMethod]
    public void TestSquareRootEstimation()
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
                (< (abs (- (square guess) x)) 0.001)))
                
            (def sqrt (lambda (x)
                (sqrt-iter 1.0 x)))
                
            (print (sqrt 9))
            (print (sqrt (+ 100 37)))
            (print (sqrt (+ (sqrt 2) (sqrt 3))))
            (print (square (sqrt 1000)))
            """);

        Assert.AreEqual("3.0000915541\n11.7046999178\n1.7739279023\n1000.0003699244\n", output);
    }
}