namespace Lisp.Parsing.Nodes;

public enum TokenType
{
    Decimal,
    Integer,
    StringLiteral,
    Identifier,
    RestIdentifier
}

public class TokenNode : Node
{
    public required TokenType Type { get; init; }
    public required string Text { get; init; }
    
    public FileInfo? FileInfo { get; init; }
    public int? Line { get; init; }
    
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