namespace Lisp.Parsing.Nodes;

public class NumberLiteralNode : TokenNode
{
    public decimal Value => decimal.Parse(Text);
}