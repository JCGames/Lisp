namespace Lisp.Exceptions;

public class ReportMessage
{
    public string Message { get; }

    public ReportMessage(string message)
    {
        Message = message;
    }
}