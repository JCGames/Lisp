using System.Diagnostics.CodeAnalysis;
using Lisp.Exceptions;

namespace Lisp.Diagnostics;

public static class Report
{
    public static bool PreferThrownErrors = false;
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
            var lineBefore = location.SourceFile.GetStartAndEndOfLine(location.Line - 1);
            var line = location.SourceFile.GetStartAndEndOfLine(location.Line);
            var lineAfter = location.SourceFile.GetStartAndEndOfLine(location.Line + 1);
            
            // couldn't find the line
            if (line is null) throw new Exception(message);
            
            Console.WriteLine($"{location.SourceFile.FileInfo?.FullName ?? "no_file"}:{location.Line}:{location.Position - line.Value.start}");
            Console.WriteLine($"\t{message}");

            Console.WriteLine();
            
            if (lineBefore is not null)
            {
                Console.WriteLine($"|{location.Line - 1}\t{location.SourceFile.Text[lineBefore.Value.start..lineBefore.Value.end].TrimEnd()}");
            }
            
            Console.WriteLine($"|{location.Line}\t{location.SourceFile.Text[line.Value.start..line.Value.end].TrimEnd()}");
            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("|\t");
            for (var i = 0; i < location.Position - line.Value.start - 1; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("^");
            Console.ForegroundColor = ConsoleColor.Red;
            
            if (lineAfter is not null)
            {
                Console.WriteLine($"|{location.Line + 1}\t{location.SourceFile.Text[lineAfter.Value.start..lineAfter.Value.end].TrimEnd()}");
            }
        }
        else
        {
            Console.WriteLine($"\t{message}");
        }
        
        Console.ForegroundColor = originalColor;

        if (PreferThrownErrors)
        {
            throw new Exception(message);    
        }
        else
        {
            Environment.Exit(1);    
        }
        
        return new Exception();
    }
    
    public static void Warning(string message, Location? location = null)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        
        if (location?.SourceFile is not null)
        {
            var lineBefore = location.SourceFile.GetStartAndEndOfLine(location.Line - 1);
            var line = location.SourceFile.GetStartAndEndOfLine(location.Line);
            var lineAfter = location.SourceFile.GetStartAndEndOfLine(location.Line + 1);
            
            // couldn't find the line
            if (line is null) throw new Exception(message);
            
            Console.WriteLine($"{location.SourceFile.FileInfo?.FullName ?? "no_file"}:{location.Line}:{location.Position - line.Value.start}");
            Console.WriteLine($"\t{message}");

            Console.WriteLine();
            
            if (lineBefore is not null)
            {
                Console.WriteLine($"|{location.Line - 1}\t{location.SourceFile.Text[lineBefore.Value.start..lineBefore.Value.end].TrimEnd()}");
            }
            
            Console.WriteLine($"|{location.Line}\t{location.SourceFile.Text[line.Value.start..line.Value.end].TrimEnd()}");
            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("|\t");
            for (var i = 0; i < location.Position - line.Value.start - 1; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("^");
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            if (lineAfter is not null)
            {
                Console.WriteLine($"|{location.Line + 1}\t{location.SourceFile.Text[lineAfter.Value.start..lineAfter.Value.end].TrimEnd()}");
            }
        }
        else
        {
            Console.WriteLine($"\t{message}");
        }
        
        Console.ForegroundColor = originalColor;
    }
}