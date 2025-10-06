using Lisp.Diagnostics;
using Lisp.Parsing.Nodes;

namespace Lisp.Parsing;

public class Parser
{
    private readonly SourceFile _sourceFile;

    private readonly Func<char, bool> _isCharacterAllowedInIdentifier = c =>
        c is not ('(' or ')' or '{' or '}' or '"');
    
    public Parser(SourceFile sourceFile)
    {
        _sourceFile = sourceFile;
    }
    
    public List<ListNode> Parse()
    {
        var nodes = new List<ListNode>();

        while (!_sourceFile.EndOfFile)
        {
            _sourceFile.MoveToNonWhiteSpaceCharacter();
            if (_sourceFile.EndOfFile) break;

            // comment
            if (_sourceFile.Current is ';')
            {
                _sourceFile.MoveToNextLine();
                if (_sourceFile.EndOfFile) break;
                continue;
            }
            
            if (ReadNode() is ListNode node)
            {
                nodes.Add(node);
            }
            else
            {
                Report.Error("Only lisps are allowed on the top level.", Location.New(_sourceFile));
            }
        }

        return nodes;
    }

    private Node? ReadNode()
    {
        if (_sourceFile.IsNewLine)
        {
            _sourceFile.MoveNext();
        }
        else if (_sourceFile.Current is '(')
        {
            // we found a lisp
            return ReadListNode();
        }
        else if (_sourceFile.Current is '"')
        {
            return ReadStringLiteralToken();
        }
        else if (char.IsDigit(_sourceFile.Current) 
                 || (_sourceFile.Current is '.' or '-' && char.IsDigit(_sourceFile.Peek)))
        {
            return ReadNumberToken();
        }
        else if (_sourceFile.Current is '{')
        {
            return ReadStruct();
        }
        else if (_isCharacterAllowedInIdentifier(_sourceFile.Current))
        {
            // probably just a token then
            return ReadIdentifierToken();
        }

        return null;
    }

    private StructNode ReadStruct()
    {
        var structLocation = Location.New(_sourceFile);
        
        var structNode = new StructNode
        {
            Location = structLocation,
            Struct = []
        };
        
        _sourceFile.MoveNext();
        
        while (!_sourceFile.EndOfFile)
        {
            _sourceFile.MoveToNonWhiteSpaceCharacter();
            if (_sourceFile.EndOfFile) break;
            if (_sourceFile.Current is '}') break;

            var label = ReadNode();

            // ensure that the node is a properly formatted label
            if (label is not IdentifierNode identifier || identifier.Text.Length < 2 || !identifier.Text.EndsWith(':'))
                throw Report.Error("This should be a label (i.e., \"name:\").", label?.Location ?? Location.New(_sourceFile));
            
            _sourceFile.MoveToNonWhiteSpaceCharacter();
            if (_sourceFile.EndOfFile) break;
            if (_sourceFile.Current is '}') break;
            
            var value = ReadNode();

            if (value is null) throw Report.Error("This should be a value.", Location.New(_sourceFile));

            identifier.Text = identifier.Text.TrimEnd(':');
            
            structNode.Struct.Add(new KeyValueNode
            {
                Location = Location.New(_sourceFile),
                Key = identifier,
                Value = value
            });
        }
        
        structLocation.End = _sourceFile.CurrentPosition;

        if (_sourceFile.Current is not '}')
        {
            Report.Error("The structure does not end with a closing bracket.", structLocation);
        }
        
        _sourceFile.MoveNext();
        
        return structNode;
    }
    
    private ListNode ReadListNode()
    {
        // the first character should be (
        
        var listNode = new ListNode
        {
            Location = Location.New(_sourceFile),
        };
        
        // move past the (
        _sourceFile.MoveNext();
        
        while (_sourceFile is { EndOfFile: false, Current: not ')' })
        {
            // move to the first token
            _sourceFile.MoveToNonWhiteSpaceCharacter();
            if (_sourceFile.EndOfFile) break;
            
            // comment
            if (_sourceFile.Current is ';')
            {
                _sourceFile.MoveToNextLine();
                if (_sourceFile.EndOfFile) break;
                continue;
            }
            
            if (ReadNode() is { } node)
            {
                listNode.Nodes.Add(node);
            }
            else
            {
                Report.Error("This is not correct syntax.", Location.New(_sourceFile));
            }
        }
        
        // move past the )
        _sourceFile.MoveNext();
        
        return listNode;
    }
    
