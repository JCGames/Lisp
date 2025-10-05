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
    
    public required SourceFile? SourceFile { get; set; }
    public required int Line { get; set; }
    public required int Start { get; set; }
    public required int End { get; set; }
    
    public static Location New(SourceFile sourceFile) => new()
    {
        SourceFile = sourceFile,
        Line = sourceFile.CurrentLine,
        Start = sourceFile.CurrentPosition,
        End = sourceFile.CurrentPosition
    };

    public static Location Combine(Location a, Location b)
    {
        if (a.SourceFile != b.SourceFile)
            throw new InvalidOperationException("Cannot combine locations because their source files do not match.");
        
        var result = new Location
        {
            Line = a.Line,
            Start = a.Start,
            End = b.End,
            SourceFile = a.SourceFile
        };
        
        if (b.Line < a.Line)
        {
            result.Line = b.Line;
        }

        if (a.Start > b.End)
        {
            result.Start = b.Start;
            result.End = a.End;
        }
        
        return result;
    }
}