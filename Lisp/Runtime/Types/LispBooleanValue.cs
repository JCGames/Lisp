namespace Lisp.Types;

public class LispBooleanValue : LispValue
{
    public bool Value { get; }

    public LispBooleanValue(bool value)
    {
        Value = value;
    }
    
    public override string ToString() => Value.ToString();

    protected override bool Equals(BaseLispValue other) =>
        other is LispBooleanValue lispBoolean
        && Value == lispBoolean.Value;

    public override int GetHashCode() => Value.GetHashCode();
}