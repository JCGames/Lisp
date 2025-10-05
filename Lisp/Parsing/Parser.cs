using Lisp.Diagnostics;
using Lisp.Parsing.Nodes;

namespace Lisp.Parsing;

public class Parser
{
    private readonly SourceFile _sourceFile;
    
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
                Report.Error("Only lisps are allowed on the top level.", new Location
                {
                    Start = _sourceFile.Current,
                    End = _sourceFile.Current,
                    Line = _sourceFile.CurrentLine,
                    SourceFile = _sourceFile
                });
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
        else
        {
            // probably just a token then
            return ReadIdentifierToken();
        }

        return null;
    }

    private StructNode ReadStruct()
    {
        var structLocation = new Location
        {
            SourceFile = _sourceFile,
            Line = _sourceFile.CurrentLine,
            Start = _sourceFile.CurrentPosition,
            End = _sourceFile.EndOfFilePosition
        };
    
        var structNode = new StructNode
        {
            Location = structLocation,
            Struct = []
        };
        
        _sourceFile.MoveNext();
        
        while (!_sourceFile.EndOfFile)
        {
            _sourceFile.MoveToNonWhiteSpaceCharacter();
            if (_sourceFile.Current is '}' || _sourceFile.EndOfFile) break;
            
            var location = Location.New(_sourceFile);
            var node = ReadNode();
    
            if (node is null)
            {
                throw Report.Error("Must be a label.", Location.New(_sourceFile));
            }
            
            if (node is not IdentifierNode label || label.Text.Length < 2 || !label.Text.EndsWith(':'))
            {
                throw Report.Error("Must be a label.", node.Location);
            }
            
            label.Text = label.Text[..^1];

            // skip whitespace but respect end of lines
            while (_sourceFile is { EndOfFile: false, IsNewLine: false } && char.IsWhiteSpace(_sourceFile.Current))
            {
                _sourceFile.MoveNext();
            }

            if (_sourceFile.EndOfFile) break;
    
            if (_sourceFile.Current is '}')
            {
                throw Report.Error("Must have a value.", label.Location);
            }
    
            node = ReadNode();
            location.End = _sourceFile.CurrentPosition - 1;
    
            if (node is null)
            {
                throw Report.Error("Must have a value.", label.Location);
            }
            
            structNode.Struct.Add(new KeyValueNode
            {
                Location = location,
                Key = label,
                Value = node
            });
        }
        
        if (_sourceFile is { EndOfFile: true, Current: not '}' })
        {
            Report.Error("Missing closing bracket.", structLocation);
        }
        
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
                Report.Error("This is not correct syntax.", new Location
                {
                    Start = _sourceFile.Current,
                    End = _sourceFile.Current,
                    Line = _sourceFile.CurrentLine,
                    SourceFile = _sourceFile
                });
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
               && _sourceFile.Peek is not '(' and not ')')
        {
            _sourceFile.MoveNext();
            token += _sourceFile.Current;
        }
        
        location.End = _sourceFile.CurrentPosition;
        
        _sourceFile.MoveNext();
        
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