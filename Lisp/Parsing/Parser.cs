using Lisp.Diagnostics;
using Lisp.Parsing.Nodes;

namespace Lisp.Parsing;

public class Parser
{
    private readonly SourceFile _sourceFile;
    private int _currentLine = 1;
    
    public Parser(SourceFile sourceFile)
    {
        _sourceFile = sourceFile;
    }
    
    public List<ListNode> Parse()
    {
        var listNode = new List<ListNode>();
        
        // read the entire file
        while (!_sourceFile.EndOfFile)
        {
            var c = _sourceFile.ReadChar();
            
            if (char.IsWhiteSpace(c)) continue;
            if (c is not '(')
            {
                var location = new Location
                {
                    SourceFile = _sourceFile,
                    Line = _currentLine,
                    Position = _sourceFile.CurrentPosition
                };
                
                Report.Error("This token was not a list.", location);
            }
        }

        return listNode;
    }

    private ListNode ReadTokens()
    {
        var listNode = new ListNode();

        var c = '\0';
            
        while (!_sourceFile.EndOfFile)
        {
            c = _sourceFile.ReadChar();
            
            if (c is ')') break;

            if (c is '\r' && _sourceFile.PeekChar() is '\n')
            {
                _sourceFile.ReadChar();
                _currentLine++;
            }
            else if (c is '\r' or '\n')
            {
                _currentLine++;
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
                listNode.Nodes.Add(newList);
            }
            else if (c is '"')
            {
                listNode.Nodes.Add(ReadStringLiteralToken(c));
            }
            else if (char.IsDigit(c) || (c is '.' && char.IsDigit(_sourceFile.PeekChar())))
            {
                listNode.Nodes.Add(ReadNumberToken(c));
            }
            else if (char.IsWhiteSpace(c))
            {
                SkipWhitespace();
            }
            else if (c is '(')
            {
                listNode.Nodes.Add(ReadTokens());
            }
            else
            {
                listNode.Nodes.Add(ReadIdentifierToken(c));
            }
        }

        if (_sourceFile.EndOfFile && c is not ')')
        {
            // throw error here
        } 
        
        return listNode;
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

        return new TokenNode
        {
            FileInfo = _sourceFile.FileInfo,
            Text = token,
            Line = _currentLine,
            Type = TokenType.StringLiteral
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
            return new()
            {
                FileInfo = _sourceFile.FileInfo,
                Text = token,
                Line = _currentLine,
                Type = TokenType.RestIdentifier
            };
        }
        
        return new()
        {
            FileInfo = _sourceFile.FileInfo,
            Text = token,
            Line = _currentLine,
            Type = TokenType.Identifier
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

        if (foundPoint)
        {
            return new TokenNode
            {
                FileInfo = _sourceFile.FileInfo,
                Text = token,
                Line = _currentLine,
                Type = TokenType.Decimal
            };
        }
        
        return new TokenNode
        {
            FileInfo = _sourceFile.FileInfo,
            Text = token,
            Line = _currentLine,
            Type = TokenType.Integer
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
                _currentLine++;
                break;
            }
            
            if (c is '\n' or '\r')
            {
                _currentLine++;
                break;
            }
        }
    }
}