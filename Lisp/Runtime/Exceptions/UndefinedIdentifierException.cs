namespace Lisp.Exceptions;

public class UndefinedIdentifierException : LispException
{
    public UndefinedIdentifierException(string identifier) : base($"`{identifier}` is not defined.")
    { }
}