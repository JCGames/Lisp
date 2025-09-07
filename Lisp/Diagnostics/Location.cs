using Lisp.Parsing;

namespace Lisp.Diagnostics;

public class Location
{
    public static Location None => new()
    {
        Line = -1,
        SourceFile = null,
        Start = -1,
        End = -1
    };

    public static Location New(SourceFile sourceFile) => new()
    {
        SourceFile = sourceFile,
        Line = sourceFile.CurrentLine,
        Start = sourceFile.CurrentPosition > 0 ? sourceFile.CurrentPosition - 1 : sourceFile.CurrentPosition,
        End = sourceFile.CurrentPosition
    };
    
    public required SourceFile? SourceFile { get; set; }
    public required int Line { get; set; }
    public required int Start { get; set; }
    public required int End { get; set; }
}