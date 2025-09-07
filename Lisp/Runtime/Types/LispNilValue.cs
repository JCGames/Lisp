namespace Lisp.Types;

public class LispNilValue : LispValue
{
    public static LispNilValue Instance { get; } = new();
    private LispNilValue()
    { }
    
    public override string ToString() => "nil";

    protected override bool Equals(BaseLispValue other) => false;

    public override int GetHashCode() => 0;
}