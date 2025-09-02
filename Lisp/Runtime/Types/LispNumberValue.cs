namespace Lisp.Types;

public class LispNumberValue : LispValue
{
    public decimal Value { get; }

    public LispNumberValue(decimal value)
    {
        Value = value;
    }
    
    public LispNumberValue(int value)
    {
        Value = value;
    }
    
    public override string ToString() => Value.ToString();

    protected override bool Equals(BaseLispValue other) => 
        other is LispNumberValue lispDecimal
        && Value == lispDecimal.Value;

    public override int GetHashCode() => Value.GetHashCode();
}