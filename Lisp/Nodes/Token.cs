namespace Lisp.Nodes;

// ; fadsf

public enum TokenType
{
    Decimal,
    Integer,
    StringLiteral,
    Comment,
    Whitespace,
    NewLine,
    Identifier
}

public class Token : Node
{
    public TokenType Type { get; set; }
    public required FileInfo? FileInfo { get; set; }
    public required string Text { get; set; }
    public required int Line { get; set; }
    
    public override void Print(string indent)
    {
        Console.WriteLine($"{indent}[{Type}, |{Text}|, {Line}]");
    }
}