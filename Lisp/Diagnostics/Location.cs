using Lisp.Parsing;

namespace Lisp.Diagnostics;

public class Location
{
    public static Location None => new()
    {
        Line = -1,
        SourceFile = null,
        Position = -1
    };
    
    public required SourceFile? SourceFile { get; set; }
    public required int Line { get; set; }
    public required int Position { get; set; }
}