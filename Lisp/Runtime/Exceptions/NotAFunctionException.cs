namespace Lisp.Exceptions;

public class NotAFunctionException : LispException
{
    public NotAFunctionException() : base("This is not a function!")
    { }
}