namespace Lisp.Parsing.Nodes.Classifications;

/// <summary>
/// Nodes that can be used as a parameter
/// </summary>
public interface IParameterNode : INode
{
    public string PublicParameterName { get; }
    public string PrivateParameterName => PublicParameterName;
}