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
        var list = new ListNode();

        var c = '\0';
            
        while (!_sourceFile.EndOfFile)
        {
            c = _sourceFile.ReadChar();
            
            if (c is ')') break;

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
                list.Nodes.Add(newList);
            }
            else if (c is '"')
            {
                list.Nodes.Add(ReadStringLiteralToken(c));
            }
            else if (char.IsDigit(c) || (c is '.' && char.IsDigit(_sourceFile.PeekChar())))
            {
                list.Nodes.Add(ReadNumberToken(c));
            }
            else if (char.IsWhiteSpace(c))
            {
                SkipWhitespace();
            }
            else if (c is '(')
            {
                list.Nodes.Add(ReadTokens());
            }
            else
            {
                list.Nodes.Add(ReadIdentifierToken(c));
            }
        }

        if (_sourceFile.EndOfFile && c is not ')')
        {
            // throw error here
        } 
        
        return list;
    }
    
    private TokenNode ReadStringLiteralToken(char startQuote)
    {
        var token = string.Empty;
        var c = '\0';
        
        while (!_sourceFile.EndOfFile)
        {
            c = _sourceFile.ReadChar();

            if (c == startQuote) break;
            
            token += c;
        }

        if (_sourceFile.EndOfFile && c != startQuote)
        {
            // throw error
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
                // throw error
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
}