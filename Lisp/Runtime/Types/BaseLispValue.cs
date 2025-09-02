namespace Lisp.Types;

/// <summary>
/// A base LispValue represents everything that can exist in lisp.
/// </summary>
public abstract class BaseLispValue
{
    public abstract override string ToString();
    
    protected abstract bool Equals(BaseLispValue other);

    public override bool Equals(object? obj)
    {
        return obj is BaseLispValue value && Equals(value);
    }
    
    public abstract override int GetHashCode();
}