namespace Lisp.Types;

public class LispStringValue : LispValue
{
    public string Value { get; }

    public LispStringValue(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public override string ToString() => Value;

    protected override bool Equals(BaseLispValue other) =>
        other is LispStringValue lispString
        && Value == lispString.Value;

    public override int GetHashCode() => Value.GetHashCode();
}