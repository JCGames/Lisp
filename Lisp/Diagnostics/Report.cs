using System.Diagnostics.CodeAnalysis;
using Lisp.Exceptions;

namespace Lisp.Diagnostics;

public static class Report
{
    private static List<string> _errors = [];

    [DoesNotReturn]
    public static Exception Error(ReportMessage reportMessage, Location? location = null)
    {
        return Error(reportMessage.Message, location);
    }
    
    [DoesNotReturn]
    public static Exception Error(string message, Location? location = null)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        
        if (location?.SourceFile is not null)
        {
            var (start, end) = location.SourceFile.GetStartAndEndOfLine(location.Line);
            
            Console.WriteLine($"{location.SourceFile.FileInfo?.FullName}:({location.Line}:{location.Position - start})");
            Console.WriteLine($"\t{message}");

            Console.WriteLine();
            Console.WriteLine($"\t{location.SourceFile.Text[start..end].Trim()}");
            
            Console.Write("\t");
            for (var i = 0; i < location.Position - start - 1; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("^");
        }
        else
        {
            Console.WriteLine($"\t{message}");
        }
        
        Console.ForegroundColor = originalColor;
        
        Environment.Exit(1);

        return new Exception();
    }
    
    public static void Warning(string message, Location? location = null)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        
        if (location?.SourceFile is not null)
        {
            var (start, end) = location.SourceFile.GetStartAndEndOfLine(location.Line);
            
            Console.WriteLine($"{location.SourceFile.FileInfo?.FullName}:({location.Line}:{location.Position - start})");
            Console.WriteLine($"\t{message}");

            Console.WriteLine();
            Console.WriteLine($"\t{location.SourceFile.Text[start..end].Replace("\n", "\\n").Replace("\r", "\\r")}");
            
            Console.Write("\t");
            for (var i = 0; i < location.Position - start - 1; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("^");
        }
        else
        {
            Console.WriteLine($"\t{message}");
        }
        
        Console.ForegroundColor = originalColor;
    }
}