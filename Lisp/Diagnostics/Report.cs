namespace Lisp.Diagnostics;

public static class Report
{
    private static List<string> _errors = [];

    public static void Error(string message, Location? location = null)
    {
        Environment.Exit(1);
    }
}