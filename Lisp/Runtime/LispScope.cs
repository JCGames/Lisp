using System.Collections.Immutable;
using Lisp.Diagnostics;
using Lisp.Exceptions;
using Lisp.Types;

namespace Lisp;

public class LispScope
{
    private ImmutableDictionary<string, LispValue> _scope = ImmutableDictionary<string, LispValue>.Empty;
    private LispScope _global;
    private readonly LispScope? _parent;

    public LispScope()
    {
        _global = CreateGlobal();
    }

    private LispScope(LispScope? global, LispScope? parent)
    {
        _global = global!;
        _parent = parent;
    }

    private static LispScope CreateGlobal()
    {
        var global = new LispScope(null, null);
        global._global = global;
        return global;
    }

    public LispScope PushScope()
    {
        return new(_global, this);
    }

    public LispScope PopScope()
    {
        return _parent ?? throw new InvalidOperationException("Cannot pop scope.");
    }

    public BaseLispValue? Read(string identifier)
    {
        var scope = this;
        LispValue? value;
        while (!scope._scope.TryGetValue(identifier, out value))
        {
            scope = scope._parent;
            if (scope == null) break;
        }

        if (value is null)
        {
            _global._scope.TryGetValue(identifier, out value);
        }
        
        return value;
    }

    public void UpdateScope(string identifier, LispValue value)
    {
        _scope = _scope.SetItem(identifier, value);
    }

    public void UpdateGlobalScope(string identifier, LispValue value)
    {
        _global.UpdateScope(identifier, value);
    }
}