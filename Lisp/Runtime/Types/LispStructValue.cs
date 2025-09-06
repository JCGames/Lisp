using System.Diagnostics.CodeAnalysis;
using System.Text;
using Lisp.Diagnostics;
using Lisp.Parsing.Nodes;

namespace Lisp.Types;

public class LispStructValue : LispValue, ICollectionLispValue<LispStringValue>
{
    public Dictionary<LispStringValue, LispValue> Value { get; } = new();

    public LispStructValue(Dictionary<LispStringValue, LispValue> value)
    {
        Value = value;
    }

    public LispStructValue(IEnumerable<KeyValueNode> nodes, LispScope scope)
    {
        foreach (var item in nodes)
        {
            var value = Runner.EvaluateNode(item.Value, scope);
            if (value is not LispValue lispValue) throw Report.Error($"{value} cannot be stored in a struct", item.Location);
            
            Value.Add(new(item.Key.Text), lispValue);
        }
    }
    
    public LispValue? GetValue(LispStringValue key)
    {
        Value.TryGetValue(new(key.Value), out var value);
        return value;
    }
    
    public override string ToString()
    {
        var builder = new StringBuilder();
        
        builder.Append('{');
        foreach (var (key, value) in Value)
        {
            builder.Append($"\t{key}: {value}");
        }
        builder.Append('}');
        
        return builder.ToString();
    }

    [SuppressMessage("ReSharper", "UsageOfDefaultStructEquality")]
    protected override bool Equals(BaseLispValue other) =>
        other is LispStructValue structValue
        && Value.Count == structValue.Value.Count
        && !Value.Except(structValue.Value).Any();

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var (key, value) in Value)
        {
            hash.Add(key);
            hash.Add(value);
        }
        return hash.ToHashCode();
    }

    public LispValue First()
    {
        var value = Value.First();
        return new LispTupleValue((value.Key, value.Value));
    }

    public ICollectionLispValue<LispStringValue> Rest()
    {
        throw new NotImplementedException();
    }

    public LispNumberValue Count() => new(Value.Count);
}