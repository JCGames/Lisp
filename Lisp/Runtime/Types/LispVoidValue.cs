namespace Lisp.Types;

/// <summary>
/// The void type represents the absence of any value.
/// </summary>
public class LispVoidValue : BaseLispValue
{
    public override string ToString() => "(void)";

    protected override bool Equals(BaseLispValue other) => false;

    public override int GetHashCode() => 0;
}