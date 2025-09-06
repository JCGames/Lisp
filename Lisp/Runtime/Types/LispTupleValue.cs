namespace Lisp.Types;

public class LispTupleValue : LispValue
{
    public (LispValue left, LispValue right) Value { get; }

    public LispTupleValue((LispValue left, LispValue right) value)
    {
        Value = value;
    }

    public override string ToString() => $"({Value.left} . {Value.right})";

    protected override bool Equals(BaseLispValue other) =>
        other is LispTupleValue tuple
        && Value.Equals(tuple.Value);

    public override int GetHashCode() => Value.GetHashCode();
}