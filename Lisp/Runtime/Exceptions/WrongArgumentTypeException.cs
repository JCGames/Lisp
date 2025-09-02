namespace Lisp.Exceptions;

public class WrongArgumentTypeException : LispException
{
    public WrongArgumentTypeException(string message) : base(message)
    { }
}