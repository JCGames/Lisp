using Lisp.Parsing.Nodes;

namespace Lisp.Exceptions;

public class WrongArgumentCountReportMessage : ReportMessage
{
    public WrongArgumentCountReportMessage(List<IdentifierNode> expectedArguments, int passedCount, int? requiredCount = null)
        : base($"{requiredCount ?? expectedArguments.Count} arguments were required ({string.Join(", ", expectedArguments.Select(x => x.Text))}), but {passedCount} was passed.")
    { }
}