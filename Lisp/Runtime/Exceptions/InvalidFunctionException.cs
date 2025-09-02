namespace Lisp.Exceptions;

public class InvalidFunctionException : LispException
{
    public InvalidFunctionException(string message) : base(message)
    { }
}