    private StringLiteralNode ReadStringLiteralToken()
    {
        var startQuote = _sourceFile.Current;
        var stringLiteralLocation = Location.New(_sourceFile);
        var token = string.Empty;
        
        _sourceFile.MoveNext();
        
        while (_sourceFile is { EndOfFile: false, IsNewLine: false } && _sourceFile.Current != startQuote)
        {
            token += _sourceFile.Current;
            _sourceFile.MoveNext();
        }

        if (_sourceFile.IsNewLine || (_sourceFile.EndOfFile && _sourceFile.Current != startQuote))
        {
            stringLiteralLocation.End = _sourceFile.CurrentPosition;
            Report.Error("Missing closing quote.", stringLiteralLocation);
        }

        stringLiteralLocation.End = _sourceFile.CurrentPosition;
        
        _sourceFile.MoveNext();
        
        return new StringLiteralNode
        {
            Location = stringLiteralLocation,
            Text = token
        };
    }

    private TokenNode ReadIdentifierToken()
    {
        var location = Location.New(_sourceFile);
        var token = _sourceFile.Current.ToString();
        
        while (!_sourceFile.EndOfFile 
               && !char.IsWhiteSpace(_sourceFile.Peek) 
               && _isCharacterAllowedInIdentifier(_sourceFile.Peek))
        {
            _sourceFile.MoveNext();
            token += _sourceFile.Current;
        }
        
        location.End = _sourceFile.CurrentPosition;
        _sourceFile.MoveNext();

        var dotSplit = token.Split('.');
        var start = location.Start;

        if (dotSplit.Length > 1)
        {
            return new AccessorNode
            {
                Text = token,
                Location = location,
                Identifiers = dotSplit.Select(x =>
                {
                    var loc = new Location
                    {
                        Start = start,
                        End = start + x.Length - 1,
                        Line = location.Line,
                        SourceFile = location.SourceFile,
                    };

                    start += x.Length + 1;
                    return new IdentifierNode
                    {
                        Location = loc,
                        Text = x
                    };
                }).ToList()
            };
        }
        
        if (token.StartsWith('&'))
        {
            return new RestIdentifierNode
            {
                Location = location,
                Text = token[1..]
            };
        }
        
        if (token.StartsWith('\''))
        {
            return new SymbolNode
            {
                Location = location,
                Text = token[1..]
            };
        }
        
        return new IdentifierNode
        {
            Location = location,
            Text = token
        };
    }

    private NumberLiteralNode ReadNumberToken()
    {
        var location = Location.New(_sourceFile);
        var number = _sourceFile.Current.ToString();
        var foundPoint = false;
        
        while (!_sourceFile.EndOfFile 
               && (char.IsNumber(_sourceFile.Peek) || _sourceFile.Peek is '.' or ','))
        {
            _sourceFile.MoveNext();
            if (_sourceFile.Current is ',' && !char.IsNumber(_sourceFile.Peek))
            {
                location.End = _sourceFile.CurrentPosition;
                throw Report.Error("A digit should proceed a comma.", location);
            }
            
            switch (_sourceFile.Current)
            {
                case '.' when !foundPoint:
                    foundPoint = true;
                    break;
                case '.' when foundPoint:
                    Report.Error("Too many decimal points.", Location.New(_sourceFile));
                    break;
            }
            
            number += _sourceFile.Current;
        }

        location.End = _sourceFile.CurrentPosition;
        
        _sourceFile.MoveNext();
        
        return new NumberLiteralNode
        {
            Location = location,
            Text = number
        };
    }
}