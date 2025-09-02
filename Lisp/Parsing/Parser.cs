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
        var list = new List<ListNode>();
        
        while (!_sourceFile.EndOfFile)
        {
            var c = _sourceFile.ReadChar();

            if (char.IsWhiteSpace(c)) continue;

            if (c is ';')
            {
                SkipComment();
                continue;
            }
            
            if (c is not '(')
            {
                var location = new Location
                {
                    Line = _sourceFile.CurrentLine,
                    Position = _sourceFile.CurrentPosition,
                    SourceFile = _sourceFile
                };
                
                Report.Error("A list must start with an open parenthesis.", location);
            }

            list.Add(ReadTokens());
        }

        return list;
    }

    private ListNode ReadTokens()
    {
        var list = new ListNode
        {
            Location = new Location
            {
                SourceFile = _sourceFile,
                Line = _sourceFile.CurrentLine,
                Position = _sourceFile.CurrentPosition
            }
        };

        var c = '\0';
            
        while (!_sourceFile.EndOfFile)
        {
            c = _sourceFile.ReadChar();
            
            if (c is ')') break;

            var errorLocation = new Location
            {
                Line = _sourceFile.CurrentLine,
                Position = _sourceFile.CurrentPosition,
                SourceFile = _sourceFile
            };

            if (ReadNode(c) is { } value)
            {
                list.Nodes.Add(value);
            }
        }

        if (_sourceFile.EndOfFile && c is not ')')
        {
            Report.Error("A list must close with a closing parenthesis.", list.Location);
        } 
        
        return list;
    }

    private Node? ReadNode(char c)
    {
        if (c is '\r' && _sourceFile.PeekChar() is '\n')
        {
            _sourceFile.ReadChar();
        }
        else if (c is '\r' or '\n')
        {
        }
        else if (c is ';')
        {
            SkipComment();
        }
        else if (c is '\'' && _sourceFile.PeekChar() is '(')
        {
            _sourceFile.ReadChar();
                
            var newList = ReadTokens();
            newList.IsQuoted = true;
            return newList;
        }
        else if (c is '"')
        {
            return ReadStringLiteralToken(c);
        }
        else if (char.IsDigit(c) || (c is '.' or '-' && char.IsDigit(_sourceFile.PeekChar())))
        {
            return ReadNumberToken(c);
        }
        else if (char.IsWhiteSpace(c))
        {
            SkipWhitespace();
        }
        else if (c is '(')
        {
            return ReadTokens();
        }
        else if (c is '{')
        {
            ReadStruct();
        }
        else
        {
            return ReadIdentifierToken(c);
        }

        return null;
    }

    private StructNode ReadStruct()
    {
        var structLocation = new Location
        {
            SourceFile = _sourceFile,
            Line = _sourceFile.CurrentLine,
            Position = _sourceFile.CurrentPosition
        };

        var structNode = new StructNode
        {
            Location = structLocation,
            Struct = []
        };

        var c = '\0';
        
        while (!_sourceFile.EndOfFile)
        {
            c = ReadToNonWhitespace();
            if (c is '}') break;

            var node = ReadNode(c);

            if (node is null)
            {
                var location = new Location
                {
                    SourceFile = _sourceFile,
                    Line = _sourceFile.CurrentLine,
                    Position = _sourceFile.CurrentPosition
                };
                
                throw Report.Error("Must be a label.", location);
            }
            
            if (node is not IdentifierNode label || label.Text.Length < 2 || !label.Text.EndsWith(':'))
            {
                throw Report.Error("Must be a label.", node.Location);
            }
            
            label.Text = label.Text[..^1];
            
            c = ReadToNonWhitespaceRespectEOL();

            if (c is '}')
            {
                throw Report.Error("Must have a value.", label.Location);
            }

            node = ReadNode(c);

            if (node is null)
            {
                throw Report.Error("Must have a value.", label.Location);
            }
            
            structNode.Struct.Add(new KeyValueNode
            {
                Location = new Location
                {
                    SourceFile = _sourceFile,
                    Line = _sourceFile.CurrentLine,
                    Position = _sourceFile.CurrentPosition
                },
                Key = label,
                Value = node
            });
        }
        
        if (_sourceFile.EndOfFile && c is not '}')
        {
            Report.Error("Missing closing bracket.", structLocation);
        }
        
        return structNode;
    }
    
    private TokenNode ReadStringLiteralToken(char startQuote)
    {
        var stringLiteralLocation = new Location
        {
            SourceFile = _sourceFile,
            Line = _sourceFile.CurrentLine,
            Position = _sourceFile.CurrentPosition
        };
        
        var token = string.Empty;
        var c = '\0';
        
        while (!_sourceFile.EndOfFile)
        {
            c = _sourceFile.ReadChar();

            if ((c is '\r' && _sourceFile.PeekChar() is '\n')
                || c is '\r' or '\n')
            {
                Report.Error("Missing closing quote.", stringLiteralLocation);
            }

            if (c == startQuote) break;
            
            token += c;
        }

        if (_sourceFile.EndOfFile && c != startQuote)
        {
            Report.Error("Missing closing quote.", stringLiteralLocation);
        }

        return new StringLiteralNode
        {
            Location = new Location
            {
                Line = _sourceFile.CurrentLine,
                Position = _sourceFile.CurrentPosition,
                SourceFile = _sourceFile
            },
            Text = token
        };
    }

    private TokenNode ReadIdentifierToken(char firstCharacter)
    {
        var token = firstCharacter.ToString();
        
        while (!_sourceFile.EndOfFile && !char.IsWhiteSpace(_sourceFile.PeekChar()) && _sourceFile.PeekChar() is not '(' and not ')')
        {
            token += _sourceFile.ReadChar();
        }
        
        if (token.StartsWith('&'))
        {
            return new RestIdentifierNode()
            {
                Location = new Location
                {
                    Line = _sourceFile.CurrentLine,
                    Position = _sourceFile.CurrentPosition,
                    SourceFile = _sourceFile
                },
                Text = token[1..]
            };
        }
        
        return new IdentifierNode
        {
            Location = new Location
            {
                Line = _sourceFile.CurrentLine,
                Position = _sourceFile.CurrentPosition,
                SourceFile = _sourceFile
            },
            Text = token
        };
    }

    private TokenNode ReadNumberToken(char firstCharacter)
    {
        var token = firstCharacter.ToString();
        var foundPoint = false;
        
        while (!_sourceFile.EndOfFile && char.IsNumber(_sourceFile.PeekChar()) || _sourceFile.PeekChar() is '.' or ',')
        {
            var c = _sourceFile.ReadChar();
            
            if (c is '.' && !foundPoint)
            {
                foundPoint = true;
            }
            else if (c is '.' && foundPoint)
            {
                Report.Error("Too many decimal points.", new Location
                {
                    SourceFile = _sourceFile,
                    Line = _sourceFile.CurrentLine,
                    Position = _sourceFile.CurrentPosition
                });
            }
            
            token += c;
        }

        return new NumberLiteralNode
        {
            Location = new Location
            {
                Line = _sourceFile.CurrentLine,
                Position = _sourceFile.CurrentPosition,
                SourceFile = _sourceFile
            },
            Text = token
        };
    }

    private void SkipWhitespace()
    {
        while (!_sourceFile.EndOfFile && char.IsWhiteSpace(_sourceFile.PeekChar()))
        {
            _sourceFile.ReadChar();
        }
    }

    private void SkipComment()
    {
        while (!_sourceFile.EndOfFile)
        {
            var c = _sourceFile.ReadChar();

            if (c is '\r' && _sourceFile.PeekChar() is '\n')
            {
                _sourceFile.ReadChar();
                break;
            }
            
            if (c is '\n' or '\r')
            {
                break;
            }
        }
    }

    private char ReadToNonWhitespace()
    {
        while (!_sourceFile.EndOfFile)
        {
            if (!char.IsWhiteSpace(_sourceFile.PeekChar())) break;
            
            _sourceFile.ReadChar();
        }
        
        return _sourceFile.ReadChar();
    }
    
    private char ReadToNonWhitespaceRespectEOL()
    {
        while (!_sourceFile.EndOfFile)
        {
            if (_sourceFile.PeekChar() is '\r' or '\n')
            {
                break;
            }
            
            if (!char.IsWhiteSpace(_sourceFile.PeekChar())) break;
            
            _sourceFile.ReadChar();
        }
        
        return _sourceFile.ReadChar();
    }
}