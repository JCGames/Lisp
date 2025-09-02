namespace Lisp.Exceptions;

public class NotAFunctionReportMessage : ReportMessage
{
    public NotAFunctionReportMessage() : base("This is not a function!")
    { }
}