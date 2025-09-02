namespace Lisp.Parsing.Nodes;

public abstract class Node
{
    public abstract void Print(string indent, TextWriter? writer = null);
}