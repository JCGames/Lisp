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
    
    [TestMethod]
    public void TestSquareRootEstimationDynamicEndPoint()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def abs (lambda (x)
                (if (< x 0)
                    (- x)
                    x)))
                    
            (def square (lambda (x) (* x x)))
                
            (def sqrt-iter (lambda (guess x last-guess)
                (if (good-enough? guess last-guess)
                    guess
                    (sqrt-iter (improve guess x) x guess))))
                    
            (def improve (lambda (guess x)
                (average guess (/ x guess))))
                
            (def average (lambda (x y)
                (/ (+ x y) 2)))
                
            (def good-enough? (lambda (guess last-guess)
                (< (abs (- guess last-guess)) 0.0000000000001)))
                
            (def sqrt (lambda (x)
                (sqrt-iter 1.0 x x)))
                
            (print (sqrt 9))
            (print (sqrt (+ 100 37)))
            (print (sqrt (+ (sqrt 2) (sqrt 3))))
            (print (square (sqrt 1000)))
            """);

        Assert.AreEqual("3\n11.7046999107\n1.7737712282\n1000\n", output);
    }
    
    
    [TestMethod]
    public void TestCubeRootEstimation()
    {
        // lang=clojure
        var output = TestTools.Run(
            """
            (def abs (lambda (x)
                (if (< x 0)
                    (- x)
                    x)))

            (def square (lambda (x) (* x x)))
            (def cube (lambda (x) (* x x x)))

            (def cube-root-iter (lambda (guess x last-guess)
                (if (good-enough? guess last-guess)
                    guess
                    (cube-root-iter (improve guess x) x guess))))

            (def improve (lambda (guess x)
                (/ (+ (/ x (square guess))
                      (* 2 guess))
                   3)))


            (def good-enough? (lambda (guess last-guess)
                (< (abs (- guess last-guess)) 0.0000000000001)))

            (def cube-root (lambda (x)
                (cube-root-iter 1.0 x x)))

            (print (cube-root 27))
            (print (cube-root (+ 100 37)))
            (print (cube-root (+ (cube-root 2) (cube-root 3))))
            (print (cube (cube-root 1000)))
            """);

        Assert.AreEqual("3\n5.1551367355\n1.392849703\n1000\n", output);
    }

}