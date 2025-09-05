namespace Lisp.Types;

public class LispSymbolValue : LispStringValue
{
    private static readonly Dictionary<string, LispSymbolValue> Symbols = new();

    private LispSymbolValue(string value) : base(string.Intern(value))
    { }

    public static LispSymbolValue New(string value)
    {
        if (Symbols.TryGetValue(value, out var existing)) return existing;
        
        var newSymbol = new LispSymbolValue(value);
        Symbols.Add(newSymbol.Value, newSymbol);
        return newSymbol;
    }

    public override string ToString() => Value;

    protected override bool Equals(BaseLispValue other) => ReferenceEquals(this, other);

    public override int GetHashCode() => Value.GetHashCode();
}