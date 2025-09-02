namespace Lisp.Parsing.Nodes;


public abstract class TokenNode : Node
{
    public required string Text { get; set; }
    public FileInfo? FileInfo { get; set; }
    public int? Line { get; set; }
    
    public override void Print(string indent, TextWriter? writer = null)
    {
        writer ??= Console.Out;
        writer.WriteLine($"{indent}[{GetType().Name}, |{Text}|, {Line}]");
    }
}