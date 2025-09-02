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
            var (lineBeforeStart, lineBeforeEnd) = location.SourceFile.GetStartAndEndOfLine(location.Line - 1);
            var (start, end) = location.SourceFile.GetStartAndEndOfLine(location.Line);
            var (lineAfterStart, lineAfterEnd) = location.SourceFile.GetStartAndEndOfLine(location.Line + 1);
            
            Console.WriteLine($"{location.SourceFile.FileInfo?.FullName}:({location.Line}:{location.Position - start})");
            Console.WriteLine($"\t{message}");

            Console.WriteLine();
            
            if (lineBeforeStart >= 0)
            {
                Console.WriteLine($"|{location.Line - 1}\t{location.SourceFile.Text[lineBeforeStart..lineBeforeEnd].TrimEnd()}");
            }
            
            Console.WriteLine($"|{location.Line}\t{location.SourceFile.Text[start..end].TrimEnd()}");
            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("|\t");
            for (var i = 0; i < location.Position - start - 1; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("^");
            Console.ForegroundColor = ConsoleColor.Red;
            
            if (lineAfterStart >= 0)
            {
                Console.WriteLine($"|{location.Line + 1}\t{location.SourceFile.Text[lineAfterStart..lineAfterEnd].TrimEnd()}");
            }
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
            var (lineBeforeStart, lineBeforeEnd) = location.SourceFile.GetStartAndEndOfLine(location.Line - 1);
            var (start, end) = location.SourceFile.GetStartAndEndOfLine(location.Line);
            var (lineAfterStart, lineAfterEnd) = location.SourceFile.GetStartAndEndOfLine(location.Line + 1);
            
            Console.WriteLine($"{location.SourceFile.FileInfo?.FullName}:({location.Line}:{location.Position - start})");
            Console.WriteLine($"\t{message}");

            Console.WriteLine();
            
            if (lineBeforeStart >= 0)
            {
                Console.WriteLine($"|{location.Line - 1}\t{location.SourceFile.Text[lineBeforeStart..lineBeforeEnd].TrimEnd()}");
            }
            
            Console.WriteLine($"|{location.Line}\t{location.SourceFile.Text[start..end].TrimEnd()}");
            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("|\t");
            for (var i = 0; i < location.Position - start - 1; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("^");
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            if (lineAfterStart >= 0)
            {
                Console.WriteLine($"|{location.Line + 1}\t{location.SourceFile.Text[lineAfterStart..lineAfterEnd].TrimEnd()}");
            }
        }
        else
        {
            Console.WriteLine($"\t{message}");
        }
        
        Console.ForegroundColor = originalColor;
    }
}