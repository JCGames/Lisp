using Lisp.Diagnostics;

namespace Lisp.Types;

public interface ICollectionLispValue
{
    public Type KeyType { get; }
    public LispValue? GetValue(LispValue key);
}
public interface ICollectionLispValue<in TKey> : ICollectionLispValue
    where TKey : LispValue
{
    public LispValue? GetValue(TKey key);
    LispValue? ICollectionLispValue.GetValue(LispValue key)
    {
        if (key is not TKey typedKey) throw new InvalidOperationException("Invalid keyType");
        
        return GetValue(typedKey);
        
    }
    
    Type ICollectionLispValue.KeyType => typeof(TKey);

}