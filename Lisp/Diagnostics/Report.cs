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
        if (location?.SourceFile is not null)
        {
            var originalForegroundColor = Console.ForegroundColor;

            if (location.SourceFile.FileInfo?.FullName is not null)
            {
                var dir = Path.GetDirectoryName(location.SourceFile.FileInfo?.FullName);
                var name = Path.GetFileName(location.SourceFile.FileInfo?.FullName);
            
                Console.Write(dir + Path.DirectorySeparatorChar);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(name);
                Console.ForegroundColor = originalForegroundColor;
                Console.WriteLine($":{location.Line}");
            }
            else
            {
                Console.WriteLine($"no_file:{location.Line}");
            }
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t{message}");
            Console.ForegroundColor = originalForegroundColor;
            
            Console.WriteLine();

            if (location.SourceFile.HasLine(location.Line - 1))
            {
                Console.WriteLine($"|{location.Line - 1}\t" + location.SourceFile.GetLineSpan(location.Line - 1).ToString());
            }
            
            var lineSpan = location.SourceFile.GetLineSpan(location.Line);
            Console.WriteLine($"|{location.Line}\t{lineSpan.ToString()}");
            
            if (location.SourceFile.HasLine(location.Line + 1))
            {
                Console.WriteLine($"|{location.Line + 1}\t" + location.SourceFile.GetLineSpan(location.Line + 1).ToString());
            }
        }
        else
        {
            Console.WriteLine($"{message}");
        }

        if (PreferThrownErrors)
        {
            throw new Exception(message);
        }
        
        Environment.Exit(1);
        return new Exception();
    }
}