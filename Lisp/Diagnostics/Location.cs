using Lisp.Parsing;

namespace Lisp.Diagnostics;

public class Location
{
    public required SourceFile? SourceFile { get; set; }
    public required int Line { get; set; }
    public required int Position { get; set; }
}