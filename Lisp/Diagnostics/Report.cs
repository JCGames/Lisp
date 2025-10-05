using System.Diagnostics.CodeAnalysis;
using Lisp.Exceptions;

namespace Lisp.Diagnostics;

public static class Report
{
    public static bool PreferThrownErrors = false;

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
            Console.WriteLine($"{location.SourceFile.FileInfo?.FullName ?? "no_file"}:{location.Line}");
            Console.WriteLine($"\t{message}");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"{message}");
        }
        
        Console.ForegroundColor = originalColor;

        if (PreferThrownErrors)
        {
            throw new Exception(message);
        }
        
        Environment.Exit(1);
        return new Exception();
    }
}