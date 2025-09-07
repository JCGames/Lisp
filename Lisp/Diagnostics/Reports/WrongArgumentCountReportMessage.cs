using System.Diagnostics.CodeAnalysis;
using Lisp.Parsing.Nodes;
using Lisp.Parsing.Nodes.Classifications;

namespace Lisp.Exceptions;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public class WrongArgumentCountReportMessage : ReportMessage
{
    public WrongArgumentCountReportMessage(IEnumerable<IParameterNode> expectedArguments, int passedCount, int? requiredCount = null)
        : base($"{requiredCount ?? expectedArguments.Count()} arguments were required ({string.Join(", ", expectedArguments.Select(x => x.PublicParameterName))}), but {passedCount} was passed.")
    { }
}