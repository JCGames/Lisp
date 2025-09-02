namespace Lisp.Exceptions;

public class UndefinedIdentifierReportMessage : ReportMessage
{
    public UndefinedIdentifierReportMessage(string identifier) : base($"`{identifier}` is not defined.")
    { }
}