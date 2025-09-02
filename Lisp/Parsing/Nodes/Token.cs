namespace Lisp.Parsing.Nodes;

// ; fadsf

public enum TokenType
{
    Decimal,
    Integer,
    StringLiteral,
    Boolean,
    Identifier,
    RestIdentifier,
}

public class Token : Node
{
    public required TokenType Type { get; set; }
    public required string Text { get; set; }
    public FileInfo? FileInfo { get; set; }
    public int? Line { get; set; }
    
    public override void Print(string indent, TextWriter? writer = null)
    {
        writer ??= Console.Out;
        var text = Type switch
        {
            TokenType.RestIdentifier => $"&{Text}",
            _ => Text,
        };
        writer.WriteLine($"{indent}[{Type}, |{text}|, {Line}]");
    }
}